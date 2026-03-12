using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using sistema_estoque.core.models;
using sistema_estoque.infrastructure.database;

namespace sistema_estoque.infrastructure.repositories
{
    public class ProdutoRepository
    {
        private readonly DatabaseConnection _database = new DatabaseConnection();

        public void CriarProduto(Produto produto)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = @"INSERT INTO produtos (usuario_id, nome, descricao, quantidade_atual, valor_unitario, data_criacao) 
                            VALUES (@usuarioId, @nome, @descricao, @quantidade, @valor, @data)";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuarioId", produto.UsuarioId);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@quantidade", produto.QuantidadeAtual);
            cmd.Parameters.AddWithValue("@valor", produto.ValorUnitario);
            cmd.Parameters.AddWithValue("@data", produto.DataCriacao);

            cmd.ExecuteNonQuery();
        }

        public List<Produto> ListarProdutos(int usuarioId)
        {
            var lista = new List<Produto>();
            using var conn = _database.GetConnection();
            conn.Open();
            string query = "SELECT id, usuario_id, nome, descricao, quantidade_atual, valor_unitario, data_criacao FROM produtos WHERE usuario_id = @usuarioId";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuarioId", usuarioId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var p = new Produto(
                    reader.GetInt32("usuario_id"),
                    reader.GetString("nome"),
                    reader.GetString("descricao"),
                    reader.GetInt32("quantidade_atual"),
                    reader.GetDecimal("valor_unitario")
                )
                {
                    Id = reader.GetInt32("id"),
                    DataCriacao = reader.GetDateTime("data_criacao")
                };
                lista.Add(p);
            }

            return lista;
        }

        public void AtualizarProduto(Produto produto)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = @"UPDATE produtos 
                    SET nome = @nome, descricao = @descricao, valor_unitario = @valor 
                    WHERE id = @id AND usuario_id = @usuarioId";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", produto.Id);
            cmd.Parameters.AddWithValue("@usuarioId", produto.UsuarioId);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@valor", produto.ValorUnitario);

            cmd.ExecuteNonQuery();
        }

        public void AtualizarSaldo(int produtoId, int qtd, string operacao)
        {
            using var conn = _database.GetConnection();
            conn.Open();
            string query = $"UPDATE produtos SET quantidade_atual = quantidade_atual {operacao} @qtd WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@qtd", qtd);
            cmd.Parameters.AddWithValue("@id", produtoId);
            cmd.ExecuteNonQuery();
        }
    }
}
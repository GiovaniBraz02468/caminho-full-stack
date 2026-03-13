using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using sistema_estoque.core.models;
using sistema_estoque.infrastructure.database;

namespace sistema_estoque.infrastructure.repositories
{
    /// <summary>
    /// Classe responspavel por realizar a troca de dados entre o sistema e o banco de dados
    /// Utiliza o modelo Produto
    /// </summary>
    public class ProdutoRepository
    {
        //Variáveis auxiliares
        private readonly DatabaseConnection _database = new DatabaseConnection();

        //Métodos

        /// <summary>
        /// Função responsável por salvar um produto no banco de dados
        /// </summary>
        /// <param name="produto"></param>
        public int CriarProduto(Produto produto)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = @"INSERT INTO produtos (usuario_id, nome, descricao, quantidade_atual, valor_unitario, data_criacao) 
                    VALUES (@usuarioId, @nome, @descricao, @quantidade, @valor, @data);
                    SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuarioId", produto.UsuarioId);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@descricao", produto.Descricao);
            cmd.Parameters.AddWithValue("@quantidade", produto.QuantidadeAtual);
            cmd.Parameters.AddWithValue("@valor", produto.ValorUnitario);
            cmd.Parameters.AddWithValue("@data", produto.DataCriacao);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// Função responsável por listar todos os produtos do usuário logado
        /// </summary>
        /// <param name="usuarioId">Id do usuário logado, para que seja realizado o filtro por usuário</param>
        /// <returns></returns>
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

        /// <summary>
        /// Função responsável por atualizar um produto no banco de dados
        /// </summary>
        /// <param name="produto">Instancia do modelo produto, com os dados preenchidos para realizar a atualização</param>
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

        /// <summary>
        /// Função que atualiza saldo do produto e valor médio a cada movimentação
        /// </summary>
        /// <param name="produtoId">Id do produto para atualizar os dados</param>
        /// <param name="qtd">Quantidade para remover ou adicionar</param>
        /// <param name="valorMovimentacao">Valor dos itens da movimentação</param>
        /// <param name="tipo">Tipo de movimentação (1 para adicionar, 2 para subtrair)</param>
        public void AtualizarSaldo(int produtoId, int qtd, decimal valorMovimentacao, int tipo)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query;
            if (tipo == 1)
            {
                query = @"UPDATE produtos 
                  SET valor_unitario = ((quantidade_atual * valor_unitario) + (@qtd * @valor)) / (quantidade_atual + @qtd),
                      quantidade_atual = quantidade_atual + @qtd 
                  WHERE id = @id";
            }
            else
            {
                query = "UPDATE produtos SET quantidade_atual = quantidade_atual - @qtd WHERE id = @id";
            }

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@qtd", qtd);
            cmd.Parameters.AddWithValue("@valor", valorMovimentacao);
            cmd.Parameters.AddWithValue("@id", produtoId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Função responsável por Excluir um produto
        /// </summary>
        /// <param name="id">Id do produto a ser excluido</param>
        public void ExcluirProduto(int id)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = "DELETE FROM produtos WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
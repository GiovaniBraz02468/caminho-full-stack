using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using sistema_estoque.core.enums;
using sistema_estoque.core.models;
using sistema_estoque.infrastructure.database;

namespace sistema_estoque.infrastructure.repositories
{
    /// <summary>
    /// Classe responspavel por realizar a troca de dados entre o sistema e o banco de dados
    /// Utiliza o modelo Movimentacao
    /// </summary>
    public class MovimentacaoRepository
    {
        //Variáveis auxiliares
        private readonly DatabaseConnection _database = new DatabaseConnection();

        //Métodos

        /// <summary>
        /// Função responsável por salvar os dados no banco de dados
        /// </summary>
        /// <param name="produtoId">Id do produto para vincular a essa movimentação</param>
        /// <param name="qtd">Quantidade de itens a remover / adicionar</param>
        /// <param name="valor">Valor dos produtos sendo adicionados</param>
        /// <param name="tipo">Tipo de movimentação (1 para entrada, 2 para retirada)</param>
        public void Salvar(int produtoId, int qtd, decimal valor, int tipo)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = @"INSERT INTO movimentacoes (produto_id, quantidade, valor_unitario, tipo, criado_em) 
                        VALUES (@prodId, @qtd, @valor, @tipo, @data)";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@prodId", produtoId);
            cmd.Parameters.AddWithValue("@qtd", qtd);
            cmd.Parameters.AddWithValue("@valor", valor);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@data", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Função responsável por listar o histórico de movimentações realizadas
        /// </summary>
        /// <returns></returns>
        public List<dynamic> ListarHistorico()
        {
            var lista = new List<dynamic>();
            using var conn = _database.GetConnection();
            conn.Open();

            string query = @"SELECT m.id, p.nome as produto, m.quantidade, m.tipo, m.criado_em 
                    FROM movimentacoes m
                    INNER JOIN produtos p ON m.produto_id = p.id
                    ORDER BY m.criado_em DESC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new
                {
                    Id = reader.GetInt32("id"),
                    Produto = reader.GetString("produto"),
                    Qtd = reader.GetInt32("quantidade"),
                    Tipo = reader.GetInt32("tipo") == 1 ? "Entrada" : "Saída",
                    Data = reader.GetDateTime("criado_em")
                });
            }
            return lista;
        }

        /// <summary>
        /// função responsável por listar um histórico por deter minado produto
        /// </summary>
        /// <param name="produtoId">Id do produto para poder buscar os históricos</param>
        /// <returns>Retorna uma lista de movimentacoes</returns>
        public List<Movimentacao> ListarHistoricoPorProduto(int produtoId)
        {
            var lista = new List<Movimentacao>();
            using var conn = _database.GetConnection();
            conn.Open();
            string query = @"SELECT id, produto_id, quantidade, valor_unitario, tipo, criado_em 
                     FROM movimentacoes 
                     WHERE produto_id = @produtoId 
                     ORDER BY criado_em DESC";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@produtoId", produtoId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Movimentacao
                {
                    Id = reader.GetInt32("id"),
                    ProdutoId = reader.GetInt32("produto_id"),
                    Quantidade = reader.GetInt32("quantidade"),
                    ValorUnitario = reader.GetDecimal("valor_unitario"),
                    Tipo = (TipoMovimentacao)reader.GetInt32("tipo"),
                    DataMovmentacao = reader.GetDateTime("criado_em")
                });
            }
            return lista;
        }
    }
}
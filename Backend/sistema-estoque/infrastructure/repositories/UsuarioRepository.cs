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
    /// Classe responspavel por realizar o cadastro/manutenção de usuários no banco de dados
    /// </summary>
    public class UsuarioRepository
    {
        //Variáveis auxiliares
        private readonly DatabaseConnection _database;

        //Construtores
        public UsuarioRepository()
        {
            _database = new DatabaseConnection();
        }

        //Métodos

        /// <summary>
        /// Função responsável por realizar o cadastro de um novo usuário no banco de dados
        /// </summary>
        /// <param name="usuario">Instância do usuário, para poder pegar os valores e adicionar no banco</param>
        public void CriarUsuario(Usuario usuario)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = @"INSERT INTO usuarios (nome, email, senha_hash)
            VALUES (@nome, @email, @senha)";

            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@senha", usuario.SenhaHash);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Função responsável por realiar uma busca para o login e senha, em 2 campos na tabela de usuario
        /// </summary>
        /// <param name="email">Email informado pelo cliente</param>
        /// <param name="senha">Senha informado pelo cliente</param>
        /// <returns>Pode retornar 2 objetos, se encontrar um usuário com essas credenciais ele retorna o objeto com todas as informações do usuário preenchida
        /// Caso não encontra, retorna um valor null</returns>
        public Usuario? BuscarPorEmailESenha(string email, string senha)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = "SELECT * FROM usuarios WHERE email = @email AND senha_hash = @senha";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Usuario(
                    reader.GetString("nome"),
                    reader.GetString("email"),
                    reader.GetString("senha_hash")
                )
                { Id = reader.GetInt32("id") };
            }

            return null;
        }

        /// <summary>
        /// Função responsável por confirmar o login
        /// </summary>
        /// <param name="email">Email confirmado na busca</param>
        /// <param name="senha">Senha confirmada na busca</param>
        /// <returns>Retorna uma instância do usuário logado corretamente</returns>
        public Usuario? Login(string email, string senha)
        {
            using var conn = _database.GetConnection();
            conn.Open();

            string query = "SELECT id, nome, email, senha_hash FROM usuarios WHERE email = @email AND senha_hash = @senha";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Usuario(
                    reader.GetString("nome"),
                    reader.GetString("email"),
                    reader.GetString("senha_hash")
                )
                {
                    Id = reader.GetInt32("id")
                };
            }

            return null;
        }


    }
}
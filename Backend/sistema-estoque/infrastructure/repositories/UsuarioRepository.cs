using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using sistema_estoque.core.models;
using sistema_estoque.infrastructure.database;

namespace sistema_estoque.infrastructure.repositories
{
    public class UsuarioRepository
    {
        private readonly DatabaseConnection _database;

        public UsuarioRepository()
        {
            _database = new DatabaseConnection();
        }

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

        public Usuario? BuscarParaLogin(string email, string senha)
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
                { Id = reader.GetInt32("id") };
            }

            return null;
        }
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
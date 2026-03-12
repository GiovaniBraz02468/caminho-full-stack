using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_estoque.core.models
{
    public class Usuario
    {
        //Propriedades
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }
        public bool SenhaTemporaria { get; set; }
        public DateTime DataCriacao { get; set; }

        //Construtores
        [SetsRequiredMembers]
        public Usuario(string nome, string email, string senhaHash)
        {
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
            DataCriacao = DateTime.UtcNow;
        }
    }
}
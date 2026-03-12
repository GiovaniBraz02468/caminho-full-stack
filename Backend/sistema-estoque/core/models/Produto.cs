using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_estoque.core.models
{
    public class Produto
    {
        //Propriedades
        public int Id { get; set; }
        public required string Nome { get; set; }
        public int UsuarioId { get; set; }
        public required string Descricao { get; set; }
        public int QuantidadeAtual { get; set; }
        public decimal ValorUnitario { get; set; }
        public DateTime DataCriacao { get; set; }

        //Construtores
        public Produto(int usuarioId, string nome, string descricao, int quantidadeInicial, decimal valorUnitario)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Descricao = descricao;
            QuantidadeAtual = quantidadeInicial;
            DataCriacao = DateTime.UtcNow;
        }
    }
}
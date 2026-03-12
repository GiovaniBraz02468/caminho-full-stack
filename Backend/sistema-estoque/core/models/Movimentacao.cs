using sistema_estoque.core.enums;

namespace sistema_estoque.core.models
{
     /// <summary>
    /// Classe mmodelo para representar os campos da tabela movimentacoes
    /// </summary>
    public class Movimentacao
    {
        //Propriedades
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public required TipoMovimentacao Tipo { get; set; }
        public DateTime DataMovmentacao { get; set; }

        //Construtores
        public Movimentacao(int produtoId, int quantidade, decimal valorUnitario, TipoMovimentacao tipo)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            Tipo = tipo;
            DataMovmentacao = DateTime.UtcNow;
        }
    }
}
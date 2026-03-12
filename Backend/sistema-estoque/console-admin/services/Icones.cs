using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_estoque.console_admin.services
{
    /// <summary>
    /// Classe responsável por entregar ícones para o restante do sistema
    /// </summary>
    public class Icones
    {
        //Propriedades auxiliares
        // Navegação e Status
        public const string Sucesso = "✅";
        public const string Erro = "❌";
        public const string Alerta = "⚠️";
        public const string Info = "ℹ️";
        public const string Sair = "🚪";
        public const string Voltar = "⬅️";

        // Entidades
        public const string Usuario = "👤";
        public const string Produto = "📦";
        public const string Estoque = "📊";
        public const string Movimentacao = "🔄";
        public const string Login = "🔑";
        public const string Cadastro = "📝";

        // Sistema
        public const string Admin = "⚙️";
        public const string API = "🌐";
        public const string Home = "🏠";

        public const string Lista = "📜";

    }
}
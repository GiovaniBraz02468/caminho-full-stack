using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_estoque.console_admin.services;
using sistema_estoque.core.models;
using sistema_estoque.infrastructure.repositories;

namespace sistema_estoque.console_admin.menus
{
    public class MenuLogin
    {
        private UsuarioRepository _repo = new UsuarioRepository();

        public void Exibir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ACESSO ADMINISTRATIVO ===");
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Se cadastrar (Criar conta)");
                Console.WriteLine("0 - Voltar");
                string opcao = ConsoleUtils.LerOpcaoMenu(new string[] { "1", "2", "0" });

                if (opcao == "0") break;
                if (opcao == "2") RealizarCadastro();
                if (opcao == "1") RealizarLogin();

            }
        }
        private void RealizarLogin()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Login} ACESSO AO SISTEMA ===");
            string email = ConsoleUtils.LerInputObrigatorio("Email");
            string senha = ConsoleUtils.LerInputObrigatorio("Senha");
            Console.WriteLine("Autenticando...");

            var usuarioLogado = _repo.Login(email, senha);

            if (usuarioLogado != null)
            {
                Console.WriteLine($"{Icones.Sucesso} Login realizado! Bem-vindo(a), {usuarioLogado.Nome}.");
                Thread.Sleep(1500);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(intercept: true);
                }
                new MenuSistema(usuarioLogado).Exibir();
            }
            else
            {
                Console.WriteLine($"{Icones.Erro} Email ou senha incorretos!");
                Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                Console.ReadKey();
            }
        }

        private void RealizarCadastro()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Usuario} NOVO CADASTRO ===");

            string nome = ConsoleUtils.LerInputObrigatorio("Nome");
            string email = ConsoleUtils.LerInputObrigatorio("Email");
            string senha = ConsoleUtils.LerInputObrigatorio("Senha");
            var novo = new Usuario(nome, email, senha);

            try
            {

                _repo.CriarUsuario(novo);
                Console.WriteLine($"{Icones.Sucesso} Sucesso! Entrando no sistema...");
                Thread.Sleep(1500);

                new MenuSistema(novo).Exibir();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Icones.Erro} Erro: {ex.Message}");
                Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
                Console.ReadKey();
            }
        }

    }
}
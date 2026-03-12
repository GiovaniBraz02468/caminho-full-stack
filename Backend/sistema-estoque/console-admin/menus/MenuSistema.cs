using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_estoque.console_admin.services;
using sistema_estoque.core.models;
using sistema_estoque.infrastructure.database;
using sistema_estoque.infrastructure.repositories;

namespace sistema_estoque.console_admin.menus
{
    /// <summary>
    /// Classe responsável por gerenciar o menu do cliente após o login (Gerenciar produtos e estoque)
    /// </summary>
    public class MenuSistema
    {
        //Variáveis auxiliares
        private Usuario _usuarioLogado;
        private readonly MovimentacaoRepository _movimentacaoRepo;
        public ProdutoRepository _produtoRepo = new ProdutoRepository();

        //Construtores
        public MenuSistema(Usuario usuario)
        {
            _usuarioLogado = usuario;
            _produtoRepo = new ProdutoRepository();
            _movimentacaoRepo = new MovimentacaoRepository();
        }

        //Métodos

        /// <summary>
        /// Função responsável por exibir o menu principal do cliente logado, para gerenciar os produtos e movimentações
        /// </summary>
        public void Exibir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== {Icones.Estoque} SISTEMA | Olá, {Icones.Usuario} {_usuarioLogado.Nome} ===");
                Console.WriteLine($"1 - {Icones.Cadastro} Cadastrar Produto");
                Console.WriteLine($"2 - {Icones.Estoque} Ver Produto / Estoque");
                Console.WriteLine($"3 - {Icones.Produto} Editar Produto");
                Console.WriteLine($"4 - {Icones.Movimentacao} Lançar Movimentação (Entrada/Saída)");
                Console.WriteLine($"5 - {Icones.Lista} Extrato de Movimentações");
                Console.WriteLine($"0 - {Icones.Sair} Logout");

                string opcao = ConsoleUtils.LerOpcaoMenu(["1", "2", "3", "4", "5", "0"]);

                if (opcao == "0")
                {
                    Console.WriteLine($"{Icones.Sair} Fazendo logout... Até logo!");
                    Thread.Sleep(1000);
                    break;
                }

                else if (opcao == "1") CadastrarNovoProduto();
                else if (opcao == "2") VerEstoque();
                else if (opcao == "3") EditarProduto();
                else if (opcao == "4") RealizarMovimentacao();
                else if (opcao == "5") VerHistorico();
                else Console.WriteLine("[Funcionalidade em desenvolvimento...]");
            }
        }

        /// <summary>
        /// Função responsável por realizar o cadastro de um novo produto (chamando as funções do repositório)
        /// </summary>
        private void CadastrarNovoProduto()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Cadastro} CADASTRAR NOVO PRODUTO ===");
            string nome = ConsoleUtils.LerInputObrigatorio("Nome do Produto");
            string descricao = ConsoleUtils.LerInputObrigatorio("Descrição");

            int qtd = ConsoleUtils.LerIntOpcional("Quantidade Inicial (ou Enter para 0)");
            decimal valor = ConsoleUtils.LerDecimalOpcional("Valor Unitário (ou Enter para 0)");

            var novoProduto = new Produto(_usuarioLogado.Id, nome, descricao, qtd, valor);

            try
            {
                _produtoRepo.CriarProduto(novoProduto);
                Console.WriteLine($"{Icones.Sucesso} Produto '{nome}' cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Icones.Erro} Erro ao salvar produto: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        /// <summary>
        /// Função responsável por listar os itens do estoque
        /// </summary>
        private void VerEstoque()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Estoque} SEU ESTOQUE ATUAL ===");

            var produtos = _produtoRepo.ListarProdutos(_usuarioLogado.Id);

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado ainda.");
            }
            else
            {
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-10} | {3,-12}", "ID", "NOME", "QTD", "PREÇO"));
                Console.WriteLine("----------------------------------------------------------------------");

                foreach (var p in produtos)
                {
                    Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-10} | {3,-12:C}",
                        p.Id,
                        p.Nome.Length > 20 ? p.Nome.Substring(0, 17) + "..." : p.Nome,
                        p.QuantidadeAtual,
                        p.ValorUnitario));
                }
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        /// <summary>
        /// Função responsável por realizar a edição de um produto (Chamando as funções do repositório)
        /// </summary>
        private void EditarProduto()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Movimentacao} EDITAR PRODUTO ===");

            var produtos = _produtoRepo.ListarProdutos(_usuarioLogado.Id);

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto para editar.");
                Console.ReadKey();
                return;
            }

            foreach (var p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Preço Atual: {p.ValorUnitario:C}");
            }

            Console.Write("Digite o ID do produto para editar (ou 0 para cancelar): ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id == 0) return;

            var produto = produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
            {
                Console.WriteLine("ID inválido!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Editando: {produto.Nome}");
            Console.WriteLine("(Deixe em branco para manter o valor atual)");

            Console.Write($"Novo Nome [{produto.Nome}]: ");
            string novoNome = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(novoNome)) produto.Nome = novoNome;

            Console.Write($"Nova Descrição [{produto.Descricao}]: ");
            string novaDesc = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(novaDesc)) produto.Descricao = novaDesc;

            produto.ValorUnitario = ConsoleUtils.LerDecimalOpcional("Novo Valor");

            try
            {
                _produtoRepo.AtualizarProduto(produto);
                Console.WriteLine($"{Icones.Sucesso} Alterações salvas com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Icones.Erro} Erro ao atualizar: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        /// <summary>
        /// Função responsável por realizar uma nova movimentação de um produto (Chamando as funções do repositório)
        /// </summary>
        private void RealizarMovimentacao()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Produto} NOVA MOVIMENTAÇÃO ===");
            var produtos = _produtoRepo.ListarProdutos(_usuarioLogado.Id);

            foreach (var p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | {p.Nome} | Saldo Atual: {p.QuantidadeAtual}");
            }

            Produto? produto = null;
            int id = 0;
            while (true)
            {
                Console.Write("Digite o ID do produto (ou 0 para cancelar): ");
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("⚠️ Digite um número de ID válido!");
                    continue;
                }
                if (id == 0) return;

                produto = produtos.FirstOrDefault(p => p.Id == id);
                if (produto != null) break;

                Console.WriteLine($"{Icones.Erro} Produto não encontrado! Tente novamente.");
            }

            int tipo = 0;
            while (true)
            {
                Console.Write("Tipo [1 - Entrada (+) | 2 - Saída (-)]: ");
                if (int.TryParse(Console.ReadLine(), out tipo) && (tipo == 1 || tipo == 2))
                    break;

                Console.WriteLine($"{Icones.Erro} Opção inválida! Escolha 1 para Entrada ou 2 para Saída.");
            }

            int qtd = 0;
            while (true)
            {
                qtd = ConsoleUtils.LerIntOpcional("Quantidade", 1);

                if (tipo == 2 && produto.QuantidadeAtual < qtd)
                {
                    Console.WriteLine($"{Icones.Erro} Saldo insuficiente! Você tem apenas {produto.QuantidadeAtual}.");
                    continue;
                }

                break;
            }

            decimal valor = 0;
            while (true)
            {
                valor = ConsoleUtils.LerDecimalOpcional("Valor", 1);
                break;
            }

            try
            {
                _movimentacaoRepo.Salvar(id, qtd, valor, tipo);
                string operacao = (tipo == 1) ? "+" : "-";
                _produtoRepo.AtualizarSaldo(id, qtd, operacao);

                Console.WriteLine($"\n{Icones.Sucesso} Movimentação de {(tipo == 1 ? "Entrada" : "Saída")} realizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Icones.Erro} Erro ao processar: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        /// <summary>
        /// Função responsável por exibir as movimentações realizadas de um produto (Chamando as funções do repositório)
        /// </summary>
        private void VerHistorico()
        {
            Console.Clear();
            Console.WriteLine("=== 📜 HISTÓRICO DE MOVIMENTAÇÕES ===");
            Console.WriteLine("ID | Produto          | Tipo    | Qtd | Data");
            Console.WriteLine("--------------------------------------------------");

            var historico = _movimentacaoRepo.ListarHistorico();

            foreach (var h in historico)
            {
                Console.WriteLine($"{h.Id,-2} | {h.Produto,-16} | {h.Tipo,-7} | {h.Qtd,-3} | {h.Data:dd/MM/yyyy HH:mm}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }
    }
}
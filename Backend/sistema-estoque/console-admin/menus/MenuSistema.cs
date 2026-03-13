using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistema_estoque.console_admin.services;
using sistema_estoque.core.enums;
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
        /// Atualização -> Adicionado uma possível movimentação 
        /// </summary>
        private void CadastrarNovoProduto()
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Cadastro} CADASTRAR NOVO PRODUTO ===");
            string nome = ConsoleUtils.LerInputObrigatorio("Nome do Produto");
            string descricao = ConsoleUtils.LerInputObrigatorio("Descrição");

            var novoProduto = new Produto(_usuarioLogado.Id, nome, descricao, 0, 0);

            try
            {
                int idGerado = _produtoRepo.CriarProduto(novoProduto);
                Console.WriteLine($"{Icones.Sucesso} Produto '{nome}' cadastrado com sucesso (ID: {idGerado})!");

                Console.WriteLine($"{Icones.Estoque} Deseja realizar a primeira entrada de estoque?");
                string resposta = ConsoleUtils.LerOpcaoMenu(["S", "N"]); if (Console.ReadLine()?.ToUpper() == "S")
                {

                    RealizarMovimentacao(idGerado);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Icones.Erro} Erro ao salvar produto: {ex.Message}");
            }
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
        /// Atualização - > realiza a exclusão tbm
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

            Console.WriteLine($"O que deseja fazer com o produto: {produto.Nome}?");
            Console.WriteLine("1 - Editar informações");
            Console.WriteLine("2 - EXCLUIR PRODUTO (e todo o seu histórico)");
            Console.WriteLine("0 - Voltar");

            string subOpcao = ConsoleUtils.LerOpcaoMenu(["1", "2", "0"]);

            if (subOpcao == "1")
            {
                Console.WriteLine($"Editando: {produto.Nome}");
                Console.WriteLine("(Deixe em branco para manter o valor atual)");

                Console.Write($"Novo Nome [{produto.Nome}]: ");
                string novoNome = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(novoNome)) produto.Nome = novoNome;

                Console.Write($"Nova Descrição [{produto.Descricao}]: ");
                string novaDesc = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(novaDesc)) produto.Descricao = novaDesc;
                try
                {
                    _produtoRepo.AtualizarProduto(produto);
                    Console.WriteLine($"{Icones.Sucesso} Alterações salvas com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{Icones.Erro} Erro ao atualizar: {ex.Message}");
                }
            }
            else if (subOpcao == "2")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Icones.Alerta} ATENÇÃO: Isso apagará permanentemente o produto e TODAS as suas movimentações!");
                Console.Write("Para confirmar, digite exatamente 'DELETAR': ");
                Console.ResetColor();

                if (Console.ReadLine()?.ToUpper() == "DELETAR")
                {
                    _produtoRepo.ExcluirProduto(produto.Id);
                    Console.WriteLine($"{Icones.Sucesso} Produto removido com sucesso!");
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("Ação cancelada. Nada foi excluído.");
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        /// <summary>
        /// Função responsável por realizar uma nova movimentação de um produto (Chamando as funções do repositório)
        /// </summary>
        private void RealizarMovimentacao(int? idPreSelecionado = null)
        {
            Console.Clear();
            Console.WriteLine($"=== {Icones.Produto} NOVA MOVIMENTAÇÃO ===");

            var produtos = _produtoRepo.ListarProdutos(_usuarioLogado.Id);

            if (produtos.Count == 0)
            {
                Console.Clear();
                Console.WriteLine($"{Icones.Erro} Você não tem produtos cadastrados.");
                Console.WriteLine("Cadastre um produto primeiro para poder lançar movimentações.");
                Console.WriteLine("Pressione qualquer tecla para voltar...");
                Console.ReadKey();
                return;
            }

            if (idPreSelecionado == null)
            {
                foreach (var p in produtos)
                {
                    Console.WriteLine($"ID: {p.Id} | {p.Nome} | Saldo Atual: {p.QuantidadeAtual}");
                }
            }

            Produto? produto = null;
            int id = idPreSelecionado ?? 0;

            while (true)
            {
                if (id > 0)
                {
                    produto = produtos.FirstOrDefault(p => p.Id == id);
                    if (produto != null) break;
                }

                Console.Write("Digite o ID do produto (ou 0 para cancelar): ");
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine($"{Icones.Alerta} Digite um número de ID válido!");
                    id = 0;
                    continue;
                }

                if (id == 0) return;
            }

            Console.WriteLine($"{Icones.Produto} Movimentando: {produto.Nome}");

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
                valor = ConsoleUtils.LerDecimalOpcional(tipo == 1 ? "Valor de Compra Unitário" : "Valor de Saída Unitário", 0.01m);
                break;
            }

            try
            {
                _movimentacaoRepo.Salvar(id, qtd, valor, tipo);

                _produtoRepo.AtualizarSaldo(id, qtd, valor, tipo);

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
            Console.WriteLine($"=== {Icones.Lista} EXTRATO POR PRODUTO ===");

            var produtos = _produtoRepo.ListarProdutos(_usuarioLogado.Id);

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado para ver histórico.");
                Console.ReadKey();
                return;
            }
            foreach (var p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | {p.Nome}");
            }

            Console.Write("\nDigite o ID do produto para ver o extrato (ou 0 para cancelar): ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id == 0) return;
            var produtoSel = produtos.FirstOrDefault(p => p.Id == id);

            if (produtoSel == null)
            {
                Console.WriteLine("Produto não encontrado!");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"=== 📜 EXTRATO: {produtoSel.Nome.ToUpper()} ===");
            Console.WriteLine($"Saldo Atual: {produtoSel.QuantidadeAtual} | Valor Médio: {produtoSel.ValorUnitario:C}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Data       | Tipo    | Qtd | Valor Unit. | Total");
            Console.WriteLine("--------------------------------------------------");

            var historico = _movimentacaoRepo.ListarHistoricoPorProduto(id);
            foreach (var h in historico)
            {
                string sinal = h.Tipo == TipoMovimentacao.Entrada ? "+" : "-";
                string descTipo = h.Tipo == TipoMovimentacao.Entrada ? "Entrada" : "Saída";
                decimal totalLinha = h.Quantidade * h.ValorUnitario;

                Console.WriteLine($"{h.DataMovmentacao:dd/MM/yyyy} | {descTipo,-7} | {sinal}{h.Quantidade,-3} | {h.ValorUnitario,10:C} | {totalLinha,10:C}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }
    }
}
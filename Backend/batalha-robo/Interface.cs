using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace batalha_robo
{
    public class Interface
    {
        //Métodos
        public static void InterfaceCadastrar()
        {
            Console.WriteLine("Vamos cadastrar um robô!");
            while (true)
            {
                Console.WriteLine("Digite o nome do participante (Não pode ter nomes repetidos)");
                string nomeRobo = Console.ReadLine() ?? "";
                if (!BatalhaRobo.Torneio.VerificaRobo(nomeRobo))
                {
                    BatalhaRobo.Torneio.CadastraParticipate(new Robo(nomeRobo));
                    Console.Clear();
                    Console.WriteLine($"Novo participante {nomeRobo.ToUpper()} cadastrada com sucesso!");
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Um robô com o nome {nomeRobo.ToUpper()} já existem, cadastre um novo!");
                }
            }
        }
        public static void InterfaceListar()
        {
            Console.Clear();

            if (BatalhaRobo.Torneio.Participantes.Count() > 0)
            {

                Console.WriteLine("Vamos listar os participantes (Robôs)");

                int contador = 1;
                foreach (Robo robo in BatalhaRobo.Torneio.Participantes)
                {
                    Console.WriteLine($"Robo {contador}:");
                    Console.WriteLine($"Nome: {robo.Nome}");
                    if (robo.JaLutou)
                    {
                        if (robo.Invicto)
                        {
                            Console.WriteLine("O robô é invicto!");
                        }
                        else
                        {
                            Console.WriteLine($"Vitórias: {robo.Vitorias}");
                            Console.WriteLine($"Derrotas: {robo.Derrotas}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ainda não lutou");
                    }
                    Console.WriteLine("=======================");
                    contador += 1;
                }
            }
            else
            {
                Console.WriteLine("Não temos robôs cadastrados!");
                return;
            }

        }
        public static void InterfaceEditaDeleta()
        {
            Console.Clear();
            if (BatalhaRobo.Torneio.QtdParticipantes() > 0)
            {
                Console.WriteLine("Vamos Editar um robo");
                while (true)
                {
                    int contador = 1;
                    foreach (Robo robo in BatalhaRobo.Torneio.Participantes)
                    {
                        Console.WriteLine($"({contador} -> {robo.Nome})");
                        contador += 1;
                    }

                    Console.WriteLine("Digite o número do robo para editar ele, ou 0 para sair!");
                    string indice = Console.ReadLine() ?? "";

                    if (indice == "0")
                    {
                        Console.Clear();
                        Console.WriteLine("Retornando para o menu principal");
                        break;
                    }
                    else
                    {
                        if (int.TryParse(indice, out int nEscolhido))
                        {
                            int indiceCerto = nEscolhido - 1;
                            if (indiceCerto >= 0 && indiceCerto < BatalhaRobo.Torneio.Participantes.Count())
                            {
                                Robo roboSelecionado = BatalhaRobo.Torneio.Participantes[indiceCerto];
                                Console.WriteLine($"Qual a opção sobre o {roboSelecionado.Nome}?");
                                while (true)
                                {
                                    Console.WriteLine("1 - Editar");
                                    Console.WriteLine("2 - Deletar");
                                    Console.WriteLine("3 - Cancelar");
                                    string op = Console.ReadLine() ?? "";

                                    switch (op)
                                    {
                                        case "1":
                                            Console.WriteLine($"Digite o novo nome");
                                            string entrada = Console.ReadLine() ?? "";
                                            if (!string.IsNullOrWhiteSpace(entrada))
                                            {
                                                roboSelecionado.Nome = entrada;
                                                Console.Clear();
                                                Console.WriteLine("Nome alterado!");
                                                Console.WriteLine("Voltando para o menu principal");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nome mantido.");
                                            }
                                            return;
                                        case "2":
                                            BatalhaRobo.Torneio.RemoverPArticipante(roboSelecionado);
                                            Console.Clear();
                                            Console.WriteLine("Robô deletado");
                                            return;
                                        case "3":
                                            Console.Clear();
                                            Console.WriteLine("Cancelando edição");
                                            return;
                                        default:
                                            Console.WriteLine("Opção inválida");
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Esse robô não existe na lista!");
                            }

                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Digite uma opção correta!");
                        }
                    }

                }

            }
            else
            {
                Console.WriteLine("Não temos robôs cadastrados!");
            }

        }
        public static void InterfaceBatalha()
        {
            if (BatalhaRobo.Torneio.QtdParticipantes() > 1)
            {
                Console.WriteLine("Finalmente vamos para as batalhas!");
                Robo? robo1 = null;
                Robo? robo2 = null;

                while (true)
                {
                    int contador = 1;
                    Console.WriteLine("Selecione o PRIMEIRO robô para lutar!");
                    foreach (Robo robo in BatalhaRobo.Torneio.Participantes)
                    {
                        Console.WriteLine($"({contador} -> {robo.Nome})");
                        contador += 1;
                    }
                    Console.WriteLine("Digite o número 0 para sair");
                    string indice = Console.ReadLine() ?? "";

                    if (indice == "0")
                    {
                        Console.Clear();
                        Console.WriteLine("Retornando para o menu principal");
                    }

                    if (int.TryParse(indice, out int nEscolhido))
                    {
                        int indiceCerto = nEscolhido - 1;
                        if (indiceCerto >= 0 && indiceCerto < BatalhaRobo.Torneio.QtdParticipantes())
                        {
                            robo1 = BatalhaRobo.Torneio.Participantes[indiceCerto];
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Robô não existe!");
                        }
                    }
                }
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"O primeiro lutador é: {robo1.Nome}");
                    Console.WriteLine("Agora escolha o segundo robô");

                    var listaFiltrada = BatalhaRobo.Torneio.Participantes.Where(r => r != robo1).ToList();

                    int contador = 1;
                    foreach (Robo robo in listaFiltrada)
                    {
                        Console.WriteLine($"({contador} -> {robo.Nome})");
                        contador += 1;
                    }

                    Console.WriteLine("Digite o número ou 0 para cancelar");
                    string indice2 = Console.ReadLine() ?? "";

                    if (indice2 == "0") return;

                    if (int.TryParse(indice2, out int nEscolhido2))
                    {
                        int indiceCerto2 = nEscolhido2 - 1;
                        if (indiceCerto2 >= 0 && indiceCerto2 < listaFiltrada.Count())
                        {
                            robo2 = listaFiltrada[indiceCerto2];
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Robô não existe!");
                        }
                    }
                }

                Console.Clear();
                Console.WriteLine($"confronto decidido: {robo1.Nome} VS {robo2.Nome}");
                Console.WriteLine("Pressione qualquer tecla para começar a luta!");
                Console.ReadKey();

                InterfaceExecutarBatalha(robo1, robo2);

                return;

            }
            else if (BatalhaRobo.Torneio.QtdParticipantes() <= 1)
            {
                Console.WriteLine("Precisamos ter pelo menos 2 robôs para lutar, cadastre um novo!");
            }
            else if (BatalhaRobo.Torneio.QtdParticipantes() == 0)
            {
                Console.WriteLine("Não temos robôs cadastrados!");
            }
        }
        public static void InterfaceExecutarBatalha(Robo r1, Robo r2)
        {
            Console.Clear();
            Console.WriteLine($"INÍCIO DA BATALHA: {r1.Nome} VS {r2.Nome}");

            bool turnoRobo1 = Utils.Sorteio.Next(0, 2) == 0;
            Console.WriteLine($"O sorteio decidiu: {(turnoRobo1 ? r1.Nome : r2.Nome)} ataca primeiro!");
            Console.WriteLine("Pressione qualquer tecla para começar");
            Console.ReadKey();

            while (r1.Vida > 0 && r2.Vida > 0)
            {
                Console.WriteLine("\n------------------------------");
                if (turnoRobo1)
                {
                    RealizarTurno(r1, r2);
                }
                else
                {
                    RealizarTurno(r2, r1);
                }
                turnoRobo1 = !turnoRobo1;

                System.Threading.Thread.Sleep(1000);

            }

            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }

            if (r1.Vida > 0)
            {
                Console.WriteLine($"VITÓRIA DE {r1.Nome}!");
                r1.Vencer();
                r2.Derrota();
            }
            else
            {
                Console.WriteLine($"VITÓRIA DE {r2.Nome}!");
                r2.Vencer();
                r1.Derrota();
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
        }
        private static void RealizarTurno(Robo atacante, Robo defensor)
        {
            int forcaAtaque = atacante.Atacar();
            int dano = defensor.Defender(forcaAtaque);

            if (dano < 0)
            {
                Console.WriteLine($"ATAQUE: {atacante.Nome} deu um golpe de {forcaAtaque}!");
                Console.WriteLine($"DANO A VIDA: {defensor.Nome} recebeu {-dano} de dano! (Vida Restante: {defensor.Vida})");
            }
            else
            {
                Console.WriteLine($"DEFESA: {atacante.Nome} tentou atacar ({forcaAtaque}), mas {defensor.Nome} DEFENDEU TUDO!");
            }
        }
    }


}
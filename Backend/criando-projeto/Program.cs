/*
Entendendo como funciona instalar um novo projeto
Comandos:
dotnet new - mostra os projetos baixados
dotnet new nome-projeto - cria um projeto com esse tipo

No caso, necessário instalar o SDK do .NET, para poder
rodar o dotnet via terminal

Exercício 1: Entendendo comandos via console (Escrever e ler)
Console.WriteLine("Testando a instalação do C# para o VsCode");
Console.WriteLine("Digite seu nome");
var name = Console.ReadLine();
Console.WriteLine("Digite seu país");
var pais = Console.ReadLine();
Console.WriteLine("=== Processando perfil ===");
Console.WriteLine($"Olá, me chamo {name} e moro em {pais}");
*/

using System.Diagnostics;

var livros = new List<string>();

while (true)
{
    Console.WriteLine("=== MENU PRINCIPAL ===");
    Console.WriteLine("1 - Cadastrar livro");
    Console.WriteLine("2 - listar livros");
    Console.WriteLine("3 - remover livro");
    Console.WriteLine("0 - sair");

    var op = Console.ReadLine();

    switch (op)
    {
        case "1":
            Console.WriteLine("Digite o título do livro");
            var nomelivro = Console.ReadLine();
            livros.Add(nomelivro);
            Console.WriteLine($"Livro {nomelivro} cadastrado com sucesso");
            break;
        case "2":
            foreach (var livro in livros)
            {
                Console.WriteLine(livro);
            }
            break;
        case "3":
            Console.WriteLine("Digite o nome do livro para remover");
            var livroRemover = Console.ReadLine();
            var sucesso = livros.Remove(livroRemover);
            if (sucesso)
            {
                Console.WriteLine($"Livro {livroRemover} removido com sucesso");
            }
            else
            {
                Console.WriteLine($"Livro {livroRemover} não encontrado");
            }
            break;
        case "0":
            Console.WriteLine("Encerrando aplicação...");
            break;
        default:
            Console.WriteLine("Opção inválida");
            break;
    }
}
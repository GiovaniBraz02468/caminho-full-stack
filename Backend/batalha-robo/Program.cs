namespace batalha_robo;

public class BatalhaRobo
{
    //Propriedades
    public static RoundLuta Torneio { get; set; } = new RoundLuta();

    //Métodos
    public static void Main()
    {
        MenuPrincipal.Apresentacao();
        while (true)
        {
            string op = MenuPrincipal.ProcessaMenu();
            if (op == "5")
            {
                MenuPrincipal.Despedida();
                break;
            }
            MenuPrincipal.ProcessaOpcao(op);
        }

    }
}

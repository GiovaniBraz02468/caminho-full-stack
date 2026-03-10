namespace batalha_robo;

public class Robo
{
    //variáveis ajudantes
    private string _nome = "";

    //Propriedades
    public string Nome
    {
        get => _nome;
        set => _nome = value.ToUpper();
    }
    public int Vida { get; set; }
    public int Vitorias { get; set; }
    public int Derrotas { get; set; }
    public bool JaLutou
    {
        get
        {
            return Vitorias > 0 || Derrotas > 0;
        }
    }
    public bool Invicto
    {
        get
        {
            return Vitorias > 0 && Derrotas == 0;
        }
    }

    //Construtores
    public Robo(string name)
    {
        Nome = name;
        Vida = 100;
        Vitorias = 0;
        Derrotas = 0;
    }

    //Métodos
    public int Atacar()
    {
        return Utils.Sorteio.Next(1, 21);
    }
    public int Defender(int ataqueRealizado)
    {
        int defesa = Utils.Sorteio.Next(1, 6);
        int ataqueInfringido = defesa - ataqueRealizado;

        if (ataqueInfringido < 0)
        {
            Vida += ataqueInfringido;
            if (Vida < 0) Vida = 0;
            return ataqueInfringido;
        }

        return 0;
    }
    public void Vencer()
    {
        Vida = 100;
        Vitorias += 1;
    }
    public void Derrota()
    {
        Vida = 100;
        Derrotas += 1;
    }
}
namespace batalha_robo;

public class RoundLuta
{
    //Propriedades
    public List<Robo> Participantes { get; set; } = new();
    public Robo? PrimeiroRobo { get; set; }
    public Robo? SegundoRobo { get; set; }

    //Construtores
    public RoundLuta()
    {
        Participantes = new List<Robo>();
    }

    //Métodos
    public void CadastraParticipate(Robo novoIntegrante)
    {
        Participantes.Add(novoIntegrante);
    }
    public void RemoverPArticipante(Robo antigoIntegrante)
    {
        Participantes.Remove(antigoIntegrante);
    }
    public bool VerificaRobo(string novoNome)
    {
        return Participantes.Any(nr => nr.Nome == novoNome.ToUpper());
    }
    public int QtdParticipantes()
    {
        return Participantes.Count();
    }
}

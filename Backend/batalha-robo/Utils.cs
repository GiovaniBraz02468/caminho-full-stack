namespace batalha_robo
{
    public class Utils
    {
        //Propriedades 
        public static readonly Random Sorteio = new();
        
        //Métodos
        public static bool ValidaOp(string op)
        {
            string[] opcoesValidas = { "1", "2", "3", "4", "5" };
            return opcoesValidas.Contains(op);
        }
    }
}
using System.Threading.Tasks;

namespace treinando_api;

public class Program
{
    //Métodos
    public static async Task Main(string[] args)
    {
        var menu = new treinando_api.Interface.Menuconsole();
        await menu.Iniciar();
    }
}
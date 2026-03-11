using treinando_api.Services;

namespace treinando_api.Interface
{
    public class ExecutaInterfaces
    {
        //Variáveis auxiliares
        private BrasilApiService _api;
        private AcoesService _acoesApi;
        private IbgeService _ibgeApi;
        private CnpjService _cnpjApi;

        //Construtores
        public ExecutaInterfaces()
        {
            _api = new BrasilApiService();
            _acoesApi = new AcoesService();
            _ibgeApi = new IbgeService();
            _cnpjApi = new CnpjService();
        }

        //Métodos
        public async Task VerificaOp(string op)
        {
            switch (op)
            {
                case "1":
                    await InterfaceApiCep();
                    break;
                case "2":
                    await InterfaceApiAcoes();
                    break;
                case "3":
                    await InterfaceApiIbge();
                    break;
                case "4":
                    await InterfaceApiCnpj();
                    break;
            }
        }
        public async Task InterfaceApiCep()
        {
            while (true)
            {
                Console.WriteLine("Digite o CEP (apenas números) ou 0 para cancelar:");
                var cep = Console.ReadLine();
                if (cep == "0") { CancelaOp(); return; }
                if (string.IsNullOrWhiteSpace(cep) || cep.Length < 8)
                {
                    Console.Clear();
                    Console.WriteLine("Atenção: O CEP deve conter 8 dígitos.");
                    continue;
                }

                try
                {
                    var resultado = await _api.BuscarCep(cep);
                    if (resultado == null || string.IsNullOrEmpty(resultado.city))
                    {
                        Console.Clear();
                        Console.WriteLine("CEP não encontrado na base de dados.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=== RESULTADO ENCONTRADO ===");
                        Console.WriteLine($"Cidade: {resultado.city} - {resultado.state}");
                        Console.WriteLine($"Bairro: {resultado.neighborhood}");
                        Console.WriteLine($"Rua:    {resultado.street}");
                        Console.WriteLine("============================");
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Erro: {Utils.Utils.TratarErroApi(ex)}");
                }

                Console.WriteLine("1 para nova consulta, qualquer tecla para sair:");
                if (Console.ReadLine() != "1") { CancelaOp(); break; }
                Console.Clear();
            }
        }
        public async Task InterfaceApiAcoes()
        {
            while (true)
            {
                Console.WriteLine("Digite o código da ação (ex: PETR4, VALE3) ou 0 para voltar:");
                var ticker = Console.ReadLine()?.ToUpper() ?? "";

                if (ticker == "0") { CancelaOp(); return; }

                try
                {
                    var acao = await _acoesApi.BuscarCotacao(ticker);

                    if (acao == null)
                    {
                        Console.WriteLine("Ação não encontrada.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=== COTAÇÃO ATUAL ===");
                        Console.WriteLine($"Empresa: {acao.longName}");
                        Console.WriteLine($"Símbolo: {acao.symbol}");
                        Console.WriteLine($"Preço: {acao.regularMarketPrice:C2} ({acao.currency})");
                        Console.WriteLine("=====================");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Utils.Utils.TratarErroApi(ex));
                }

                Console.WriteLine("1 para nova consulta, qualquer tecla para sair:");
                if (Console.ReadLine() != "1") { CancelaOp(); break; }
                Console.Clear();
            }
        }
        public async Task InterfaceApiIbge()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Buscando estados do Brasil no IBGE...");
                Console.WriteLine("Aguarde um momento ou digite 0 para cancelar...");

                try
                {
                    var estados = await _ibgeApi.BuscarEstados();

                    if (estados == null || !estados.Any())
                    {
                        Console.WriteLine("Não foi possível carregar a lista de estados.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=== LISTA DE ESTADOS (IBGE) ===");
                        foreach (var est in estados.OrderBy(e => e.nome))
                        {
                            Console.WriteLine($"{est.sigla?.PadRight(3) ?? "   "} - {est.nome}");
                        }
                        Console.WriteLine("===============================");
                    }
                }
                catch (Exception ex)
                {
                    string mensagemMassa = Utils.Utils.TratarErroApi(ex);
                    Console.WriteLine($"Atenção: {mensagemMassa}");
                }

                Console.WriteLine("1 para nova listagem, qualquer tecla para sair:");
                if (Console.ReadLine() != "1") { CancelaOp(); break; }
                Console.Clear();
            }
        }
        public async Task InterfaceApiCnpj()
        {
            while (true)
            {
                Console.WriteLine("Digite o CNPJ (apenas números) ou 0 para voltar:");
                var cnpj = Console.ReadLine() ?? "";

                if (cnpj == "0") { CancelaOp(); return; }

                try
                {
                    var empresa = await _cnpjApi.BuscarCnpj(cnpj);

                    if (empresa == null || empresa.status == "ERROR")
                    {
                        Console.Clear();
                        Console.WriteLine($"Erro: {empresa?.message ?? "CNPJ não encontrado."}");
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine("=== DADOS DA EMPRESA ===");
                    Console.WriteLine($"Razão Social: {empresa.nome}");
                    Console.WriteLine($"Nome Fantasia: {empresa.fantasia}");
                    Console.WriteLine($"CNPJ: {empresa.cnpj}");
                    Console.WriteLine($"Local: {empresa.municipio} - {empresa.uf}");
                    Console.WriteLine($"Endereço: {empresa.logradouro}, {empresa.numero}");
                    Console.WriteLine("=========================");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Utils.Utils.TratarErroApi(ex));
                }

                Console.WriteLine("1 para nova consulta, qualquer tecla para sair:");
                if (Console.ReadLine() != "1") { CancelaOp(); break; }
                Console.Clear();
            }
        }
        public void CancelaOp()
        {
            Console.Clear();
            Console.WriteLine("Retornando para o menu principal...");
        }

    }
}
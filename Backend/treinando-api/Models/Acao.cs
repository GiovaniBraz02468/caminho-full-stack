using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treinando_api.Models
{
    public class Acao
    {
        //Propriedades
        public string? symbol { get; set; }
        public string? longName { get; set; }
        public double? regularMarketPrice { get; set; }
        public string? currency { get; set; }
    }

    public class AcaoResponse()
    {
        //Propriedades
        public List<Acao>? results { get; set; }
    }
}
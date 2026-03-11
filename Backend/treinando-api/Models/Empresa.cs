using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treinando_api.Models
{
    public class Empresa
    {
        //Propriedades
        public string? cnpj { get; set; }
        public string? nome { get; set; }
        public string? fantasia { get; set; }
        public string? logradouro { get; set; }
        public string? numero { get; set; }
        public string? municipio { get; set; }
        public string? uf { get; set; }
        public string? status { get; set; }
        public string? message { get; set; }
    }
}
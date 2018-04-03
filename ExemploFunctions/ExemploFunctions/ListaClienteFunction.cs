using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using ExemploFunctions.Model;
using Newtonsoft.Json;

namespace ExemploFunctions
{
    public static class ListaClienteFunction
    {
        [FunctionName("ListaClienteFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var conexaoStr = ConfigurationManager.ConnectionStrings["funcDB"].ConnectionString;
            var conexao = new SqlConnection(conexaoStr);
            var dados = conexao.Query<Cliente>("select id,nome from cliente");
            return req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(dados));
        }
    }
}

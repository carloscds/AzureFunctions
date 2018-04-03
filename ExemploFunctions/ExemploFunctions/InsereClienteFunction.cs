using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExemploFunctions.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Dapper;
using System.Collections.Generic;

namespace ExemploFunctions
{
    public static class InsereClienteFunction
    {
        [FunctionName("InsereClienteFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var data = await req.Content.ReadAsAsync<Cliente>();
            if(data == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Dados inválidos");
            }
            var conexaoStr = ConfigurationManager.ConnectionStrings["funcDB"].ConnectionString;
            var conexao = new SqlConnection(conexaoStr);
            conexao.Execute("insert into cliente(nome) values(@nome)",
                    new Dictionary<string, object>() { { "@nome", data.Nome } });
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}

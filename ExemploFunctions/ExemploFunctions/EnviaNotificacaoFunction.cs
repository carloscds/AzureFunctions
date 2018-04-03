using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Dapper;
using ExemploFunctions.Model;

namespace ExemploFunctions
{
    public static class EnviaNotificacaoFunction
    {
        [FunctionName("EnviaNotificacaoFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var conexaoStr = ConfigurationManager.ConnectionStrings["funcDB"].ConnectionString;
            var conexao = new SqlConnection(conexaoStr);

            try
            {
                var dados = conexao.Query<Cliente>("select id,nome from cliente");
                foreach (var c in dados)
                {
                    var msg = $"Notificação enviada para {c.Nome}";
                    conexao.Execute("insert into notificacao values(@data, @cliente, @mensagem)",
                            new Dictionary<string, object>() { { "@data", DateTime.Now }, { "@cliente", c.ID }, { "@mensagem", msg } });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}

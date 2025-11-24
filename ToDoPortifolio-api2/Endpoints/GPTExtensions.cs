using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OpenAI.Chat;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static ToDoPortifolio.API.Response.TarefaResponse;
using ToDoPortifolio.Dados.BancoDeDados;
using ToDoPortifolio.Modelo.Modelos;

namespace ToDoPortifolio.API.Endpoints
{
    public static class GPTExtensions
    {


        private static string GetGPTResponse(string prompt)
        {
            ChatClient client = new(model: "gpt-4o", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            ChatCompletion completion = client.CompleteChat(prompt);
            var response = $"[ASSISTANT]: {completion.Content[0].Text}";
            Console.WriteLine(response);
            return $"{response}";
        }
        public static void AddEndPointsGPT(this WebApplication app)
        {
            var gptGrupo = app.MapGroup("/gpt").RequireAuthorization();

            gptGrupo.MapGet("/teste", ([FromServices] DAL<Tarefa> tarefaDal, ClaimsPrincipal user) =>
            {
                var userIdClaim = user.FindFirst("UserID")?.Value;
                if (userIdClaim is null)
                {
                    return Results.Unauthorized();
                }

                var lista = tarefaDal.Listar().Where(t => t.UsuarioId == int.Parse(userIdClaim)).ToList();
                if (lista is null)
                {
                    return Results.NotFound("Esse usuario não possui nenhuma tarefa");
                }

                //var listaDto = lista.Select(t => new TarefaResponseAPI{ id = t.Id, titulo = t.Nome, descricao = t.Descricao, urgencia = t.Urgencia, status = t.Status, usuarioId = t.UsuarioId, DiasRestantes =   }).ToList();
                var listaDto = FiltroDto(lista);
                var response = GetGPTResponse($"[Contexto] Temos um Software que faz gerenciamento de tarefas de usuarios, para que os usuarios possam " +
                    $"cadastrar/visualizar/editar/deletar suas tarefas e ter o controle destas e oferecemos um serviço de insight inteligente com IA que terceirizamos para você." +
                    $"Com base nessa lista desse usuario: '{listaDto}', de maneira sucinta e objetiva, de dicas de como o usuario pode otimizar suas tarefas.");
                return Results.Ok(response);
            });


        }

        private static string FiltroDto(IEnumerable<Tarefa> tarefas)
        {
            var tarefasDto = new List<TarefaResponseAPI>();

            foreach (var tarefa in tarefas)
            {

                int diasRestantes = (tarefa.DataCriacao.ToDateTime(TimeOnly.MinValue) - DateOnly.FromDateTime(DateTime.Now).ToDateTime(TimeOnly.MinValue)).Days;

                tarefasDto.Add(new TarefaResponseAPI(tarefa.Id, tarefa.Nome, tarefa.Descricao, tarefa.Urgencia, tarefa.Status, tarefa.UsuarioId, diasRestantes, tarefa.DataCriacao, tarefa.Concluido));
            }

            var listaJson = JsonSerializer.Serialize(tarefasDto, new JsonSerializerOptions { WriteIndented = true });

            return listaJson;
        }

    }

}


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using static ToDoPortifolio.API.Response.TarefaResponse;
using ToDoPortifolio.API.Requests;
using ToDoPortifolio.Dados.BancoDeDados;
using ToDoPortifolio.Modelo.Modelos;

namespace ToDoPortifolio.API.Endpoints
{
    public static class TarefasExtensions
    {
        public static void AddEndPointsTarefas(this WebApplication app)
        {
            var tarefasGrupo = app.MapGroup("/tarefas").RequireAuthorization();

            tarefasGrupo.MapGet("/overview", ([FromServices] DAL<Tarefa> tarefaDal, ClaimsPrincipal user) =>
            {
                var userIdClaim = user.FindFirst("UserID")?.Value;

                if (userIdClaim is null)
                {
                    return Results.Unauthorized();
                }

                var id = int.Parse(userIdClaim);

                var lista = tarefaDal.Listar().Where(t => t.UsuarioId == id).ToList();
                if (lista is null)
                {
                    return Results.NotFound("Esse usuario não possui nenhuma tarefa");
                }

                try
                {
                    var listaDto = FiltroDto(lista);
                    var dados = FiltroOverview(listaDto);
                    return Results.Ok(dados);

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao listar tarefas: {ex.Message}");
                }

            });

            tarefasGrupo.MapGet("", ([FromServices] DAL<Tarefa> tarefaDal, ClaimsPrincipal user) =>
            {

                var userIdClaim = user.FindFirst("UserID")?.Value;

                if (userIdClaim is null)
                {
                    return Results.Unauthorized();
                }

                var id = int.Parse(userIdClaim);

                var lista = tarefaDal.Listar().Where(t => t.UsuarioId == id).ToList();
                if (lista is null)
                {
                    return Results.NotFound("Esse usuario não possui nenhuma tarefa");
                }

                try
                {
                    var listaDto = FiltroDto(lista);
                    var listaDtoTarefasNaoConcluidas = listaDto.Where(t => !t.concluido).ToList();


                    return Results.Ok(listaDtoTarefasNaoConcluidas);

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao listar tarefas: {ex.Message}");
                }


            });

            tarefasGrupo.MapPut("/concluir/{id}", ([FromServices] DAL<Tarefa> tarefaDal, int id) => {
                var tarefaExistente = tarefaDal.ObterPor(t => t.Id == id);

                if (tarefaExistente is null)
                {
                    return Results.Problem("Tarefa não encontrada na base de dados");
                }

                try
                {
                    tarefaExistente.Concluido = true;
                    tarefaDal.Atualizar(tarefaExistente);
                    return Results.Ok("Tarefa atualizada com sucesso.");

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Problema ao atualizar a tarefa no banco: {ex.Message}");
                }

            });

            tarefasGrupo.MapPost("", ([FromServices] DAL<Tarefa> tarefaDal, [FromBody] TarefaRequestPost tarefa, ClaimsPrincipal user) => {

                var userIdClaim = user.FindFirst("UserID")?.Value;

                if (userIdClaim is null)
                {
                    return Results.Unauthorized();
                }

                var id = int.Parse(userIdClaim);

                try
                {
                    tarefaDal.Adicionar(new Tarefa
                    {
                        Nome = tarefa.titulo,
                        Descricao = tarefa.descricao,
                        Urgencia = tarefa.Urgencia,
                        Status = tarefa.status,
                        DataCriacao = tarefa.dataDeEntrega,
                        UsuarioId = id
                    });
                    return Results.Created();

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao adicionar tarefa{ex.Message}");
                }

            });

            tarefasGrupo.MapPut("", ([FromServices] DAL<Tarefa> tarefaDal, [FromBody] TarefaRequestPut tarefa) => {
                var tarefaExistente = tarefaDal.ObterPor(t => t.Id == tarefa.id);

                if (tarefaExistente is null)
                {
                    return Results.Problem("Tarefa não encontrada na base de dados");
                }

                try
                {
                    tarefaExistente.Nome = tarefa.titulo;
                    tarefaExistente.Descricao = tarefa.descricao;
                    tarefaExistente.DataCriacao = tarefa.dataDeEntrega;
                    tarefaExistente.Urgencia = tarefa.Urgencia;
                    tarefaExistente.Status = tarefa.status;

                    tarefaDal.Atualizar(tarefaExistente);
                    return Results.Ok("Tarefa atualizada com sucesso.");

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Problema ao atualizar a tarefa no banco: {ex.Message}");
                }

            });

            tarefasGrupo.MapDelete("{id}", ([FromServices] DAL<Tarefa> tarefaDal, int id) => {
                var tarefaExistente = tarefaDal.ObterPor(t => t.Id == id);
                if (tarefaExistente is null)
                {
                    return Results.Problem("Tarefa não encontrada na base de dados");
                }

                try
                {
                    tarefaDal.Remover(tarefaExistente);
                    return Results.Ok("Tarefa removida com sucesso.");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao remover tarefa: {ex.Message}");
                }
            });
        }

        private static TarefaResponseAPIOverview FiltroOverview(IEnumerable<TarefaResponseAPI> listaTarefa)
        {
            var totalTarefas = listaTarefa.Count();
            var tarefasAtrasadas = listaTarefa.Count(t => t.DiasRestantes < 0 && !t.concluido);
            var tarefasNoPrazo = listaTarefa.Count(t => t.DiasRestantes >= 0 && !t.concluido);
            var tarefasConcluidas = listaTarefa.Count(t => t.concluido);
            var tarefasAltaUrgencia = listaTarefa.Count(t => t.urgencia == "Alta" && !t.concluido);
            var tarefasMediaUrgencia = listaTarefa.Count(t => t.urgencia == "Media" && !t.concluido);
            var tarefasBaixaUrgencia = listaTarefa.Count(t => t.urgencia == "Baixa" && !t.concluido);

            return new TarefaResponseAPIOverview(
                totalTarefas,
                tarefasAtrasadas,
                tarefasNoPrazo,
                tarefasConcluidas,
                tarefasAltaUrgencia,
                tarefasMediaUrgencia,
                tarefasBaixaUrgencia
            );
        }
        private static ICollection<TarefaResponseAPI> FiltroDto(IEnumerable<Tarefa> tarefas)
        {
            var tarefasDto = new List<TarefaResponseAPI>();

            foreach (var tarefa in tarefas)
            {

                int diasRestantes = (tarefa.DataCriacao.ToDateTime(TimeOnly.MinValue) - DateOnly.FromDateTime(DateTime.Now).ToDateTime(TimeOnly.MinValue)).Days;

                tarefasDto.Add(new TarefaResponseAPI(tarefa.Id, tarefa.Nome, tarefa.Descricao, tarefa.Urgencia, tarefa.Status, tarefa.UsuarioId, diasRestantes, tarefa.DataCriacao, tarefa.Concluido));
            }

            return tarefasDto;
        }


    }
}

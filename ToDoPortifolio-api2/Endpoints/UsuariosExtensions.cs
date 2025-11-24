using Microsoft.AspNetCore.Mvc;
using ToDoPortifolio.Dados.BancoDeDados;
using ToDoPortifolio.Modelo.Modelos;
using ToDoPortifolio.API.Requests;
using static ToDoPortifolio.API.Response.UsuarioResponse;
using MiniValidation;

namespace ToDoPortifolio.API.Endpoints
{
    public static class UsuariosExtensions
    {
        public static void AddEndPointsUsuarios(this WebApplication app)
        {

            app.MapPost("/usuario", ([FromServices] DAL<Usuario> userDal, [FromBody] UsuarioRequestPost user) => {
                

                try
                {
                    //Validando dos dados recebidos
                    if (!MiniValidator.TryValidate(user, out var errors))
                    {
                        return Results.ValidationProblem(errors);
                    }

                    userDal.Adicionar(
                    new Usuario
                    {
                        Nome = user.nome,
                        Email = user.email,
                        Senha = user.senha
                    }
                );

                    return Results.Created();

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao adicionar usuário: {ex.Message}");
                }
            });

            var usuarioGrupo = app.MapGroup("/usuarios").RequireAuthorization();

            usuarioGrupo.MapGet("", ([FromServices] DAL<Usuario> dal) =>
            {
                try
                {
                    var usuarios = dal.Listar();
                    var usuariosDto = FiltroDto(usuarios);

                    return Results.Ok(usuariosDto);

                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao listar usuários: {ex.Message}");
                }
            });

            usuarioGrupo.MapPut("", ([FromServices] DAL<Usuario> userDal, [FromBody] UsuarioRequestPut user) => {

                var usuarioExistente = userDal.ObterPor(u => u.Id == user.id);

                if (usuarioExistente is null)
                {
                    return Results.Problem("Usuario não encontrado na base de dados");
                }

                usuarioExistente.Nome = user.nome;
                usuarioExistente.Email = user.email;
                usuarioExistente.Senha = user.senha;

                try
                {
                    userDal.Atualizar(usuarioExistente);
                    return Results.Ok("Usuário atualizado com sucesso.");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao atualizar usuário: {ex.Message}");
                }


            });

            usuarioGrupo.MapDelete("{id}", ([FromServices] DAL<Usuario> userDal, int id) => {
                var usuarioExistente = userDal.ObterPor(u => u.Id == id);
                if (usuarioExistente is null)
                {
                    return Results.Problem("Usuario não encontrado na base de dados");
                }

                try
                {
                    userDal.Remover(usuarioExistente);
                    return Results.Ok("Usuário removido com sucesso.");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Erro ao remover usuário: {ex.Message}");
                }

            });


        }

        private static ICollection<UsuarioResponseAPI> FiltroDto(IEnumerable<Usuario> usuarios)
        {

            var listaResponse = usuarios.Select(u => new UsuarioResponseAPI
            (
                 u.Nome,
                 u.Email,
                 u.Senha
            )).ToList();

            return listaResponse;
        }
    }
}

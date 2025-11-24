using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoPortifolio.Dados.BancoDeDados;
using ToDoPortifolio.Modelo.Modelos;
using ToDoPortifolio.API.Requests;
public static class AutenticaExtensions
{
    public static void AddEndPointsAutentica(this WebApplication app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/login", (LoginRequests login, IConfiguration config, DAL<Usuario> dal) =>
        {
            var usuario = dal.ObterPor(u =>
                u.Email == login.Email && u.Senha == login.Senha);

            if (usuario is null)
                return Results.Unauthorized();

            var token = GenerateJwtToken(usuario.Email, usuario.Id, config);
            return Results.Ok(new { token });
        });

        group.MapGet("/me", [Authorize] (ClaimsPrincipal user) =>
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            return Results.Ok(new { email });
        });


    }

    private static string GenerateJwtToken(string email, int id, IConfiguration config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] { new Claim(ClaimTypes.Email, email), new Claim("UserID", id.ToString()) };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}



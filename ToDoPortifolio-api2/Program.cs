using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using ToDoPortifolio.API.Endpoints;
using ToDoPortifolio.Dados.BancoDeDados;
using ToDoPortifolio.Modelo.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("https://todoportifolio.vercel.app", "http://localhost:5173") // React
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // necessário se usar autenticação (JWT, cookies, etc)
        });
});

builder.Services.AddEndpointsApiExplorer(); //Mapeia os endpoints
builder.Services.AddSwaggerGen(); //Gera a documentação Swagger para a API

builder.Services.AddDbContext<ToDoPortifolioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexaoString"))); // Injeção de dependência para o contexto do banco de dados, utilizando a string de conexão do appsettings.json
//builder.Services.AddDbContext<ToDoPortifolioContext()> Injeção de dependência para o contexto do banco de dados(Com isso não terei que instanciar o Contexto DO BANCO toda vez que instanciar DAL)
builder.Services.AddTransient<DAL<Usuario>>(); // Injeção de dependência para a classe DAL com o tipo Usuario. Instancia a DAL do tipo Usuario toda vez que for requisitada
builder.Services.AddTransient<DAL<Tarefa>>(); // Injeção de dependência para a classe DAL com o tipo Tarefa. Instancia a DAL do tipo Tarefa, ja com a classe tarefa instanciada internamente, toda vez que for requisitada


var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.AddEndPointsGPT();
app.AddEndPointsAutentica();
app.AddEndPointsUsuarios();
app.AddEndPointsTarefas();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


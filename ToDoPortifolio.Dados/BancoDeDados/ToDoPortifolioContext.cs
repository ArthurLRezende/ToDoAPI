using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoPortifolio.Modelo.Modelos;
using System.IO;

namespace ToDoPortifolio.Dados.BancoDeDados
{
    public class ToDoPortifolioContext : DbContext
    {

        //private string conexaoString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ToDoPortifolio;Integrated Security=True;" +
        //"Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; //Inspeciona o banco e vai em cadeia de conexão

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlServer(conexaoString)//Conecta o banco de dados
        //        .UseLazyLoadingProxies();//Carrega os dados relacionados quando necessário
        //} Utilizado para conexão local 
        public ToDoPortifolioContext(DbContextOptions<ToDoPortifolioContext> options)
       : base(options) //Chama o construtor da classe base DbContext
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } //Liga a classe Usuario com o banco de dados pelo Entity Framework e cria a tabela Usuarios
        public DbSet<Tarefa> Tarefas { get; set; } //Liga a classe Tarefa com o banco de dados pelo Entity Framework e cria a tabela Tarefas

    }
}

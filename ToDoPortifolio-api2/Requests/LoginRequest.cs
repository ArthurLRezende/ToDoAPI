using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Estrutura de Requisição Login
namespace ToDoPortifolio.API.Requests
{
    public record LoginRequests(string Email, string Senha);

}
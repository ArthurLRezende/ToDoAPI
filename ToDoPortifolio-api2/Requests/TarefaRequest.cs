using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Estrutura de Requisições para Tarefa
namespace ToDoPortifolio.API.Requests
{
    public record TarefaRequestPost(
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
        string titulo,string descricao, DateOnly dataDeEntrega, string Urgencia, string status, int usuarioId);
    public record TarefaRequestPut(int id,
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
         string titulo, string descricao, DateOnly dataDeEntrega, string Urgencia, string status);
    public record TarefaRequestDelete(int id);
}

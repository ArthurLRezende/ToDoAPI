using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Estrutura de Requisições para Usuario 
namespace ToDoPortifolio.API.Requests
{
    public record UsuarioRequestPost(
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
        string nome,
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
        string email,
         [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]string senha);
    public record UsuarioRequestPut(int id,
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
        string nome,
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
        string email,
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "O Nome deve ter no mínimo 2 caracteres")]
        string senha);
    public record UsuarioRequestDelete(int id);
}

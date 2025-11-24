using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoPortifolio.API.Response
{
    internal class UsuarioResponse
    {
        public record UsuarioResponseAPI(string nome, string email, string senha);
    }
}

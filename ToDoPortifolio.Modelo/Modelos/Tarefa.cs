using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoPortifolio.Modelo.Modelos
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UsuarioId { get; set; }
        public DateOnly DataCriacao { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? Urgencia { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public string? Status { get; set; } = "Normal";
        public bool Concluido { get; set; } = false;
        public virtual Usuario Usuario { get; set; }

        public Tarefa() { }
        public Tarefa(string nome, DateOnly data, int id)
        {
            Nome = nome;
            UsuarioId = id;
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} - {DataCriacao} - {Urgencia} - {Descricao} - {Status}";
        }

    }
}

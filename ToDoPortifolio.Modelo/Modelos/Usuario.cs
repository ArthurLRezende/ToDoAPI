using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoPortifolio.Modelo.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public virtual ICollection<Tarefa> Tarefas { get; set; }
        public Usuario()
        {
            Tarefas = new List<Tarefa>();
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} - {Email}";
        }

        public void AdicionarTarefa(Tarefa tarefa)
        {
            Tarefas.Add(tarefa);
        }
        public int ContarNumeros(int num1, int num2)
        { 
            return num1 + num2;
        }


        public void ExibirTarefas()
        {
            Console.WriteLine($"Tarefas do usuário {Nome}:");
            if (Tarefas == null || Tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa encontrada.");
                return;
            }
            else
            {
                foreach (var tarefa in Tarefas)
                {
                    Console.WriteLine(tarefa);
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoPortifolio.Dados.BancoDeDados
{
    public class DAL<T> where T : class
    {
        private readonly ToDoPortifolioContext context;
        public DAL(ToDoPortifolioContext contexto)
        {
            this.context = contexto;
        }

        public ICollection<T> Listar()
        {
            return context.Set<T>().ToList();
        }
        public void Adicionar(T objeto)//Recebe como argumento um objeto
        {
            context.Set<T>().Add(objeto);
            context.SaveChanges();
        }

        public void Atualizar(T objeto)
        {
            context.Set<T>().Update(objeto);
            context.SaveChanges();
        }

        public void Remover(T objeto)
        {
            context.Set<T>().Remove(objeto);
            context.SaveChanges();
        }

        public T? ObterPor(Func<T, bool> condicao)
        {
            return context.Set<T>().FirstOrDefault(condicao);
        }

    }
}

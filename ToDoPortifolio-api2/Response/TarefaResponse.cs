using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoPortifolio.API.Response
{
    internal class TarefaResponse
    {
        public record TarefaResponseAPI(int id, string titulo, string descricao, string urgencia, string status, int usuarioId, int DiasRestantes, DateOnly dataPrazo, bool concluido);
        public record TarefaResponseAPIOverview(int totalTarefas, int tarefasAtrasadas, int tarefasNoPrazo, int tarefasConcluidas, int tarefasAltaUrgencia, int tarefasMediaUrgencia, int tarefasBaixaUrgencia);
    }
}

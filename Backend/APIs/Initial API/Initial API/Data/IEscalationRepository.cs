using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public interface IEscalationRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public EscalationModel GetSingleEscalation(int idEscalation);
        public IActionResult EditEscalation(EscalationModel escalation);
        public IActionResult AddEscalation(EscalationModel Escalation);
        public IActionResult DeleteEscalation(int idEscalation);
        public IEnumerable<EscalationModel> GetEscalations();

    }
}

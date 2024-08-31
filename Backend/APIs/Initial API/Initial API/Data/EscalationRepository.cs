using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public class EscalationRepository : IEscalationRepository
    {
        DataContextEF _entityFramework;
        public EscalationRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }
        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }
        public void AddEntity<T>(T entity)
        {
            if (entity != null)
            {
                _entityFramework.Add(entity);
            }
        }
        public void RemoveEntity<T>(T entity)
        {
            if (entity != null)
            {
                _entityFramework.Remove(entity);
            }
        }
        public IEnumerable<EscalationModel> GetEscalations()
        {
            IEnumerable<EscalationModel> Escalations = _entityFramework.Escalations.ToList<EscalationModel>();
            return Escalations;
        }
        public EscalationModel GetSingleEscalation(int idEscalation)
        {
            EscalationModel? Escalation = _entityFramework.Escalations.Where(Escalation => Escalation.IdEscalation == idEscalation).FirstOrDefault();

            if (Escalation != null)
            {
                return Escalation;
            }
            throw new Exception("Failed to Get Escalation");
        }
        public IActionResult EditEscalation(EscalationModel escalation)
        {
            EscalationModel? EscalationOnDb = _entityFramework.Escalations.Where(Escalation => Escalation.IdEscalation == escalation.IdEscalation).FirstOrDefault();

            if (EscalationOnDb != null)
            {
                EscalationOnDb.EmployeeNumber = escalation.EmployeeNumber;
                EscalationOnDb.Notes = escalation.Notes ?? EscalationOnDb.Notes;
                EscalationOnDb.IdCustomer = escalation.IdCustomer;

                if (_entityFramework.SaveChanges() > 0)
                {
                    return (IActionResult)EscalationOnDb;
                }
            }
            throw new Exception("Failed to Update or edit Escalation or was not found");
        }
        public IActionResult AddEscalation(EscalationModel Escalation)
        {
            EscalationModel EscalationToDb = new EscalationModel();

            EscalationToDb.Notes = Escalation.Notes;
            EscalationToDb.IdCustomer = Escalation.IdCustomer;
            EscalationToDb.EmployeeNumber = Escalation.EmployeeNumber;
            EscalationToDb.CreationDate = DateTime.Now;
            EscalationToDb.Departament = Escalation.Departament;

            _entityFramework.Add(EscalationToDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return (IActionResult)EscalationToDb;
            }
            throw new Exception("Failed to Add Escalation");
        }
        public IActionResult DeleteEscalation(int idEscalation)
        {
            EscalationModel? EscalationOnDb = _entityFramework.Escalations.Where(Escalation => Escalation.IdEscalation == idEscalation).FirstOrDefault<EscalationModel>();

            if (EscalationOnDb != null)
            {
                _entityFramework.Escalations.Remove(EscalationOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return (IActionResult)EscalationOnDb;
                }
            }
            throw new Exception("Failed to Update or delete Escalation");

        }
    }
}

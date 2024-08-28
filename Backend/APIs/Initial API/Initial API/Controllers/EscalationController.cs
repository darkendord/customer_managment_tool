using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EscalationController : ControllerBase
    {
        DataContextEF _entityFramework;
        public EscalationController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        [HttpGet("GetEscalations")]
        public IEnumerable<EscalationModel> GetEscalations()
        {
            IEnumerable<EscalationModel> Escalations = _entityFramework.Escalations.ToList<EscalationModel>();
            return Escalations;
        }

        [HttpGet("GetSingleEscalation/{idEscalation}")]
        public EscalationModel GetSingleEscalation(int idEscalation)
        {
            EscalationModel? Escalation = _entityFramework.Escalations.Where(Escalation => Escalation.IdEscalation == idEscalation).FirstOrDefault();

            if (Escalation != null)
            {
                return Escalation;
            }
            throw new Exception("Failed to Get Escalation");
        }

        [HttpPut("EditEscalation")]
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
                    return Ok(EscalationOnDb);
                }
            }
            throw new Exception("Failed to Update or edit Escalation or was not found");
        }

        [HttpPost("PostEscalation")]
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
                return Ok(EscalationToDb);
            }
            throw new Exception("Failed to Add Escalation");
        }

        [HttpDelete("DeleteEscalation/{idEscalation}")]
        public IActionResult DeleteEscalation(int idEscalation)
        {
            EscalationModel? EscalationOnDb = _entityFramework.Escalations.Where(Escalation => Escalation.IdEscalation == idEscalation).FirstOrDefault<EscalationModel>();

            if (EscalationOnDb != null)
            {
                _entityFramework.Escalations.Remove(EscalationOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(EscalationOnDb);
                }
            }
            throw new Exception("Failed to Update or delete Escalation");

        }
    }
}

using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EscalationController : ControllerBase
    {
        IEscalationRepository _escalationRepository;
        public EscalationController(IEscalationRepository escalationRepository)
        {
            _escalationRepository = escalationRepository;
        }

        [HttpGet("GetEscalations")]
        public IEnumerable<EscalationModel> GetEscalations()
        {
            IEnumerable<EscalationModel> Escalations = _escalationRepository.GetEscalations();
            return Escalations;
        }

        [HttpGet("GetSingleEscalation/{idEscalation}")]
        public EscalationModel GetSingleEscalation(int idEscalation)
        {
            EscalationModel? Escalation = _escalationRepository.GetSingleEscalation(idEscalation);

            if (Escalation != null)
            {
                return Escalation;
            }
            throw new Exception("Failed to Get Escalation");
        }

        [HttpPut("EditEscalation")]
        public IActionResult EditEscalation(EscalationModel escalation)
        {
            IActionResult EscalationOnDb = _escalationRepository.EditEscalation(escalation);

                if (_escalationRepository.SaveChanges())
                {
                    return Ok(EscalationOnDb);
                }
            throw new Exception("Failed to Update or edit Escalation or was not found");
        }

        [HttpPost("PostEscalation")]
        public IActionResult AddEscalation(EscalationModel escalation)
        {
            IActionResult EscalationToDb =  _escalationRepository.AddEscalation(escalation);
            if (_escalationRepository.SaveChanges())
            {
                return Ok(EscalationToDb);
            }
            throw new Exception("Failed to Add Escalation");
        }

        [HttpDelete("DeleteEscalation/{idEscalation}")]
        public IActionResult DeleteEscalation(int idEscalation)
        {
            IActionResult EscalationOnDb = _escalationRepository.DeleteEscalation(idEscalation);
                if (_escalationRepository.SaveChanges())
                {
                    return Ok(EscalationOnDb);
                }
            throw new Exception("Failed to Update or delete Escalation");

        }
    }
}

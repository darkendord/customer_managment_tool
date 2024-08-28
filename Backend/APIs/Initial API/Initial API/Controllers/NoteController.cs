using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        DataContextEF _entityFramework;
        public NoteController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        [HttpGet("GetNotes")]
        public IEnumerable<NoteModel> GetNotes()
        {
            IEnumerable<NoteModel> Notes = _entityFramework.Notes.ToList<NoteModel>();
            return Notes;
        }

        [HttpGet("GetSingleNote/{idNote}")]
        public NoteModel GetSingleNote(int idNote)
        {
            NoteModel? Note = _entityFramework.Notes.Where(Note => Note.IdNote == idNote).FirstOrDefault();

            if (Note != null)
            {
                return Note;
            }
            throw new Exception("Failed to Get Note");
        }

        [HttpPut("EditNote")]
        public IActionResult EditNote(NoteModel note)
        {
            NoteModel? NoteOnDb = _entityFramework.Notes.Where(Note => Note.IdNote == note.IdNote).FirstOrDefault();

            if (NoteOnDb != null)
            {
                NoteOnDb.CreatedBy = note.CreatedBy ?? NoteOnDb.CreatedBy;
                NoteOnDb.Detail = note.Detail ?? NoteOnDb.Detail;
                NoteOnDb.IdCustomer = NoteOnDb.IdCustomer;
                NoteOnDb.EmployeeNumber = NoteOnDb.EmployeeNumber;

                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(NoteOnDb);
                }
            }
            throw new Exception("Failed to Update or edit Note or was not found");
        }

        [HttpPost("PostNote")]
        public IActionResult AddNote(NoteModel note)
        {
            NoteModel NoteToDb = new NoteModel();

            NoteToDb.CreatedBy = note.CreatedBy;
            NoteToDb.Detail = note.Detail;
            NoteToDb.IdCustomer = note.IdCustomer;
            NoteToDb.EmployeeNumber = note.EmployeeNumber;
            NoteToDb.CreationDate = DateTime.Now;

            _entityFramework.Add(NoteToDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok(NoteToDb);
            }
            throw new Exception("Failed to Add Note");
        }

        [HttpDelete("DeleteNote/{idNote}")]
        public IActionResult DeleteNote(int idNote)
        {
            NoteModel? NoteOnDb = _entityFramework.Notes.Where(Note => Note.IdNote == idNote).FirstOrDefault<NoteModel>();

            if (NoteOnDb != null)
            {
                _entityFramework.Notes.Remove(NoteOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(NoteOnDb);
                }
            }
            throw new Exception("Failed to Update or delete Note");

        }
    }
}

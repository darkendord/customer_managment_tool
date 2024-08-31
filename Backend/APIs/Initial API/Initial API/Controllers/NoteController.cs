using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        INoteRepository _noteRepository;
        public NoteController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet("GetNotes")]
        public IEnumerable<NoteModel> GetNotes()
        {
            IEnumerable<NoteModel> Notes = _noteRepository.GetNotes();
            return Notes;
        }

        [HttpGet("GetSingleNote/{idNote}")]
        public NoteModel GetSingleNote(int idNote)
        {
            NoteModel? Note = _noteRepository.GetSingleNote(idNote);

            if (Note != null)
            {
                return Note;
            }
            throw new Exception("Failed to Get Note");
        }

        [HttpPut("EditNote")]
        public IActionResult EditNote(NoteModel note)
        {
            IActionResult NoteOnDb = _noteRepository.EditNote(note);

                if (_noteRepository.SaveChanges())
                {
                    return Ok(NoteOnDb);
                }
            throw new Exception("Failed to Update or edit Note or was not found");
        }

        [HttpPost("PostNote")]
        public IActionResult AddNote(NoteModel note)
        {
            IActionResult NoteToDb = _noteRepository.AddNote(note);

            if (_noteRepository.SaveChanges())
            {
                return Ok(NoteToDb);
            }
            throw new Exception("Failed to Add Note");
        }

        [HttpDelete("DeleteNote/{idNote}")]
        public IActionResult DeleteNote(int idNote)
        {
            IActionResult noteOnDb = _noteRepository.DeleteNote(idNote);

                if (_noteRepository.SaveChanges())
                {
                    return Ok(noteOnDb);
                }
            throw new Exception("Failed to Update or delete Note");

        }
    }
}

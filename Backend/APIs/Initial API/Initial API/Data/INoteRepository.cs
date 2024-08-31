using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public interface INoteRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public IEnumerable<NoteModel> GetNotes();
        public NoteModel GetSingleNote(int idNote);
        public IActionResult EditNote(NoteModel note);
        public IActionResult AddNote(NoteModel note);
        public IActionResult DeleteNote(int idNote);
    }
}

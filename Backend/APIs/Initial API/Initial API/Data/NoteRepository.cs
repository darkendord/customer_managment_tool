using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Initial_API.Data
{
    public class NoteRepository : INoteRepository
    {
        DataContextEF _entityFramework;
        public NoteRepository(IConfiguration config)
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
        public IEnumerable<NoteModel> GetNotes()
        {
            IEnumerable<NoteModel> Notes = _entityFramework.Notes.ToList<NoteModel>();
            return Notes;
        }
        public NoteModel GetSingleNote(int idNote)
        {
            NoteModel? Note = _entityFramework.Notes.Where(Note => Note.IdNote == idNote).FirstOrDefault();

            if (Note != null)
            {
                return Note;
            }
            throw new Exception("Failed to Get Note");
        }
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
                    return (IActionResult)NoteOnDb;
                }
            }
            throw new Exception("Failed to Update or edit Note or was not found");
        }
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
                return (IActionResult)NoteToDb;
            }
            throw new Exception("Failed to Add Note");
        }
        public IActionResult DeleteNote(int idNote)
        {
            NoteModel? NoteOnDb = _entityFramework.Notes.Where(Note => Note.IdNote == idNote).FirstOrDefault<NoteModel>();

            if (NoteOnDb != null)
            {
                _entityFramework.Notes.Remove(NoteOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return (IActionResult)NoteOnDb;
                }
            }
            throw new Exception("Failed to Update or delete Note");

        }
    }
}
    
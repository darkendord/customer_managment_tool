using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public interface ICardRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public IEnumerable<CardModel> GetCards();
        public CardModel GetSingleCard(string numberCard);
        public IActionResult EditCard(CardModel card);
        public IActionResult AddCard(CardModel card);
        public IActionResult DeleteCard(string numberCard);
    }
}

using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public class CardRepository : ICardRepository
    {
        DataContextEF _entityFramework;
        public CardRepository(IConfiguration config)
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
        public IEnumerable<CardModel> GetCards()
        {
            IEnumerable<CardModel> Cards = _entityFramework.Cards.ToList<CardModel>();
            return Cards;
        }
        public CardModel GetSingleCard(string numberCard)
        {
            CardModel? Card = _entityFramework.Cards.Where(Card => Card.NumberCard == numberCard).FirstOrDefault();

            if (Card != null)
            {
                return Card;
            }
            throw new Exception("Failed to Get Card");
        }
        public IActionResult EditCard(CardModel card)
        {
            CardModel? CardOnDb = _entityFramework.Cards.Where(Card => Card.NumberCard == card.NumberCard).FirstOrDefault();

            if (CardOnDb != null)
            {
                CardOnDb.BillingCycle = card.BillingCycle ?? CardOnDb.BillingCycle;
                CardOnDb.Balance = card.Balance > 0 ? card.Balance : CardOnDb.Balance;
                CardOnDb.IsCardActive = card.IsCardActive || CardOnDb.IsCardActive;
                if (_entityFramework.SaveChanges() > 0)
                {
                    return (IActionResult)CardOnDb;
                }
            }
            throw new Exception("Failed to Update or edit Card or was not found");
        }
        public IActionResult AddCard(CardModel card)
        {
            CardModel CardToDb = new CardModel();

            CardToDb.BillingCycle = card.BillingCycle;
            CardToDb.Balance = card.Balance;
            CardToDb.IsCardActive = card.IsCardActive;
            CardToDb.ExprirationDate = card.ExprirationDate;
            CardToDb.NumberCard = card.NumberCard;
            _entityFramework.Add(CardToDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return (IActionResult)CardToDb;
            }
            throw new Exception("Failed to Add Card");
        }
        public IActionResult DeleteCard(string numberCard)
        {
            CardModel? CardOnDb = _entityFramework.Cards.Where(Card => Card.NumberCard == numberCard).FirstOrDefault<CardModel>();

            if (CardOnDb != null)
            {
                _entityFramework.Cards.Remove(CardOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return (IActionResult)CardOnDb;
                }
            }
            throw new Exception("Failed to Update or delete Card");

        }
    }
}

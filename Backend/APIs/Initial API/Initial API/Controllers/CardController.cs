using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        DataContextEF _entityFramework;
        public CardController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        [HttpGet("GetCards")]
        public IEnumerable<CardModel> GetCards()
        {
            IEnumerable<CardModel> Cards = _entityFramework.Cards.ToList<CardModel>();
            return Cards;
        }

        [HttpGet("GetSingleCard/{numberCard}")]
        public CardModel GetSingleCard(string numberCard)
        {
            CardModel? Card = _entityFramework.Cards.Where(Card => Card.NumberCard == numberCard).FirstOrDefault();

            if (Card != null)
            {
                return Card;
            }
            throw new Exception("Failed to Get Card");
        }

        [HttpPut("EditCard")]
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
                    return Ok(CardOnDb);
                }
            }
            throw new Exception("Failed to Update or edit Card or was not found");
        }

        [HttpPost("PostCard")]
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
                return Ok(CardToDb);
            }
            throw new Exception("Failed to Add Card");
        }

        [HttpDelete("DeleteCard/{numberCard}")]
        public IActionResult DeleteCard(string numberCard)
        {
            CardModel? CardOnDb = _entityFramework.Cards.Where(Card => Card.NumberCard == numberCard).FirstOrDefault<CardModel>();

            if (CardOnDb != null)
            {
                _entityFramework.Cards.Remove(CardOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(CardOnDb);
                }
            }
            throw new Exception("Failed to Update or delete Card");

        }
    }
}

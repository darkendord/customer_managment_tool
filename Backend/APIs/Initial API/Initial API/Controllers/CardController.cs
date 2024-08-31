using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        ICardRepository _cardRepository;
        public CardController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        [HttpGet("GetCards")]
        public IEnumerable<CardModel> GetCards()
        {
            IEnumerable<CardModel> Cards = _cardRepository.GetCards();
            return Cards;
        }

        [HttpGet("GetSingleCard/{numberCard}")]
        public CardModel GetSingleCard(string numberCard)
        {
            CardModel? Card = _cardRepository.GetSingleCard(numberCard);

            if (Card != null)
            {
                return Card;
            }
            throw new Exception("Failed to Get Card");
        }

        [HttpPut("EditCard")]
        public IActionResult EditCard(CardModel card)
        {
            IActionResult CardOnDb = _cardRepository.EditCard(card);

                if (_cardRepository.SaveChanges())
                {
                    return Ok(CardOnDb);
                }
            throw new Exception("Failed to Update or edit Card or was not found");
        }

        [HttpPost("PostCard")]
        public IActionResult AddCard(CardModel card)
        {
            IActionResult CardToDb = _cardRepository.AddCard(card);

            if (_cardRepository.SaveChanges())
            {
                return Ok(CardToDb);
            }
            throw new Exception("Failed to Add Card");
        }

        [HttpDelete("DeleteCard/{numberCard}")]
        public IActionResult DeleteCard(string numberCard)
        {
            IActionResult CardOnDb = _cardRepository.DeleteCard(numberCard);
                if (_cardRepository.SaveChanges())
                {
                    return Ok(CardOnDb);
                }
            throw new Exception("Failed to Update or delete Card");

        }
    }
}

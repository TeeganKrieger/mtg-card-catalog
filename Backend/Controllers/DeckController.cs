using Microsoft.AspNetCore.Mvc;
using MTGCC.Database;
using MTGCC.Services;

namespace MTGCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeckController : ControllerBase
    {
        private AppUser ActiveUser => (AppUser)HttpContext.Items["Account"];

        private readonly AppDbContext _dbContext;

        public DeckController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("list")]
        public ActionResult<DeckListResult[]> List([FromQuery] string user = null)
        {
            AppUser activeUser = ActiveUser;

            if (user != null)
                activeUser = this._dbContext.Users.FirstOrDefault(x => x.ID == user);

            if (activeUser == null)
                return BadRequest("The specified user does not exist!");

            IEnumerable<Deck> decks = this._dbContext.Decks.Where(x => x.Owner == activeUser);
            List<DeckListResult> projectedDecks = new List<DeckListResult>();

            foreach (Deck deck in decks)
                projectedDecks.Add(new DeckListResult() { ID = deck.ID, Name = deck.Name });

            return Ok(projectedDecks.ToArray());
        }

        [HttpGet("get")]
        public ActionResult<DeckResult> Get([FromQuery] string deckID)
        {
            Deck deck = this._dbContext.Decks.FirstOrDefault(x => x.ID == deckID);

            if (deck == null)
                return BadRequest("The specified deck does not exist!");

            DeckResult result = new DeckResult() { ID = deck.ID, Name = deck.Name };

            List<DeckCardResult> deckCards = new List<DeckCardResult>();

            foreach (DeckCard card in deck.Cards)
            {
                if (card.Mode == DeckCardMode.Owned)
                {
                    MTGCard cardData = card.PlayerCard.GetMTGCard();
                    deckCards.Add(new DeckCardResult() { ID = card.ID, IsOwned = true, Data = cardData });
                }
                else
                {
                    MTGCard cardData = CardCache.SearchByID(card.CardID);
                    deckCards.Add(new DeckCardResult() { ID = card.ID, IsOwned = false, Data = cardData });
                }
            }

            result.Cards = deckCards.ToArray();

            return Ok(result);
        }

        [HttpPost("create")]
        [AuthorizeSession]
        public ActionResult<string> Create([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name must include valid text!");

            Deck newDeck = new Deck(ActiveUser, name);

            this._dbContext.Add(newDeck);
            this._dbContext.SaveChanges();

            return newDeck.ID;
        }

        [HttpPost("delete")]
        [AuthorizeSession]
        public ActionResult Delete([FromBody] string deckID)
        {
            Deck deck = this._dbContext.Decks.FirstOrDefault(x => x.ID == deckID);

            if (deck == null)
                return BadRequest("The specified deck does not exist!");

            this._dbContext.Remove(deck);
            this._dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("card/add")]
        [AuthorizeSession]
        public ActionResult AddCard([FromBody]AddCardModel model)
        {
            Deck deck = this._dbContext.Decks.FirstOrDefault(x => x.ID == model.DeckID);

            if (deck == null)
                return BadRequest("The specified deck does not exist!");

            if (model.IsOwnedCard)
            {
                PlayerCard playerCard = this._dbContext.PlayerCards.FirstOrDefault(x => x.ID == model.CardID);

                if (playerCard == null)
                    return BadRequest("The specified card should be owned by a user, but it doesn't exist!");

                DeckCard deckCard = new DeckCard(deck, playerCard);
                this._dbContext.DeckCards.Add(deckCard);
                this._dbContext.SaveChanges();
            }
            else
            {
                MTGCard card = CardCache.SearchByID(model.CardID);

                if (card == null)
                    return BadRequest("The specified card does not exist!");

                DeckCard deckCard = new DeckCard(deck, model.CardID);
                this._dbContext.DeckCards.Add(deckCard);
                this._dbContext.SaveChanges();
            }

            return Ok();
        }

        [HttpPost("card/remove")]
        [AuthorizeSession]
        public ActionResult RemoveCard([FromBody] RemoveCardModel model)
        {
            Deck deck = this._dbContext.Decks.FirstOrDefault(x => x.ID == model.DeckID);

            if (deck == null)
                return BadRequest("The specified deck does not exist!");

            DeckCard deckCard = this._dbContext.DeckCards.FirstOrDefault(x => x.DeckID == model.DeckID && x.ID == model.CardID);

            if (deckCard == null)
                return BadRequest("The specified card does not exist!");

            this._dbContext.Remove(deckCard);
            this._dbContext.SaveChanges();

            return Ok();
        }

        public class AddCardModel
        {
            public string DeckID { get; set; }
            public bool IsOwnedCard { get; set; }
            public string CardID { get; set; }
        }

        public class RemoveCardModel
        {
            public string DeckID { get; set; }
            public int CardID { get; set; }
        }

        public class DeckListResult
        {
            public string ID { get; set; }

            public string Name { get; set; }
        }

        public class DeckResult
        {
            public string ID { get; set; }

            public string Name { get; set; }

            public DeckCardResult[] Cards { get; set; }
        }

        public class DeckCardResult
        {
            public int ID { get; set; }

            public bool IsOwned { get; set; }

            public MTGCard Data { get; set; }
        }
    }
}

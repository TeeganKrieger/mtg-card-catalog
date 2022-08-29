using Microsoft.AspNetCore.Mvc;
using MTGCC;
using MTGCC.Database;
using MTGCC.Extensions;
using MTGCC.Services;
using MTGCC.Translators;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController : ControllerBase
    {
        private const int PAGE_SIZE = 2;

        private AppUser ActiveUser => (AppUser)HttpContext.Items["Account"];

        private readonly AppDbContext _dbContext;
        private readonly ScryfallAPITranslator _scryfallTranslator;

        public CollectionController(AppDbContext dbContext, ScryfallAPITranslator translator)
        {
            this._dbContext = dbContext;
            this._scryfallTranslator = translator;
        }

        [HttpGet("get")]
        [AuthorizeSession]
        public ActionResult<CollectionResponse> Get([FromQuery] int page)
        {
            AppUser activeUser = ActiveUser;
            int start = Math.Max(0, page) * PAGE_SIZE;

            int collectionSize = this._dbContext.PlayerCards.Where(x => x.Owner == activeUser).Count();
            IEnumerable<PlayerCard> collection = this._dbContext.PlayerCards.Where(x => x.Owner == activeUser).OrderBy(x => x.AddedDate).Skip(start).Take(PAGE_SIZE);
            List<CardModel> projectedCollection = new List<CardModel>();

            foreach (PlayerCard pCard in collection)
            {
                MTGCard cardData = pCard.GetMTGCard();
                if (cardData != null)
                    projectedCollection.Add(new CardModel() { InstanceID = pCard.ID, CardData = cardData });
            }

            return Ok(new CollectionResponse(page, (int)Math.Ceiling(collectionSize / (float)PAGE_SIZE), projectedCollection.ToArray()));
        }

        [HttpPost("search")]
        [AuthorizeSession]
        public async Task<ActionResult<CollectionResponse>> Search([FromBody] SearchBody searchBody)
        {
            int page = searchBody.Page;
            int start = Math.Max(0, page) * PAGE_SIZE;

            AppUser activeUser = ActiveUser;

            IEnumerable<PlayerCard> fullCollection = this._dbContext.PlayerCards.Where(x => x.Owner == activeUser);

            List<string> filteredCardIDs = new List<string>();

            int filterPage = 0;
            SearchResponse response;
            do
            {
                searchBody.Page = filterPage;
                response = await _scryfallTranslator.Search(searchBody);

                if (response == null)
                    break;

                MTGCard[] cards = response.AsMTGCards();
                foreach (MTGCard card in cards)
                {
                    filteredCardIDs.Add(card.ID);
                }
                filterPage++;
            }
            while (filterPage < response.total_pages);

            IEnumerable<PlayerCard> filteredCards = fullCollection.SupersectBy(filteredCardIDs, x => x.CardID);
            int collectionSize = filteredCards.Count();

            CardModel[] cardModels = filteredCards.Select(x => new CardModel() { InstanceID = x.ID, CardData = x.GetMTGCard() })
                .Skip(start).Take(PAGE_SIZE).ToArray();

            return Ok(new CollectionResponse(page, (int)Math.Ceiling(collectionSize / (float)PAGE_SIZE), cardModels));
        }

        [HttpPost("add")]
        [AuthorizeSession]
        public async Task<ActionResult> Add([FromBody] string[] cardIDs)
        {
            AppUser activeUser = ActiveUser;

            foreach (string cardID in cardIDs)
            {
                if (CardCache.SearchByID(cardID) != null)
                {
                    PlayerCard playerCard = new PlayerCard(cardID, activeUser);
                    this._dbContext.Add(playerCard);
                }
            }

            await this._dbContext.SaveChangesAsync();

            return Ok();
        }


        public class CardModel
        {
            public string InstanceID { get; set; }
            public MTGCard CardData { get; set; }
        }

        public class CollectionResponse
        {
            public int Page { get; set; }
            public int PageCount { get; set; }
            public CardModel[] Cards { get; set; }

            public CollectionResponse(int page, int pageCount, CardModel[] cards)
            {
                this.Page = page;
                this.PageCount = pageCount;
                this.Cards = cards;
            }
        }
    }
}

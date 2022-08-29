using Microsoft.AspNetCore.Mvc;
using MTGCC.Database;
using MTGCC.Translators;
using System.ComponentModel.DataAnnotations;

namespace MTGCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ScryfallAPITranslator _scryfallTranslator;

        public SearchController(ScryfallAPITranslator translator)
        {
            this._scryfallTranslator = translator;
        }

        [HttpPost]
        public async Task<ActionResult<SearchResponse>> Search([FromBody] SearchBody searchBody)
        {
            SearchResponse response = await _scryfallTranslator.Search(searchBody);

            if (response == null)
                return BadRequest();

            SearchResults results = new SearchResults(response.page, response.total_pages, response.AsMTGCards());

            return Ok(results);
        }

        public class SearchResults
        {
            public int Page { get; set; }
            public int PageCount { get; set; }
            public MTGCard[] Cards { get; set; }

            public SearchResults(int page, int pageCount, MTGCard[] cards)
            {
                this.Page = page;
                this.PageCount = pageCount;
                this.Cards = cards;
            }
        }
    }
}
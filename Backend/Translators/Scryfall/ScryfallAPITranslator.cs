using Microsoft.AspNetCore.Http.Extensions;
using MTGCC.Database;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MTGCC.Translators
{
    public class ScryfallAPITranslator
    {
        private const string BASE_URL = "https://api.scryfall.com";

        private const string CARD_SEARCH_URL = "/cards/search";

        private static HttpClient Client;

        private static int DelayedResponseCounter;
        private static ConcurrentQueue<DelayedRequest> DelayedRequests;
        private static ConcurrentDictionary<int, HttpResponseMessage> DelayedResponses;

        static ScryfallAPITranslator()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(BASE_URL);

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            DelayedRequests = new ConcurrentQueue<DelayedRequest>();
            DelayedResponses = new ConcurrentDictionary<int, HttpResponseMessage>();

            Task.Run(Loop);
        }

        private static async Task<HttpResponseMessage> AwaitDelayedRequest(HttpMethod method, string endpoint, int timeout, object body = null)
        {
            int key = Interlocked.Increment(ref DelayedResponseCounter);

            DelayedRequests.Enqueue(new DelayedRequest(key, method, endpoint, body));
           
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            bool timedout = false;

            await Task.WhenAny(Task.Run(Timeout, cancellationTokenSource.Token), Task.Run(Check, cancellationTokenSource.Token));

            if (!timedout && DelayedResponses.TryRemove(key, out HttpResponseMessage response))
                return response;
            else
                return null;

            async Task Timeout()
            {
                await Task.Delay(timeout);
                cancellationTokenSource.Cancel();
                timedout = true;
            }

            async Task Check()
            {
                while (!DelayedResponses.ContainsKey(key))
                    await Task.Delay(1);
                cancellationTokenSource.Cancel();
                timedout = false;
            }
        }

        private static async Task Loop()
        {
            while (true)
            {
                if (DelayedRequests.TryDequeue(out DelayedRequest delayedRequest))
                {
                    HttpResponseMessage message = await delayedRequest.GetResponse();
                    DelayedResponses.TryAdd(delayedRequest.Key, message);
                    await Task.Delay(100);
                }

                await Task.Delay(1);
            }
        }

        public async Task<SearchResponse> Search(SearchBody searchBody)
        {
            string queryString = searchBody.ToString();

            if (queryString == null)
                return new SearchResponse() { data = new Json.JsonCard[0], total_cards = 0 };

            string endpoint = $"{CARD_SEARCH_URL}{queryString}";
            HttpResponseMessage response = await AwaitDelayedRequest(HttpMethod.Get, endpoint, 10000);

            if (response == null)
                return null;
            else if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                SearchResponse sResponse = JsonConvert.DeserializeObject<SearchResponse>(json);
                sResponse.page = searchBody.Page;

                return sResponse;
            }
            else
            {
                return null;
            }
        }

        private class DelayedRequest
        {
            public int Key { get; private set; }
            private HttpMethod _method;
            private string _endpoint;
            private object _body;

            public DelayedRequest(int key, HttpMethod method, string endpoint, object body = null)
            {
                this.Key = key;
                this._method = method;
                this._endpoint = endpoint;
                this._body = body;
            }

            public async Task<HttpResponseMessage> GetResponse()
            {
                if (this._method == HttpMethod.Get)
                    return await Client.GetAsync(this._endpoint);
                else if (this._method == HttpMethod.Post)
                    return await Client.PostAsync(this._endpoint, JsonContent.Create(this._body));
                return null;
            }
        }
    }
}

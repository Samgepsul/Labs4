using App.RestApiClient.Model;

using Refit;


namespace App.RestApiClient
{

    public interface IRestClient
    {
        [Post("/word/search")]
        Task<WordSearchResponse> WordSearch(WordSearchRequest searchRequest);

        [Post("/word/create")]
        Task<WordCreateResponse> WordCreate(WordCreateRequest createRequest);

        [Get("/word/list")]
        Task<WordListResponse> WordList();

    }

}

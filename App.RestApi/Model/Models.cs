using App.Store.Model;

namespace App.RestApi.Model
{
    public record WordSearchRequest(string Root);
    public record WordCreateRequest(string Prefix, string Root, string Sufix, string Full);


    public record ApiResponse(int Status = 0, string? Detail = null);
    public record ApiResponse<T>(T Data) : ApiResponse;
    public record WordCreateResponse() : ApiResponse;
    public record WordSearchResponse(List<WordData> wordList) : ApiResponse<IEnumerable<WordData>>(wordList);
    public record WordListResponse(List<WordData> wordList) : ApiResponse<IEnumerable<WordData>>(wordList);

}

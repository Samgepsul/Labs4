namespace App.RestApiClient.Model
{
    public record Word(string Prefix, string Root, string Sufix, string Full);

    public record WordSearchResponse : ApiResponse<IEnumerable<Word>>;

    public record WordCreateResponse() : ApiResponse;

    public record WordListResponse : ApiResponse<IEnumerable<Word>>;

}

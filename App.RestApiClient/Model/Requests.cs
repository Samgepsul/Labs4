namespace App.RestApiClient.Model
{
    public record WordSearchRequest(string Root);
    public record WordCreateRequest(string Prefix, string Root, string Sufix, string Full);
}

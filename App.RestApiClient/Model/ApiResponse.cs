namespace App.RestApiClient.Model
{
    public record ApiResponse(int Status = 0, string? Detail = null);

    public record ApiResponse<T>(T? Data = default) : ApiResponse;
}

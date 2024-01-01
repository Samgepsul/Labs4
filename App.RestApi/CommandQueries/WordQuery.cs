using System.Security.Claims;

using App.RestApi.Model;
using App.Store;
using App.Store.Model;

using MediatR;

using Microsoft.AspNetCore.Identity;


namespace App.RestApi.CommandQueries
{
    public record WordListQuery() : IRequest<List<WordData>>;
    public record WordSearchQuery(string Root) : IRequest<List<WordData>>;

    public class WordQueryHandler :
        IRequestHandler<WordListQuery, List<WordData>>,
        IRequestHandler<WordSearchQuery, List<WordData>>
    {

        public async Task<List<WordData>> Handle(WordListQuery request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return new ChangeDB().GetWords();
        }

        public async Task<List<WordData>> Handle(WordSearchQuery request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var wordList = new ChangeDB().GetWords();
            return wordList.Where(w => request.Root.Contains(w.Root, StringComparison.OrdinalIgnoreCase)).ToList();

        }
    }
}

using App.Store;

using MediatR;

namespace App.RestApi.CommandQueries
{
    public record WordCreateCommand(string Prefix, string Root, string Sufix, string Full) : IRequest;

    public class WordCommandHandler : IRequestHandler<WordCreateCommand>
    {
        public async Task Handle(WordCreateCommand request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var db = new ChangeDB();
            var wordList = db.GetWords();
            var exists = wordList.Exists(i => i.Full.ToLower() == request.Full.ToLower());
            if (exists) throw new ArgumentException("Данное слово уже внесено в словарь");
            db.Add(new Store.Model.WordData { Prefix = request.Prefix, Root = request.Root, Suffix = request.Sufix, Full = request.Full });
        }
    }
}

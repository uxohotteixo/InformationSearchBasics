using System.Collections.Generic;
using System.Linq;

namespace InformationSearchBasics.Index
{
    internal class DocumentsProvider
    {
        private readonly IEnumerable<string> _binaryOperators = new[] { "&", "|" };

        private readonly Dictionary<string, IEnumerable<string>> _index;

        public DocumentsProvider(Dictionary<string, IEnumerable<string>> index)
        {
            _index = index;
        }

        public IEnumerable<string> Provide(string expression)
        {
            var splitted = expression
                .Split(' ');

            var binaryOperators = splitted.Where(e => _binaryOperators.Contains(e));

            var docs = GetDocsByToken(splitted.First());

            foreach (var part in binaryOperators)
            {
                var nextIdx = splitted.ToList().FindIndex(s => s == part) + 1;
                var nextTokenDocs = GetDocsByToken(splitted[nextIdx]);

                switch (part)
                {
                    case "&":
                        docs = IntersectDocs(docs, nextTokenDocs);
                        break;
                    case "|":
                        docs = UnionDocs(docs, nextTokenDocs);
                        break;
                }

            }

            return docs;
        }

        private IEnumerable<string> GetDocsByToken(string token)
        {
            var hasLogicalNot = token.StartsWith("!");
            var actualToken = hasLogicalNot ? token.Substring(1, token.Length - 1) : token;

            var hasToken = _index.TryGetValue(actualToken, out var savedDocs);

            return hasToken ? hasLogicalNot ? ExceptDocs(savedDocs) : savedDocs 
                : Enumerable.Empty<string>();
        }

        private IEnumerable<string> IntersectDocs(IEnumerable<string> docs1, IEnumerable<string> docs2)
            => docs1.Intersect(docs2);

        private IEnumerable<string> UnionDocs(IEnumerable<string> docs1, IEnumerable<string> docs2)
            => docs1.Union(docs2);

        private IEnumerable<string> ExceptDocs(IEnumerable<string> docs)
            => _index.Values.SelectMany(v => v).Except(docs);
    }
}

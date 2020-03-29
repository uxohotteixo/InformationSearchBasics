using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure
{
    public class IndexBuilder
    {
        private readonly string _documentsPath;

        private readonly Dictionary<string, IEnumerable<string>> _index = new Dictionary<string, IEnumerable<string>>();

        public Dictionary<string, IEnumerable<string>> Index
        {
            get
            {
                if (!_isBuilt) throw new InvalidOperationException("Index was not builded");
                return _index;
            }
        }

        private bool _isBuilt;

        public IndexBuilder(string documentsPath)
        {
            _documentsPath = documentsPath;
        }

        public IndexBuilder Build()
        {
            foreach (var file in Directory.EnumerateFiles(_documentsPath, "*.txt"))
            {
                var tokens = System.IO.File.ReadAllText(file)
                    .Split(' ')
                    .Distinct()
                    .ToList();

                IndexFile(tokens, file.Replace(_documentsPath + "\\", string.Empty));
            }

            _isBuilt = true;
            return this;
        }

        private void IndexFile(IEnumerable<string> tokens, string documentName)
        {
            foreach (var token in tokens)
            {
                if (!_index.ContainsKey(token))
                {
                    _index.Add(token, new[] { documentName });
                    continue;
                }

                _index[token] = _index[token].Append(documentName);
            }
        }
    }
}

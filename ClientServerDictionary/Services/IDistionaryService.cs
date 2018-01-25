using System.Collections.Generic;

namespace Services
{
    public interface IDistionaryService
    {
        int AddTranslation(int termId, string translation);
        string[] Get(string word);
        void Add(string word, List<string> translations);
        void Delete(string word, List<string> values);
    }
}

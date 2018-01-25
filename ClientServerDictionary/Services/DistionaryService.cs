using Data;
using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class DistionaryService
    {
        private readonly DictionaryContext _context;
        public DistionaryService()
        {
            _context = new DictionaryContext();
        }

        private int AddNewTerm(string word)
        {
            var term = _context.Terms.
                               FirstOrDefault(t => t.Name.Trim().ToLower().Equals(word.Trim().ToLower()));
            if (term == null)
            {
                term = new Term()
                {
                    Name = word.Trim().ToLower()
                };
                _context.Terms.Add(term);
                _context.SaveChanges();

            }
            return term.TermId;
        }

        private int AddTranslation(int termId, string translation)
        {
            var lowered = translation.Trim().ToLower();
            var trans = _context.Translations
                                .FirstOrDefault(x => x.TermId == termId && x.Text.Trim().ToLower().Equals(lowered));
            if (trans != null) return trans.TranslationId;
            trans = new Translation()
            {
                TermId = termId,
                Text = lowered
            };
            _context.Translations.Add(trans);
            _context.SaveChanges();
            return trans.TranslationId;
        }

        public string[] Get(string word)
        {
            var term = _context.Terms.FirstOrDefault(t => t.Name.Trim().ToLower().Equals(word.Trim().ToLower()));
            if (term == null) return new string[0];
            var trans = _context.Translations.
                    Where(x => x.TermId == term.TermId).Select(x => x.Text).ToArray();
            return trans;
        }

        public void Add(string word, List<string> translations)
        {
            var id = AddNewTerm(word);
            foreach (var t in translations)
            {
                AddTranslation(id, t);
            }
        }

        public void Delete(string word, List<string> values)
        {
            var term = _context.Terms.FirstOrDefault(t => t.Name.Trim().ToLower().Equals(word.Trim().ToLower()));
            if (term == null) return;
            foreach (var val in values)
            {
                var trans = _context.Translations.
                     FirstOrDefault(x => x.TermId == term.TermId && x.Text.ToLower().Trim().Equals(val.ToLower().Trim()));
                if (trans == null) continue;
                _context.Translations.Remove(trans);
            }
            _context.SaveChanges();
        }
    }
}

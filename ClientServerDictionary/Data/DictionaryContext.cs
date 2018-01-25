using Data.Models;
using System.Data.Entity;

namespace Data
{
    public class DictionaryContext : DbContext
    {

        public DictionaryContext():base("DefaultConnection")
        {

        }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Translation> Translations { get; set; }
    }
}

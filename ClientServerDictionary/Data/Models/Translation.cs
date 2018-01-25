using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Translation
    {
        [Key]
        public int TranslationId { get; set; }
        public int TermId { get; set; }
        public string Text { get; set; }

        [ForeignKey("TermId")]
        public Term Term { get; set; }
    }
}

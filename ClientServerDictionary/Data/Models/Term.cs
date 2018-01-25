using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("Terms")]
    public class Term
    {
        [Key]
        public int TermId { get; set; }
        public string Name { get; set; }

        public virtual List<Translation> Translations { get; set; }
    }
}

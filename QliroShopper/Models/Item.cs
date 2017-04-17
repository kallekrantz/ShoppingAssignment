using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace QliroShopper.Models {
    public class Item {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Description { get; set;}
        // Well Damn. Range doesn't support decimal. Probably not relevant.
        [Required]
        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Quantity {get; set;}

        [IgnoreDataMember]
        public virtual Order Order { get; set; }
    }
}
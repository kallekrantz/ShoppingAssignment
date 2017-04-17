using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QliroShopper.Models {
    public class Order {
        public Order()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual IList<Item> OrderItems { get; set; }

        [NotMappedAttribute]
        public decimal TotalPrice 
        { 
            get {
                if (OrderItems == null)
                {
                    return 0;
                }
                return OrderItems.Sum(i => i.Price * i.Quantity);
            }
        }
    }
}
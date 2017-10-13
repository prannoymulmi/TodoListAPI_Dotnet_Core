using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListsWebAPi.Models
{
    public class ListItem
    {
        //Auto generated Id using data annatations
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public Guid ItemId { get; set; }
        
        public bool ItemChecked { get; set; }
        [Required]
        public String ItemName { get; set; }
        
        public int Quantity { get; set; }

        public ListItem()
        {
            Quantity = 1;
            ItemChecked = false;
        }
        
    }
}
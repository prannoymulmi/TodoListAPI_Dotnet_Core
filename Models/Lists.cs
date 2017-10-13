using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListsWebAPi.Models
{
    public class Lists
    {
        //Auto generated Id using data annatations
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public Guid ListId { get; set; }
        public List<ListItem> Items { get; set; }
        
        [Required]
        public string ListName { get; set; }
        
        [ForeignKey("AspNetUserId")]
        public ApplicationUser User { get; set; }
        public string AspNetUserId { get; set; }
        
        
    }
}
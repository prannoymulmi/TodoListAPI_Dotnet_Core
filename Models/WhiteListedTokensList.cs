using System;
using System.ComponentModel.DataAnnotations;

namespace ListsWebAPi.Models
{
    public class WhiteListedTokensList
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Token { get; set; }
        
        public long TimestampCreated { get; set; } 
        
        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListsWebAPi.Models
{
    public class UserJwtInfo
    {
        
        [Required]
        [Key]
        public Guid Id { get; set; }
        
        
        public String Issuer { get; set; }
        
        
        public String Audience { get; set; }
        
        [Required]
        public Guid UserSecurityKey { get; set; }
        
        [ForeignKey("AspNetUserId")]
        public ApplicationUser User { get; set; }
        public string AspNetUserId { get; set; }
    }
}
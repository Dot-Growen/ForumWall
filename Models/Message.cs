using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add if your using Relationships
using System;

namespace TheWall.Models {
    public class Message {

        [Key]
        public int MessageId { get; set; }

        [Required (ErrorMessage = "Enter your Message")]
        [MinLength (10, ErrorMessage = "Message must be at least 10 characters")]
        public string MsgContent { get; set; }

        public int UserId { get; set; }
        public User MsgCreator { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
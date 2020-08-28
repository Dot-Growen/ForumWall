using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add if your using Relationships
using System;

namespace TheWall.Models {
    public class Comment {

        [Key]
        public int CommentId { get; set; }

        [Required (ErrorMessage = "Enter your Comment")]
        [MinLength (10, ErrorMessage = "Comment must be at least 10 characters")]
        public string CmtContent { get; set; }

        public int UserId { get; set; }

        public User CmtCreator { get; set; }

        public int MessageId { get; set; }

        public Message MsgCmtIsUnder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
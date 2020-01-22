using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public string CommentContent { get; set; }
        public virtual Items Items { get; set; }
        public virtual User User { get; set; }
    }
}

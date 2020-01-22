using Shop.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.HomeViewModels
{
    public class CommentViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "{0} phải có ít nhất {2} kí tự và tối đa {1} kí tự.", MinimumLength = 10)]
        public string CommentContent { get; set; }
        public Items Items;
        public User user;
    }
}

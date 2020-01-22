using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    public class Promotion
    {
        [Key]
        public int ID { get; set; }

        public int ItemsId { get; set; }
        public Items Items { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Discount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    public class Coupons
    {
        [Key]
        public int ID { get; set; }
        public string Coupon { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double Discount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    public class ReturnGood
    {
        [Key]
        public int ID { get; set; }
        public DateTime DateReturn { get; set; }
        public int MyProperty { get; set; }
    }
}

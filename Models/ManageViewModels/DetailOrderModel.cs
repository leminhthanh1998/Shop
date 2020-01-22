using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.ManageViewModels
{
    public class DetailOrderModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Slug { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.HomeViewModels
{
    public class VoteViewModel
    {
        public int RatingCount { get; set; }
        public double Rating1Count { get; set; }
        public double Rating2Count { get; set; }
        public double Rating3Count { get; set; }
        public double Rating4Count { get; set; }
        public double Rating5Count { get; set; }
    }
}

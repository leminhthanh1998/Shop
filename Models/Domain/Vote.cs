using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Domain
{
    public class Vote
    {
        public int VoteID { get; set; }
        public int Value { get; set; }
        public virtual Items Items { get; set; }
        public virtual User User { get; set; }
    }
}

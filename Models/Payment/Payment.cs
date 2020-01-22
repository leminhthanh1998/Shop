using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Payment
{
    public class Payment
    {
        public Payment()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

        public long OrderId { get; set; }


        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LatestUpdatedOn { get; set; }

        public decimal Amount { get; set; }

        public decimal PaymentFee { get; set; }

        [StringLength(450)]
        public string PaymentMethod { get; set; }

        [StringLength(450)]
        public string GatewayTransactionId { get; set; }

        public PaymentStatus Status { get; set; }

        public string FailureMessage { get; set; }
    }

    public enum PaymentStatus
    {
        Succeeded = 1,

        Failed = 5
    }
}

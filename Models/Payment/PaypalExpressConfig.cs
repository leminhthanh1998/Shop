using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Payment
{
    public class PaypalExpressConfig
    {
        public static bool IsSandbox { get; set; } = true;

        public static string ClientId { get; set; } = "AU7M2YhhDrqbzLrEukconnjAeRzeZzYmxncj4d3CgSbBpqdbKjlFBNOAWWq1SDD-gE6aQCYmbZo7uRZR";

        public static string ClientSecret { get; set; } = "EDDFIHNf4wyeNCEIKZeIHA84YTV3Rn-UbPJQapwKxD03yEKgQRPp0PznOvMVXK2O2CGr_UNZR4hG-f6i";

        public static decimal PaymentFee { get; set; } = 1;//1 % phi thanh toan

        public static string Environment
        {
            get
            {
                return IsSandbox ? "sandbox" : "production";
            }
        }

        public static string EnvironmentUrlPart
        {
            get
            {
                return IsSandbox ? ".sandbox" : "";
            }
        }
    }
}

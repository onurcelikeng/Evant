using System;
using System.Collections.Generic;
using System.Text;

namespace Evant.Pay.Models
{
    public class PaymentModel
    {
        public string CardHolderName { get; set; } = "Elif Seray Dönmez";

        public string CardNumber { get; set; } = "5170410000000004";

        public string ExpireMonth { get; set; } = "09";

        public string ExpireYear { get; set; } = "2020";

        public string Cvc { get; set; } = "797";

        public string FirstName { get; set; } = "Elif Seray";

        public string LastName { get; set; } = "Dönmez";

        public string Email { get; set; } = "elifseraydonmez@gmail.com";

        public string Phone { get; set; } = "05074406251";

        public string ProductName { get; set; } = "Gold Business Üyelik";

        public string Price { get; set; } = "70";
    }
}

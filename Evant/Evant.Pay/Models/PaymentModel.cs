
namespace Evant.Pay.Models
{
    public class PaymentModel
    {
        public string CardHolderName { get; set; }

        public string CardNumber { get; set; }

        public string ExpireMonth { get; set; }

        public string ExpireYear { get; set; }

        public string Cvc { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ProductName { get; set; }

        public string Price { get; set; }
    }
}


using Evant.Pay.Models;

namespace Evant.Contracts.DataTransferObjects.Business
{
    public class BusinessDTO
    {
        public string BusinessType { get; set; }

        public PaymentModel Payment { get; set; }
    }
}

using Armut.Iyzipay;
using Armut.Iyzipay.Model;
using Armut.Iyzipay.Request;
using Evant.Pay.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.Pay
{
    public class Iyzico
    {
        private Options options;


        public Iyzico()
        {
            options = new Options
            {
                ApiKey = "sandbox-dyxzdJJtion1K2CEW64RuxsiCcElGjmW",
                SecretKey = "sandbox-wEXkIUxcxujmmG3HxRSKnjfWoizlegXO",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };
        }


        public async Task<bool> Operation(PaymentModel model)
        {
            try
            {
                var request = new CreatePaymentRequest
                {
                    Price = model.Price,
                    PaidPrice = model.Price,
                    Currency = Currency.TRY.ToString(),
                    Installment = 1,
                    PaymentCard = new PaymentCard
                    {
                        CardHolderName = model.CardHolderName,
                        CardNumber = model.CardNumber,
                        ExpireMonth = model.ExpireMonth,
                        ExpireYear = model.ExpireYear,
                        Cvc = model.Cvc
                    },
                    Buyer = new Buyer
                    {
                        Id = "BY789",
                        Name = "Onur",
                        Surname = "Celik",
                        GsmNumber = "+905074406251",
                        Email = "onrcelik@outlook.com",
                        IdentityNumber = "74300864791",
                        LastLoginDate = "2015-10-05 12:43:35",
                        RegistrationDate = "2013-04-21 15:12:09",
                        RegistrationAddress = "Kuruçeşme Mahallesi No:196 Buca",
                        Ip = "85.34.78.112",
                        City = "Izmir",
                        Country = "Turkey"
                    },
                    ShippingAddress = new Address
                    {
                        ContactName = model.CardHolderName,
                        City = "Izmir",
                        Country = "Turkey",
                        Description = model.ProductName + " - Online Alışveriş"
                    },
                    BillingAddress = new Address
                    {
                        ContactName = model.CardHolderName,
                        City = "Izmir",
                        Country = "Turkey",
                        Description = "Kuruçeşme Mahallesi No:196 Buca",
                    },
                    BasketItems = new List<BasketItem>
                    {
                        new BasketItem
                        {
                            Id = "BI101",
                            Name = model.ProductName,
                            Category1 = "Collectibles",
                            ItemType = BasketItemType.PHYSICAL.ToString(),
                            Price = model.Price
                        }
                    }
                };

                Payment payment = await Payment.CreateAsync(request, options);
                if (payment.Status == "success")
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

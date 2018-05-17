using Armut.Iyzipay;
using Armut.Iyzipay.Model;
using Armut.Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.Pay
{
    public class Iyzico
    {
        public Iyzico()
        {
        }


        public async void test()
        {
            var options = new Options
            {
                ApiKey = "sandbox-dyxzdJJtion1K2CEW64RuxsiCcElGjmW",
                SecretKey = "sandbox-wEXkIUxcxujmmG3HxRSKnjfWoizlegXO",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = "123456789",
                Price = "1",
                PaidPrice = "1.2",
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = "B67832",
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString()
            };

            var paymentCard = new PaymentCard
            {
                CardHolderName = "John Doe",
                CardNumber = "5528790000000008",
                ExpireMonth = "12",
                ExpireYear = "2030",
                Cvc = "123",
                RegisterCard = 0
            };

            request.PaymentCard = paymentCard;

            var buyer = new Buyer
            {
                Id = "BY789",
                Name = "John",
                Surname = "Doe",
                GsmNumber = "+905350000000",
                Email = "email@email.com",
                IdentityNumber = "74300864791",
                LastLoginDate = "2015-10-05 12:43:35",
                RegistrationDate = "2013-04-21 15:12:09",
                RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34732"
            };

            request.Buyer = buyer;

            var shippingAddress = new Address
            {
                ContactName = "Jane Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };

            request.ShippingAddress = shippingAddress;

            var billingAddress = new Address
            {
                ContactName = "Jane Doe",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };

            request.BillingAddress = billingAddress;

            var firstBasketItem = new BasketItem
            {
                Id = "BI101",
                Name = "Binocular",
                Category1 = "Collectibles",
                Category2 = "Accessories",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = "0.3"
            };

            var secondBasketItem = new BasketItem
            {
                Id = "BI102",
                Name = "Game code",
                Category1 = "Game",
                Category2 = "Online Game Items",
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Price = "0.5"
            };

            var thirdBasketItem = new BasketItem
            {
                Id = "BI103",
                Name = "Usb",
                Category1 = "Electronics",
                Category2 = "Usb / Cable",
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = "0.2"
            };

            var basketItems = new List<BasketItem>
            {
                firstBasketItem,
                secondBasketItem,
                thirdBasketItem
            };

            request.BasketItems = basketItems;

            Payment payment = await Payment.CreateAsync(request, options);

        }

    }
}

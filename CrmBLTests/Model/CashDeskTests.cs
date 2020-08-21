using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrmBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBL.Model.Tests {
    [TestClass()]
    public class CashDeskTests {
        [TestMethod()]
        public void CashDeskTest() {
            //Arrange
            var customer1 = new Customer {
                Name = "testuser1",
                CustomerId = 1,
            };

            var customer2 = new Customer {
                Name = "testuser2",
                CustomerId = 2
            };

            var seller = new Seller() {
                Name = "testseller",
                SellerId = 1
            };

            var product1 = new Product() {
                ProductId = 1,
                Name = "pr1",
                Price = 100,
                Count = 100
            };

            var product2 = new Product() {
                ProductId = 2,
                Name = "pr2",
                Price = 200,
                Count = 200
            };

            var cart1 = new Cart(customer1);
            cart1.Add(product1);
            cart1.Add(product1);
            cart1.Add(product2);

            var cart2 = new Cart(customer2);
            cart2.Add(product1);
            cart2.Add(product2);
            cart2.Add(product2);

            var cashDesk = new CashDesk(1, seller, null);
            cashDesk.MaxQueueLength = 10;
            cashDesk.Enqueue(cart1);
            cashDesk.Enqueue(cart2);

            var ExpectedResult1 = 400;
            var ExpectedResult2 = 500;

            //Act
            var cart1ActualResult = cashDesk.Dequeue();
            var cart2ActualResult = cashDesk.Dequeue();

            //Assert
            Assert.AreEqual(ExpectedResult1, cart1ActualResult);
            Assert.AreEqual(ExpectedResult2, cart2ActualResult);
            Assert.AreEqual(97, product1.Count);
            Assert.AreEqual(197, product2.Count);
        }
    }
}
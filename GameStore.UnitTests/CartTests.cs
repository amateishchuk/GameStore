using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_To_Basket()
        {
            Game game1 = new Game { GameId = 1 };
            Game game2 = new Game { GameId = 2 };
            Cart cart = new Cart();

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 2);
            List<CartLine> results = cart.Lines.ToList();

            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].Game == game1);
            Assert.IsTrue(results[1].Game == game2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Exisiting_Lines()
        {
            Game game1 = new Game { GameId = 1 };
            Game game2 = new Game { GameId = 2 };
            Game game3 = new Game { GameId = 3 };
            Game game4 = new Game { GameId = 4 };
            Cart cart = new Cart();

            cart.AddItem(game1, 2);
            cart.AddItem(game3, 1);
            cart.AddItem(game4, 2);
            cart.AddItem(game1, 5);

            List<CartLine> results = cart.Lines.ToList();



            Assert.AreEqual(results.Count, 3);
            Assert.AreEqual(results[0].Quantity, 7);
            Assert.AreEqual(results[1].Quantity, 1);
            Assert.AreEqual(results[2].Game, game4);
        }

        [TestMethod]
        public void Can_Remove_From_Line()
        {
            Game game1 = new Game { GameId = 1 };
            Game game2 = new Game { GameId = 2 };
            Cart cart = new Cart();

            cart.AddItem(game1, 5);
            cart.AddItem(game2, 2);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 3);

            cart.RemoveGame(game2);

            List<CartLine> results = cart.Lines.ToList();


            Assert.AreEqual(results.Where(g => g.Game == game2).Count(), 0);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results.Where(g => g.Game == game1).FirstOrDefault().Quantity, 8);
        }

        [TestMethod]
        public void Can_Calculate_Cart_Total()
        {
            Game game1 = new Game { GameId = 1, Price = 100 };
            Game game2 = new Game { GameId = 2, Price = 55 };
            Cart cart = new Cart();

            cart.AddItem(game1, 2);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 4);

            decimal results = cart.ComputeTotalValue();

            Assert.AreEqual(results, 655);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Can_Clear_Line_Cart()
        {
            Game game1 = new Game { GameId = 1 };
            Game game2 = new Game { GameId = 2 };
            Game game3 = new Game { GameId = 3 };
            Cart cart = new Cart();

            cart.AddItem(game1, 2);
            cart.AddItem(game2, 5);
            cart.AddItem(game3, 1);
            cart.AddItem(game2, 2);

            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);
        }
    }

    
}

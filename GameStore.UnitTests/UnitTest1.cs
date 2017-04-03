using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.WebUI.Models;
using GameStore.WebUI.HtmlHelpers;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
         
        public Mock<IGameRepository> getGameMock()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new[] {
                new Game { GameId = 1, Name = "Game 1", Price = 10, Category = "Cat1" },
                new Game { GameId = 2, Name = "Game 2", Price = 10, Category = "Cat2" },
                new Game { GameId = 3, Name = "Game 3", Price = 10, Category = "Cat1" },
                new Game { GameId = 4, Name = "Game 4", Price = 10, Category = "Cat2" },
                new Game { GameId = 5, Name = "Game 5", Price = 10, Category = "Cat3" }
            });
            return mock;
        }

        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IGameRepository> mock = getGameMock();

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;
            
            List<Game> games = result.Games.ToList();

            Assert.IsTrue(games.Count == 2);
            Assert.IsTrue(games[0].Name == "Game 4");
        }


        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItems = 28
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IGameRepository> mock = getGameMock();

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(2, pagingInfo.CurrentPage);
            Assert.AreEqual(3, pagingInfo.ItemsPerPage);
            Assert.AreEqual(5, pagingInfo.TotalItems);
            Assert.AreEqual(2, pagingInfo.TotalPages);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            Mock<IGameRepository> mock = getGameMock();
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            List<Game> result = ((GamesListViewModel)(controller.List("Cat2", 1).Model)).Games.ToList();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result[0].Name == "Game 2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Game 4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IGameRepository> mock = getGameMock();
            NavController controller = new NavController(mock.Object);
            List<string> result = ((IEnumerable<string>)controller.Menu().Model).ToList();

            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IGameRepository> mock = getGameMock();
            NavController controller = new NavController(mock.Object);
            string category = "Cat1";

            string result = controller.Menu(category).ViewBag.SelectedCategory;

            Assert.AreEqual(category, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            Mock<IGameRepository> mock = getGameMock();
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

    }
}

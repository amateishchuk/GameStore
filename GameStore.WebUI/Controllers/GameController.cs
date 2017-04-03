using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.WebUI.Models;

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository repository;

        public int pageSize = 4;

        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int page = 1)
        {
            GamesListViewModel viewModel = new GamesListViewModel
            {
                Games = repository.Games
                    .OrderBy(g => g.GameId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Games.Count()
                }
            };
            return View(viewModel);
        }
    }
}
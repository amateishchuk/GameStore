using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        IGameRepository repository;

        public NavController(IGameRepository repo)
        {
            repository = repo;
        }


        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = repository.Games.Select(g => g.Category).Distinct().OrderBy(c => c);
            return PartialView(categories);
        }
    }
}
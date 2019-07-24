using DiscussionForum.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DiscussionForum.Controllers
{
    public class AnimeController : Controller
    {
        // GET: Anime
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Anime(int Id)
        {
            AnimeViewModel model = new AnimeViewModel();
            await model.Media.GetAnime(Id);

            return View(model.Media);
        }
    }
}
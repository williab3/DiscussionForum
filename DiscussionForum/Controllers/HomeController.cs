using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DiscussionForum.Models;
using DiscussionForum.Models.ViewModel;
using Microsoft.AspNet.Identity;

namespace DiscussionForum.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HomePage_UA model = new HomePage_UA();

                return View(model);
            }
            else
            {
                string currentUser = User.Identity.GetUserId();
                ProfilePageViewModel model = new ProfilePageViewModel(currentUser);
                return View("ProfilePage", model);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _LoginPartial()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser currentUser = new ApplicationUser(userId);
                return PartialView(currentUser);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult FavoriteCheck(int animeId)
        {
            string result = ApplicationUser.ToggleFavoriteAnime(animeId, User.Identity.GetUserId());


            return Json(result);
        }

        [HttpPost]
        public JsonResult Tags()
        {
            string testResult = "Yeah!! It worked";
            return Json(testResult);
        }

    }
}
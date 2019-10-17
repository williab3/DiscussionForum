using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
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
        [ChildActionOnly]
        public PartialViewResult _LoginModalPartial()
        {
            return PartialView();
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
            List<Tag> tags = Tag.GetTags();

            return Json(tags);
        }

        [HttpPost]
        public ActionResult AddNewDiscussion(ProfilePageViewModel viewModel, HttpPostedFileBase ImageFile)
        {
            string currentUser = User.Identity.GetUserId();
            ProfilePageViewModel model = new ProfilePageViewModel(currentUser);
            viewModel.FreshDiscussion.CreateNewDiscussion(ImageFile, currentUser);
            return View("ProfilePage", model);
        }

        [HttpPost]
        public ActionResult CropImage(ImageCropVM cropVM, HttpPostedFileBase ImageFile)
        {
            cropVM.ImageFile = ImageFile;
            Bitmap finalImage = cropVM.CropAndSaveImage();
            return Json(finalImage);
        }
    }
}
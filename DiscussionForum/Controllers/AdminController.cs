using DiscussionForum.Models;
using DiscussionForum.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DiscussionForum.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ImportAnime()
        {
            MainAdminViewModel viewModel = new MainAdminViewModel();
            viewModel = await viewModel.ImportNewAnimeData();
            return View("Index", viewModel);
        }

        public async Task<ActionResult> SeedPopularity()
        {
            MainAdminViewModel model = new MainAdminViewModel();
            await model.SeedAnimePopularity();

            return View("Index", model);
        }
        [RequireAttributeValue("anilistID")]
        public async Task<PartialViewResult> SeedPopularity(int anilistID)
        {
            MainAdminViewModel model = new MainAdminViewModel();
            await model.SeedAnimePopularity();

            return PartialView(model);
        }

        public async Task<PartialViewResult> NewsReport()
        {
            MainAdminViewModel viewModel = new MainAdminViewModel();
            await viewModel.RefreshNewsReport();
            return PartialView(viewModel);
        }

        public async Task<PartialViewResult> ImportUpdates()
        {
            AnimeStatsModel animeUpdatesModel = new AnimeStatsModel();
            List<string> addedUpdateTitles = await animeUpdatesModel.ImportAnimeUpdates();
            return PartialView(addedUpdateTitles);
        }

        [HttpPost]
        public async Task<JsonResult> SaveNewAnime(AnimeModel[] checkedAnime)
        {
            MainAdminViewModel saveResults = new MainAdminViewModel();
            saveResults.AnimeModels = checkedAnime.ToList();
            await saveResults.SaveAnimeToDB();
            saveResults.AnimeModels = null;
            string json = JsonConvert.SerializeObject(saveResults);

            return Json(json);
        }

        [HttpPost]
        public async Task<PartialViewResult> MassImport(AnilistVariables requestVariable)
        {
            MainAdminViewModel viewModel = await requestVariable.MassImport();
            viewModel.RequestVariables = requestVariable;
            return PartialView("MassImport", viewModel);
        }


        public PartialViewResult SeedTags()
        {
            MainAdminViewModel viewModel = new MainAdminViewModel();
            viewModel.FreshTags = Tag.SeedTags();
            return PartialView(viewModel);
        }
    }


    public class RequireAttributeValue : ActionMethodSelectorAttribute
    {
        public string ValueName { get; set; }
        public RequireAttributeValue(string valueName)
        {
            ValueName = valueName;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[ValueName] != null;
        }
    }
}
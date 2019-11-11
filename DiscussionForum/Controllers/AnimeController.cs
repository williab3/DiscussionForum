using DiscussionForum.Models;
using DiscussionForum.Models.ViewModel;
using Microsoft.AspNet.Identity;
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
            await model.GetAnime(Id);
            model.IsUserAuthenticated = User.Identity.IsAuthenticated;
            

            return View(model);
        }

        [HttpPost]
        public JsonResult PostAnimeComment(AnimeViewModel comment)
        {

            comment.PostedComment.CommenterId = User.Identity.GetUserId();
            comment.PostAnimeComment();
            return Json(comment.PostedComment);
        }

        [HttpPost]
        public JsonResult CommentReply(AnimeViewModel reply)
        {
            reply.PostedComment.CommenterId = User.Identity.GetUserId();
            reply.PostCommentReply();
            return Json(reply.PostedComment);
        }

        [HttpPost]
        public JsonResult Vote(VoteCast userVote)
        {
            if (User.Identity.IsAuthenticated)
            {
                userVote.CastedVote.VoterUserId = User.Identity.GetUserId();
                userVote.CastVote();
            }
            return Json(userVote);
        }
    }
}
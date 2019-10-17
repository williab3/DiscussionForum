using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiscussionForum.Models.ViewModel
{
    public class AnimeViewModel
    {
        public int AnimeId { get; set; }
        public int RepliedCommentId { get; set; }
        public Comment PostedComment { get; set; }
        public AnimeModel Anime { get; set; }
        public bool HasError { get; set; }
        public bool IsUserAuthenticated { get; set; }
        public AnimeViewModel()
        {
        }

        public async Task GetAnime(int animeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            await Task.Run(() =>
            {
                try
                {
                    Anime = dbContext.Animes.Include("Discussion.Comments.Replies").Where(a => a.Id == animeId).SingleOrDefault();
                }
                catch (Exception err)
                {

                    throw;
                }
            });
        }
        
        public void PostAnimeComment()
        {
            try
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                AnimeModel dbAnime = dbContext.Animes.Include("Discussion.Comments").Where(a => a.Id == AnimeId).SingleOrDefault();
                ApplicationUser user = dbContext.Users.Where(u => u.Id == PostedComment.CommenterId).SingleOrDefault();
                
                PostedComment.Who = user.LoginName;
                PostedComment.LastUpdated = DateTime.Now;
                dbAnime.Discussion.Comments.Add(PostedComment);
                dbContext.SaveChanges();

            }
            catch (Exception err)
            {

                HasError = true;
            }
        }

        public void PostCommentReply()
        {
            try
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                if (!String.IsNullOrEmpty(PostedComment.What))
                {
                    ApplicationUser user = dbContext.Users.Where(u => u.Id == PostedComment.CommenterId).SingleOrDefault();
                    Comment repliedComment = dbContext.Comments.Include("Replies").Where(c => c.Id == RepliedCommentId).SingleOrDefault();
                    PostedComment.RepliedTo = repliedComment.CommenterId;
                    PostedComment.LastUpdated = DateTime.Now;
                    PostedComment.Who = user.LoginName;
                    repliedComment.Replies.Add(PostedComment);
                    dbContext.SaveChanges();
                    user = dbContext.Users.Where(u => u.Id == PostedComment.RepliedTo).SingleOrDefault();
                    PostedComment.RepliedTo = user.LoginName;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }
    }
}
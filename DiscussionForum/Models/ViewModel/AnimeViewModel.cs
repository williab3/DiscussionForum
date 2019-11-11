using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Data;
using System.Data.Entity.Validation;

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
                    //Anime = dbContext.Animes.Include("Discussion.Comments.Replies.Votes").Where(a => a.Id == animeId).SingleOrDefault();

                    Anime = dbContext.Animes.Include(a => a.Discussion.Comments.Select(c => c.Replies.Select(v => v.Votes))).Include(a => a.Discussion.Comments.Select(cv => cv.Votes)) 
                        .Include(a => a.Discussion.Comments.Select(c => c.Replies.Select(r1 => r1.Replies.Select(v => v.Votes))))
                        .Include(a => a.Discussion.Comments.Select(c => c.Replies.Select(r1 => r1.Replies.Select(r2 => r2.Replies.Select(v => v.Votes)))))
                        .Include(a => a.Discussion.Comments.Select(c => c.Replies.Select(r1 => r1.Replies.Select(r2 => r2.Replies.Select(r3 => r3.Replies.Select(v => v.Votes))))))
                        .Include(a => a.Discussion.Comments.Select(c => c.Replies.Select(r1 => r1.Replies.Select(r2 => r2.Replies.Select(r3 => r3.Replies.Select(r4 => r4.Replies.Select(v => v.Votes))))))).Where(a => a.Id == animeId).SingleOrDefault();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
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
                Console.WriteLine(err.Message);
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

    public class VoteCast
    {
        public Comment VotedComment { get; set; }
        public Vote CastedVote { get; set; }

        public void CastVote ()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            Comment comment = dbContext.Comments.Include(c => c.Votes).Where(c => c.Id == VotedComment.Id).SingleOrDefault();
            //Check if this use has already made a vote on this comment
            try
            {
                Vote existingVote = comment.Votes.Where(v => v.VoterUserId == CastedVote.VoterUserId).SingleOrDefault();
                if (existingVote == null)
                {
                    //Add new vote to comment
                    comment.Votes.Add(CastedVote);
                    //dbContext.SaveChanges();
                }
                else
                {
                    //Check to see which way the user voted
                    existingVote.UpVote = CastedVote.UpVote;
                    existingVote.DownVote = CastedVote.DownVote;
                    if (existingVote.DownVote == false && existingVote.UpVote == false)
                    {
                        comment.Votes.Remove(existingVote);
                    }
                    //dbContext.SaveChanges();
                }

            }
            catch (DbEntityValidationException err)
            {
                string message = err.Message;
                throw;
            }
        }
    }
}
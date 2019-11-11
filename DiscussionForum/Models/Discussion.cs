using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class Discussion : IFeedItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(140, MinimumLength =5)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength =20, ErrorMessage ="Please add a few sentences describing the topic. More than 20 and less 2000 characters")]
        public string Premise { get; set; }

        public List<Picture> Pictures { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime? LastUpdated { get; set; }
        public List<Tag> Tags { get; set; }
        public int FeedPriority { get; set; }
        public bool IsAttachedToAnime { get; set; }
        public bool IsAttachedToNews { get; set; }

        public void CreateNewDiscussion(HttpPostedFileBase imageFile, string userId)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                this.Pictures = new List<Picture>();
                Picture pic = new Picture()
                {
                    ImageData = new byte[imageFile.ContentLength]
                };
                imageFile.InputStream.Read(pic.ImageData, 0, imageFile.ContentLength);
                Pictures.Add(pic);
            }
            LastUpdated = DateTime.Now;
            ApplicationDbContext dbContext = new ApplicationDbContext();
            IsAttachedToAnime = false;
            IsAttachedToNews = false;
            ApplicationUser _user = dbContext.Users.Include(u => u.FollowedDiscussions).SingleOrDefault(u => u.Id == userId);
            _user.FollowedDiscussions.Add(this);
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                throw;
            }
        }
    }

    public class Comment :IFeedItem
    {
        public int Id { get; set; }
        public string Who { get; set; }
        public Picture CommentorPic { get; set; }
        public string RepliedTo { get; set; }
        public string CommenterId { get; set; }
        public string What { get; set; }
        public int Likes { get; set; }
        public List<Comment> Replies { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int FeedPriority { get; set; }
        public List<Vote> Votes { get; set; }
    }

    public class Vote
    {
        public int VoteId { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }

        [Required]
        public string VoterUserId { get; set; }

    }
}
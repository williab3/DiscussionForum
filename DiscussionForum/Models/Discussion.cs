using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class Discussion : IFeedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Premise { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class Comment :IFeedItem
    {
        public int Id { get; set; }
        public string Who { get; set; }
        public string What { get; set; }
        public int Likes { get; set; }
        public List<Comment> Replies { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
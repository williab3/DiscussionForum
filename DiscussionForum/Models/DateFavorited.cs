using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class DateFavorited
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FeedItemId { get; set; }
        public DateTime WhenFavorited { get; set; }
    }
}
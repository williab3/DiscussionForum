using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class NewsArticle
    {
        public int Id { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public AnimeModel PrimaryAnime { get; set; }
        public Picture ArticleImage { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class NewAnimeReport
    {
        public int Id { get; set; }
        public List<AnimeModel> AnimeReference { get; set; } = new List<AnimeModel>();
        public string InfoUrl { get; set; }
        public string Title { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
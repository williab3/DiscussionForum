using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class AnimeGenre
    {
        public int Id { get; set; }
        public string GenreName { get; set; }

        public List<AnimeModel> Animes { get; set; }
    }
}
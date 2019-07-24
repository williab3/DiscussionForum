﻿using DiscussionForum.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace DiscussionForum.Models
{
    public class AnimeModel : IFeedItem
    {
        public int Id { get; set; }
        public int AnilistId { get; set; }
        public string Title_English { get; set; }
        public string Title_Romaji { get; set; }
        public string Description { get; set; }
        public string ImageUrlLarge { get; set; }
        public string ImageUrlMedium { get; set; }
        public string Color { get; set; }
        public List<AnimeGenre> Genres { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int Popularity { get; set; }
        public List<ApplicationUser> UsersFavorited { get; set; }
    }

    public interface IFeedItem
    {
        DateTime? LastUpdated { get; set; }
    }
}
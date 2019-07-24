using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace DiscussionForum.Models.ViewModel
{
    public class HomePage_UA
    {
        public List<IFeedItem> AnimeFeed { get; set; }

        public HomePage_UA()
        {
            AnimeFeed = new List<IFeedItem>();
            ApplicationDbContext dbContext = new ApplicationDbContext();
            List<AnimeModel> visitorFeed = dbContext.Animes.OrderByDescending(a => a.Popularity).Take(8).ToList();
            foreach (AnimeModel anime in visitorFeed)
            {
                AnimeFeed.Add(anime);
            }
        }

        public HomePage_UA(string userId)
        {
            
        }

    }
}
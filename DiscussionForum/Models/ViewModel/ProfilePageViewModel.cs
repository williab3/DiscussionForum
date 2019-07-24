using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models.ViewModel
{
    public class ProfilePageViewModel
    {
        public ApplicationUser User { get; set; }
        public List<IFeedItem> AnimeFeed { get; set; }
        public ProfilePageViewModel(string userId)
        {
            AnimeFeed = new List<IFeedItem>();
            ApplicationDbContext dbContext = new ApplicationDbContext();
            User = dbContext.Users.Include("FavoriteAnime").Include("FollowedDiscussions").Include("Friends").Where(u => u.Id == userId).SingleOrDefault();
            if (User.FavoriteAnime.Count > 0)
            {
                foreach (AnimeModel anime in User.FavoriteAnime)
                {
                    ProfileVewAnime profileAnime = new ProfileVewAnime(anime);
                    profileAnime.IsFavorite = true;
                    AnimeFeed.Add(profileAnime);
                }
            }
            if (User.FollowedDiscussions.Count > 0)
            {
                foreach (Discussion discussion in User.FollowedDiscussions)
                {
                    AnimeFeed.Add(discussion);
                }
            }
            int feedCount = AnimeFeed.Count;
            if (feedCount < 10) //Check if number of items in the profile feed is less than 10
            {
                List<AnimeModel> supplementalAnime = dbContext.Animes.OrderByDescending(a => a.Popularity).Take(10 - feedCount).ToList();
                foreach (AnimeModel anime in supplementalAnime)
                {
                    ProfileVewAnime profileAnime = new ProfileVewAnime(anime);
                    profileAnime.IsFavorite = false;
                    AnimeFeed.Add(profileAnime);
                }
            }
            AnimeFeed.OrderByDescending(af => af.LastUpdated);
        }
    }

    [NotMapped]
    public class ProfileVewAnime : AnimeModel
    {
        public bool IsFavorite { get; set; }

        public ProfileVewAnime()
        {

        }
        public ProfileVewAnime(AnimeModel anime)
        {
            Id = anime.Id;
            AnilistId = anime.AnilistId;
            Title_English = anime.Title_English;
            Title_Romaji = anime.Title_Romaji;
            Description = anime.Description;
            ImageUrlLarge = anime.ImageUrlLarge;
            ImageUrlMedium = anime.ImageUrlMedium;
            Color = anime.Color;
            Genres = anime.Genres;
            Popularity = anime.Popularity;
            UsersFavorited = anime.UsersFavorited;
            if (anime.LastUpdated != null)
            {
                LastUpdated = anime.LastUpdated;
            }
        }
    }
}
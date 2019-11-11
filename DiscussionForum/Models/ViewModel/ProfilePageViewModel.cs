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
        public Discussion FreshDiscussion { get; set; }
        public string FreshDiscussionImage { get; set; }

        public ProfilePageViewModel()
        {

        }
        public ProfilePageViewModel(string userId)
        {
            AnimeFeed = new List<IFeedItem>();
            ApplicationDbContext dbContext = new ApplicationDbContext();
            User = dbContext.Users.Include("FavoriteAnime").Include(u => u.FollowedDiscussions.Select(d => d.Pictures)).Include("Friends").Where(u => u.Id == userId).SingleOrDefault();
            if (User.FavoriteAnime.Count > 0)
            {
                foreach (AnimeModel anime in User.FavoriteAnime)
                {
                    if (anime.FeedPriority == 0)
                    {
                        SeedAnimePriority(anime);
                    }
                    anime.FeedPriority += 50;
                    ProfileVewAnime profileAnime = new ProfileVewAnime(anime);
                    profileAnime.IsFavorite = true;
                    AnimeFeed.Add(profileAnime);
                }
            }
            if (User.FollowedDiscussions.Count > 0)
            {
                //TODO: Seed Discussion feed priority for discussions that were made before the implementation of "FeedPriority" upon creation
                foreach (Discussion discussion in User.FollowedDiscussions)
                {
                    discussion.FeedPriority += 50;
                    AnimeFeed.Add(discussion);
                }
            }
            int feedCount = AnimeFeed.Count;
            if (feedCount < 10) //Check if number of items in the profile feed is less than 10
            {
                List<AnimeModel> additionalAnime = dbContext.Animes.AsEnumerable().OrderByDescending(a => a.Popularity).Where(a => isAnimeInFeed(a.Id)).Take(10 - feedCount).ToList();

                foreach (AnimeModel anime in additionalAnime)
                {
                    if (anime.FeedPriority == 0)
                    {
                        SeedAnimePriority(anime);
                    }
                    ProfileVewAnime profileAnime = new ProfileVewAnime(anime);
                    profileAnime.IsFavorite = false;
                    AnimeFeed.Add(profileAnime);
                }
            }
            AnimeFeed = AnimeFeed.OrderByDescending(af => af.FeedPriority).ToList();
        }

        private bool isAnimeInFeed(int animeId)
        {
            bool pass = true;
            if (AnimeFeed != null && AnimeFeed.Count > 0)
            {
                foreach (IFeedItem existingAnime in AnimeFeed)
                {
                    if (existingAnime.GetType() == typeof(ProfileVewAnime) && ((ProfileVewAnime)existingAnime).Id == animeId)
                    {
                        
                        pass = false;
                    }
                }
            }
            return pass;
        }

        public void SeedAnimePriority(AnimeModel anime)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            AnimeModel dbAnime = dbContext.Animes.Where(a => a.Id == anime.Id).Include("Discussion").Include("Discussion.Comments").SingleOrDefault();
            if (dbAnime.Discussion != null)
            {
                dbAnime.FeedPriority += (int)(dbAnime.Discussion.FeedPriority * .75);
                if (dbAnime.Discussion.Comments != null)
                {
                    dbAnime.Discussion.FeedPriority += dbAnime.Discussion.Comments.Count;
                }
            }
            anime.FeedPriority = dbAnime.FeedPriority;
            dbAnime.FeedPriority += (anime.Popularity / 1000);
            dbContext.SaveChanges();
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
            FeedPriority = anime.FeedPriority;
            UsersFavorited = anime.UsersFavorited;
            if (anime.LastUpdated != null)
            {
                LastUpdated = anime.LastUpdated;
            }
        }
    }
}
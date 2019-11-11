using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DiscussionForum.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string LoginName { get; set; }

        public List<ApplicationUser> Friends { get; set; }
        public List<AnimeModel> FavoriteAnime { get; set; }
        public List<Discussion> FollowedDiscussions { get; set; }

        public Picture ProfilePic { get; set; }
        public string Bio { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ApplicationUser()
        {

        }
        public ApplicationUser(string userId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            ApplicationUser _user = dbContext.Users.Include(m => m.FavoriteAnime).Include(m => m.Friends).Include(u => u.ProfilePic).SingleOrDefault(m => m.Id == userId);
            UserName = _user.UserName;
            LoginName = _user.LoginName;
            Friends = _user.Friends;
            FavoriteAnime = _user.FavoriteAnime;
            ProfilePic = _user.ProfilePic;
            Bio = _user.Bio;
        }

        public static string ToggleFavoriteAnime(int animeId, string userId)
        {
            string message;
            ApplicationDbContext dbContext = new ApplicationDbContext();
            ApplicationUser _user = dbContext.Users.Include(m => m.FavoriteAnime).SingleOrDefault(m => m.Id == userId);
            AnimeModel anime = dbContext.Animes.Where(a => a.Id == animeId).SingleOrDefault();
            DateFavorited whenFavorited = dbContext.DateFavorited.Where(df => df.UserId == userId && df.FeedItemId == animeId).SingleOrDefault();
            if (_user.FavoriteAnime.Contains(anime))
            {
                if (whenFavorited != null)
                {
                    dbContext.DateFavorited.Remove(whenFavorited);
                }
                _user.FavoriteAnime.Remove(anime);
                dbContext.SaveChanges();
                message = anime.Title_Romaji + " removed from current user's fave list.";
            }
            else
            {
                if (whenFavorited != null)
                {
                    whenFavorited.WhenFavorited = DateTime.Now;
                }
                else
                {
                    whenFavorited = new DateFavorited()
                    {
                        FeedItemId = animeId,
                        UserId = userId,
                        WhenFavorited = DateTime.Now,
                    };
                    dbContext.DateFavorited.Add(whenFavorited);
                }
                _user.FavoriteAnime.Add(anime);
                dbContext.SaveChanges();
                message = anime.Title_Romaji + " added to current user's fave list.";
            }
            return message;
        }
    }
    //TODO: Create Administrator role
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AnimeModel> Animes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<AnimeStatsModel> LastUpdated { get; set; }
        public DbSet<NewAnimeReport> NewAnimeReport { get; set; }
        public DbSet<AnimeGenre> Genres { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DateFavorited> DateFavorited { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Discussion> Discussions { get; set; }


        public static List<Tag> SeedTags()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            List<Tag> results = dbContext.Tags.ToList();
            foreach (AnimeModel anime in dbContext.Animes)
            {
                Tag existingRomajiTag = results.Where(t => t.Name == anime.Title_Romaji).SingleOrDefault();
                Tag existingEnglishTag = results.Where(t => t.Name == anime.Title_English).SingleOrDefault();
                if (existingEnglishTag == null && existingRomajiTag == null)
                {
                    Tag freshTag = new Tag()
                    {
                        Name = anime.Title_Romaji
                    };
                    results.Add(freshTag);
                    dbContext.Tags.Add(freshTag);
                    if (!String.IsNullOrEmpty(anime.Title_English) && anime.Title_English != anime.Title_Romaji)
                    {
                        freshTag.Name = anime.Title_English;
                        results.Add(freshTag);
                        dbContext.Tags.Add(freshTag);
                    }
                }
            }
            dbContext.SaveChanges();
            return results;
        }

        public static List<Tag> GetTags()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            List<Tag> results = dbContext.Tags.ToList();
            return results;
        }
    }
}
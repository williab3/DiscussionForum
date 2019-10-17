using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;


namespace DiscussionForum.Models
{
    public class AnimeStatsModel : IFeedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? LastUpdated { get; set; }

        public DateTime NextUpdate { get; set; }
        public DateTime AirTime { get; set; }
        public List<string> NewAnimeTitles { get; set; } = new List<string>();
        public int FeedPriority { get; set; }

        public async Task<List<string>> ImportAnimeUpdates()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Clear();
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await APICommunicator.ApiClient.GetAsync("https://anime-stats.net/api/v3/recent-changes"))
            {
                if (response.IsSuccessStatusCode)
                {
                    StatsData JSON = new StatsData();
                    JSON = await response.Content.ReadAsAsync<StatsData>();
                    foreach (AnimeMedia anime in JSON.LastUpdates.List)
                    {

                        AnimeStatsModel UpdateMatch = dbContext.LastUpdated.Where(a => a.Title == anime.Anime.Name).SingleOrDefault();

                        //Search the database for the anime by it's English name 
                        AnimeModel AnimeMatch = dbContext.Animes.Where(a => a.Title_English.Substring(0,5) == anime.Anime.Name.Substring(0,5)).SingleOrDefault();
                        if (AnimeMatch == null)
                        {
                            //Search the database for the anime by it's Romaji name if there was no match by it's English name.
                            AnimeMatch = dbContext.Animes.Where(a => a.Title_Romaji.Substring(0, 5) == anime.Anime.Name.Substring(0, 5)).SingleOrDefault();
                        }
                        //Update the anime's LastUpdate if there was a match.
                        if (AnimeMatch != null)
                        {
                            AnimeMatch.LastUpdated = anime.LastUpdate;
                        }

                        if (UpdateMatch == null)
                        {
                            //Add anime title to list to display in view
                            NewAnimeTitles.Add(anime.Anime.Name);
                            AnimeStatsModel newStats = new AnimeStatsModel()
                            {
                                Title = anime.Anime.Name,
                                LastUpdated = anime.LastUpdate,
                                NextUpdate = anime.NextUpdate,
                                AirTime = anime.Anime.Airtime
                            };
                            dbContext.LastUpdated.Add(newStats);
                        }
                        else
                        {
                            UpdateMatch.LastUpdated = anime.LastUpdate;
                            UpdateMatch.NextUpdate = anime.NextUpdate;
                            UpdateMatch.AirTime = anime.Anime.Airtime;
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
            return NewAnimeTitles;
        }


        /* Method Objectives
         1. Get Anime update dates & titles 
         2. Update the database with the updates
            a. Check the database to see if a record for that anime already exists
            --> if it does update the dates
            --> if it doesn't create a new record with the date retrieved.

        TODO: Make GetAnimeUpdates method only available to site administrators.
         */

    }

    class LastUpdated
    {
        public AnimeMedia[] List { get; set; }
    }
    class StatsData
    {
        public LastUpdated LastUpdates { get; set; }
    }
    class Anime
    {
        public string Name { get; set; }
        public DateTime Airtime { get; set; }
    }
    class AnimeMedia
    {
        public DateTime LastUpdate { get; set; }
        public DateTime NextUpdate { get; set; }
        public Anime Anime { get; set; }
    }
}
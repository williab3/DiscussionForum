using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models.ViewModel
{
    public class MainAdminViewModel
    {
        public int TotalAnimeInDb { get; set; }
        public int TotalGenre { get; set; }
        public List<String> NewAnimeTitles { get; set; } = new List<string>();
        public List<string> NewGenres { get; set; } = new List<string>();
        public int PopularityUpdated { get; set; }
        public bool DidSeedPopularity { get; set; } = false;
        public List<AnimeModel> AnimeModels { get; set; } = new List<AnimeModel>();
        public List<NewAnimeReport> ReportItems { get; set; }
        public AnilistVariables RequestVariables { get; set; }
        public List<Tag> FreshTags { get; set; }

        public async Task<MainAdminViewModel> ImportNewAnimeData()
        {
            /* Method Objectives
             1. Get a list of the most popular anime from Anilist.co
             2. Update database with the data
                A. Check to see if the anime is already present in the database
                    --> If it is NOT then add it to the database

            TODO: Make this method only available to administrators
             */
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Clear();
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string query = @"query ($status: MediaStatus, $page: Int, $perPage: Int) {
  Page (page: $page, perPage: $perPage) {
    pageInfo {
      total
      currentPage
      lastPage
      hasNextPage
      perPage
    }
    media (status: $status, sort: POPULARITY_DESC) {
      id
      popularity
      title {
        english
        romaji
      }
      description
      episodes
      genres
      coverImage {
        medium
        extraLarge
        color
      }
    
    }
  }
}";
            MainAdminViewModel adminViewModel = new MainAdminViewModel();
            var variables = new
            {
                status = "RELEASING",
                page = 1,
                perPage = 10
            };
            using (HttpResponseMessage response = await APICommunicator.ApiClient.PostAsJsonAsync("https://graphql.anilist.co", new { query, variables }))
            {
                if (response.IsSuccessStatusCode)
                {
                    AnilistResponseModel content = await response.Content.ReadAsAsync<AnilistResponseModel>();
                    string searchTerm;
                    ApplicationDbContext dbContext = new ApplicationDbContext();
                    adminViewModel = await IndexAndSaveNewAnimeAsync(content);
                    //foreach (Media animeData in content.Data.Page.Media)
                    //{
                    //    AnimeModel existingAnime = dbContext.Animes.Include("Genres").Where(a => a.AnilistId == animeData.Id).SingleOrDefault();
                    //    existingAnime.ImageUrlLarge = animeData.CoverImage.ExtraLarge;
                    //    AnimeGenre newGenre;
                    //    List<AnimeGenre> newGenresList = new List<AnimeGenre>();

                    //    if (!String.IsNullOrEmpty(animeData.Title.Romaji))
                    //    {
                    //        searchTerm = animeData.Title.Romaji.Substring(0, 5);
                    //    }
                    //    else
                    //    {
                    //        searchTerm = animeData.Title.English.Substring(0, 5);
                    //    }

                    //    foreach (string genre in animeData.Genres)
                    //    {
                    //        /* Loop Objectives
                    //        1. Check to see if the genre already exist in the database
                    //            --> If it does that add it to the new anime's list of genres
                    //            --> If it does not:
                    //                A. Create the new genre
                    //                B. Add is to the new anime's list of genres.
                    //        */
                    //        newGenre = dbContext.Genres.Where(g => g.GenreName == genre).SingleOrDefault();
                    //        if (newGenre == null)
                    //        {
                    //            newGenre = new AnimeGenre()
                    //            {
                    //                GenreName = genre,
                    //            };
                    //            adminViewModel.NewGenres.Add(genre);
                    //        }
                    //        newGenresList.Add(newGenre);
                    //    }

                    //    if (existingAnime == null)
                    //    {
                    //        existingAnime = new AnimeModel()
                    //        {
                    //            AnilistId = animeData.Id,
                    //            Title_English = animeData.Title.English,
                    //            Title_Romaji = animeData.Title.Romaji,
                    //            Description = animeData.Description,
                    //            ImageUrlLarge = animeData.CoverImage.Large,
                    //            ImageUrlMedium = animeData.CoverImage.Medium,
                    //            Color = animeData.CoverImage.Color,
                    //            Popularity = animeData.Popularity,
                    //            Genres = newGenresList,
                    //        };
                    //        if (!String.IsNullOrEmpty(animeData.Title.Romaji))
                    //        {
                    //            adminViewModel.NewAnimeTitles.Add(animeData.Title.Romaji);
                    //        }
                    //        else if (!String.IsNullOrEmpty(animeData.Title.English))
                    //        {
                    //            adminViewModel.NewAnimeTitles.Add(animeData.Title.English);
                    //        }
                    //        AnimeStatsModel statsModel = dbContext.LastUpdated.Where(a => a.Title.Substring(0, 5) == searchTerm).FirstOrDefault();
                    //        if (statsModel != null)
                    //        {
                    //            existingAnime.LastUpdated = statsModel.LastUpdated;
                    //        }

                    //        dbContext.Animes.Add(existingAnime);

                    //    }
                    //    else
                    //    {
                    //        AnimeStatsModel statsModel = dbContext.LastUpdated.Where(a => a.Title.Substring(0, 5) == searchTerm).FirstOrDefault();
                    //        if (statsModel != null)
                    //        {
                    //            existingAnime.LastUpdated = statsModel.LastUpdated;
                    //        }

                    //    }
                    //    dbContext.SaveChanges();
                    //}
                    adminViewModel.TotalAnimeInDb += dbContext.Animes.Count();
                    adminViewModel.TotalGenre += dbContext.Genres.Count();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            return adminViewModel;
        }

        public async Task SeedAnimePopularity()
        {
            DidSeedPopularity = true;
            string query = @"query {
  Page(page:1, perPage:13) { pageInfo{
      total }
    media (status: RELEASING, sort: POPULARITY_DESC) {
        id 
        popularity } } }";
            using (HttpResponseMessage contentResponse = await APICommunicator.ApiClient.PostAsJsonAsync("https://graphql.anilist.co", new { query}))
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                AnilistResponseModel response = await contentResponse.Content.ReadAsAsync<AnilistResponseModel>();
                foreach (Media animeItem in response.Data.Page.Media)
                {
                    AnimeModel anime = dbContext.Animes.SingleOrDefault(a => a.AnilistId == animeItem.Id);
                    if (anime != null)
                    {
                        anime.Popularity = animeItem.Popularity;
                        PopularityUpdated += 1;
                        dbContext.SaveChanges();
                    }
                }
                AnimeModels = dbContext.Animes.Where(a => a.Popularity == 0).ToList();
            }
        }

        public async Task SeedAnimePopularity(int anilistId)
        {
            DidSeedPopularity = true;
            string query = @"query($id: Int) {
                Media(id:$id, id_not:99999 ){
                    id
                    popularity
                    title{ english }
                } }";
            object variables = new
            {
                id = anilistId
            };
            ApplicationDbContext dbContext = new ApplicationDbContext();
            using (HttpResponseMessage response = await APICommunicator.ApiClient.PostAsJsonAsync("https://graphql.anilist.co", new {query, variables }))
            {
                if (response.IsSuccessStatusCode)
                {
                    AnilistResponseModel dataContent = await response.Content.ReadAsAsync<AnilistResponseModel>();
                    AnimeModel anime = dbContext.Animes.SingleOrDefault(a => a.AnilistId == anilistId);
                    if (anime != null)
                    {
                        anime.Popularity = dataContent.Data.Media.Popularity;
                        dbContext.SaveChanges();
                    }
                    AnimeModels = dbContext.Animes.Where(a => a.Popularity == 0).ToList();
                }
            }
        }

        public async Task RefreshNewsReport()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Clear();
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            using (HttpResponseMessage response = await APICommunicator.ApiClient.GetAsync("https://www.animenewsnetwork.com/encyclopedia/reports.xml?id=148"))
            {

                if (response.IsSuccessStatusCode)
                {
                    //dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.NewAnimeReports");
                    ReportItems = new List<NewAnimeReport>();
                    string xmlString = await response.Content.ReadAsStringAsync();
                    XmlDocument xmlReport = new XmlDocument();
                    xmlReport.LoadXml(xmlString);
                    foreach (XmlNode xmlNode in xmlReport.DocumentElement.ChildNodes)
                    {
                        NewAnimeReport reportItem = new NewAnimeReport();
                        reportItem.InfoUrl = xmlNode.ChildNodes[0].Attributes["href"].Value;
                        reportItem.Title = xmlNode.ChildNodes[0].InnerText;
                        string searchTerm = reportItem.Title.Substring(0, 5);
                        DateTime theDate;
                        if (DateTime.TryParse(xmlNode.ChildNodes[1].InnerText, out theDate))
                        {
                            reportItem.DateAdded = theDate;
                        }
                        List<AnimeModel> referenceAnime = dbContext.Animes.Where(a => a.Title_Romaji.Substring(0, 5) == searchTerm).ToList();
                        reportItem.AnimeReference = referenceAnime;
                        dbContext.NewAnimeReport.Add(reportItem);
                        dbContext.SaveChanges();
                    } 
                }

            }
            ReportItems = dbContext.NewAnimeReport.ToList();

        }

        public async static Task<MainAdminViewModel> IndexAndSaveNewAnimeAsync(AnilistResponseModel anilistResponse)
        {
            string searchTerm;
            MainAdminViewModel newData = new MainAdminViewModel();
            newData.FreshTags = new List<Tag>();
            ApplicationDbContext dbContext = new ApplicationDbContext();
            await Task.Run(() => {
                foreach (Media animeData in anilistResponse.Data.Page.Media)
                {
                    AnimeGenre newGenre;
                    Tag freshTag;
                    List<AnimeGenre> newGenresList = new List<AnimeGenre>();
                    foreach (string genre in animeData.Genres)
                    {
                        /* Loop Objectives
                        1. Check to see if the genre already exist in the database
                            --> If it does that add it to the new anime's list of genres
                            --> If it does not:
                                A. Create the new genre
                                B. Add is to the new anime's list of genres.
                        */
                        //Add a new genre
                        newGenre = dbContext.Genres.Where(g => g.GenreName == genre).FirstOrDefault();
                        if (newGenre == null)
                        {
                            newGenre = new AnimeGenre()
                            {
                                GenreName = genre,
                            };
                            newData.NewGenres.Add(genre);
                            dbContext.Genres.Add(newGenre);
                        }
                        newGenresList.Add(newGenre);
                        //Add a new tag for new genres
                        freshTag = dbContext.Tags.Where(t => t.Name == genre).FirstOrDefault();
                        if (freshTag == null)
                        {
                            freshTag = new Tag()
                            {
                                Name = genre,
                            };
                            newData.FreshTags.Add(freshTag);
                            dbContext.Tags.Add(freshTag);
                        }
                    }
                    //Add new tag for Romaji title
                    freshTag = dbContext.Tags.Where(t => t.Name == animeData.Title.Romaji).FirstOrDefault();
                    if (freshTag == null)
                    {
                        freshTag = new Tag()
                        {
                            Name = animeData.Title.Romaji,
                        };
                        dbContext.Tags.Add(freshTag);
                        newData.FreshTags.Add(freshTag);
                    }
                    //Add new tag for English title
                    Tag englishTag = dbContext.Tags.Where(t => t.Name == animeData.Title.English).FirstOrDefault();
                    if (englishTag == null)
                    {
                        englishTag = new Tag()
                        {
                            Name = animeData.Title.English
                        };
                        dbContext.Tags.Add(englishTag);
                        newData.FreshTags.Add(englishTag);
                    }

                    //Create a search term to use as a cross reference with existing anime in the database
                    if (!String.IsNullOrEmpty(animeData.Title.Romaji))
                    {
                        int nameLength = animeData.Title.Romaji.Length;
                        if (nameLength < 6)
                        {
                            searchTerm = animeData.Title.Romaji.Substring(0, nameLength - 1);
                        }
                        else
                        {
                            searchTerm = animeData.Title.Romaji.Substring(0, 5);
                        }
                    }
                    else
                    {
                        searchTerm = animeData.Title.English.Substring(0, 5);
                    }

                    AnimeModel existingAnime = dbContext.Animes.Include("Genres").Where(a => a.AnilistId == animeData.Id).SingleOrDefault();
                    if (existingAnime == null)
                    {
                        existingAnime = new AnimeModel()
                        {
                            AnilistId = animeData.Id,
                            Title_English = animeData.Title.English,
                            Title_Romaji = animeData.Title.Romaji,
                            Description = animeData.Description,
                            ImageUrlLarge = animeData.CoverImage.ExtraLarge,
                            ImageUrlMedium = animeData.CoverImage.Medium,
                            Color = animeData.CoverImage.Color,
                            Popularity = animeData.Popularity,
                            Genres = newGenresList,
                        };
                        //Add the new anime's title to a list to display in the view
                        if (!String.IsNullOrEmpty(animeData.Title.Romaji))
                        {
                            newData.NewAnimeTitles.Add(animeData.Title.Romaji);
                        }
                        else if (!String.IsNullOrEmpty(animeData.Title.English))
                        {
                            newData.NewAnimeTitles.Add(animeData.Title.English);
                        }
                        //Cross reference with the anime-stats table to see when the last time the anime was updated
                        AnimeStatsModel statsModel = dbContext.LastUpdated.Where(a => a.Title.Substring(0, 5) == searchTerm).FirstOrDefault();
                        if (statsModel != null)
                        {
                            existingAnime.LastUpdated = statsModel.LastUpdated;
                        }

                        dbContext.Animes.Add(existingAnime);

                    }
                    else
                    {
                        AnimeStatsModel statsModel = dbContext.LastUpdated.Where(a => a.Title.Substring(0, 5) == searchTerm).FirstOrDefault();
                        if (statsModel != null)
                        {
                            existingAnime.LastUpdated = statsModel.LastUpdated;
                        }

                    }


                    dbContext.SaveChanges();
                }
            });
            return newData;
        }
    }

    public class AnilistVariables
    {
        [Display(Name ="Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name ="Release Status")]
        public string ReleaseStatus { get; set; }

        [Display(Name ="Anime Title")]
        public string Title { get; set; }

        public async Task<MainAdminViewModel> MassImport()
        {
            string var0;
            string var1;
            string var2;
            string var3;
            string var4;
            string var5;
            if (StartDate == DateTime.MinValue)
            {
                var0 = "";
                var1 = "";
            }
            else
            {
                var0 = "$startDate: FuzzyDateInt,";
                var1 = "startDate: $startDate,";

            }
            if (EndDate == DateTime.MinValue)
            {
                var2 = "";
                var3 = "";
            }
            else
            {
                var2 = "$endDate:FuzzyDateInt,";
                var3 = "endDate: $endDate,";

            }
            if (String.IsNullOrEmpty(ReleaseStatus))
            {
                var4 = "";
                var5 = "";
            }
            else
            {
                var4 = " $releaseStatus: MediaStatus,";
                var5 = " status: $releaseStatus";
            }
            var variables = new {
                startDate = StartDate.ToString("yyyyMMdd"),
                endDate = EndDate.ToString("yyyyMMdd"),
                releaseStatus = ReleaseStatus,
                page = 1,
                perPage = 100
            };

            string query = @"query(" + var0 + var2 + var4 + " $page: Int, $perPage:Int){";
            query += " Page(page: $page, perPage: $perPage){";
            query += " pageInfo{ total currentPage ";
            query += " } media("+ var1+ var3+ var5 + " ){";
            query += " id genres episodes popularity description";
            query += " title{ english romaji";
            query += " } coverImage{ medium extraLarge color } } } }";

            string endpoint = "https://graphql.anilist.co";
            MainAdminViewModel viewModel = new MainAdminViewModel();
            using (HttpResponseMessage response = await APICommunicator.ApiClient.PostAsJsonAsync(endpoint, new {query = query, variables = variables }))
            {
                if (response.IsSuccessStatusCode)
                {
                    AnilistResponseModel content = await response.Content.ReadAsAsync<AnilistResponseModel>();
                    viewModel = await MainAdminViewModel.IndexAndSaveNewAnimeAsync(content);
                }
            }
            return viewModel;
        }
    }

}
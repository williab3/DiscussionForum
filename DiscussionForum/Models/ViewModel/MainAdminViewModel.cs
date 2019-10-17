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
using Newtonsoft.Json;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;

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
        public ImportedAnime AnimeGridData { get; set; }
        public List<NewAnimeReport> ReportItems { get; set; }
        public AnilistVariables RequestVariables { get; set; }
        public List<Tag> FreshTags { get; set; } = new List<Tag>();
        public string ErrorMessage { get; set; }

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
            string query = @"query ($status: MediaStatus, $page: Int, $perPage: Int, $sort: MediaSort) {
            Page (page: $page, perPage: $perPage) {
            pageInfo { total currentPage lastPage hasNextPage perPage } media (status: $status, sort: $sort) { id popularity startDate {
            year month day } title { english romaji } description episodes genres coverImage { medium extraLarge color } } } }";

            MainAdminViewModel adminViewModel = new MainAdminViewModel();
            var variables = new
            {
                status = "RELEASING",
                page = 1,
                perPage = 10,
                sort = ""
            };
            using (HttpResponseMessage response = await APICommunicator.ApiClient.PostAsJsonAsync("https://graphql.anilist.co", new { query, variables }))
            {
                if (response.IsSuccessStatusCode)
                {
                    AnilistResponseModel content = await response.Content.ReadAsAsync<AnilistResponseModel>();
                    ApplicationDbContext dbContext = new ApplicationDbContext();
                    //adminViewModel = await IndexAndSaveNewAnimeAsync(content);
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


        public async Task SaveAnimeToDB()
        {
            string searchTerm;
            List<AnimeModel> newData = new List<AnimeModel>();
            ApplicationDbContext dbContext = new ApplicationDbContext();
            if (AnimeModels != null)
            {
                await Task.Run(async () => {
                foreach (AnimeModel animeData in AnimeModels)
                    {
                        AnimeGenre newGenre;
                        Tag freshTag;
                        List<AnimeGenre> newGenresList = new List<AnimeGenre>();
                        animeData.Discussion = new Discussion();
                        if (String.IsNullOrEmpty(animeData.Title_English))
                        {
                            animeData.Discussion.Title = animeData.Title_Romaji;
                        }
                        else
                        {
                            animeData.Discussion.Title = animeData.Title_English;
                        }
                        animeData.Discussion.Premise = animeData.Description;
                        animeData.Discussion.IsAttachedToAnime = true;
                        animeData.Discussion.IsAttachedToNews = false;
                        //animeData.Discussion.Pictures = new List<Picture>().Add(new Picture() { ImageData = animeData.ImageUrlLarge });
                        
                        if (animeData.Genres != null && animeData.Genres.Count > 0)
                        {
                            foreach (AnimeGenre genre in animeData.Genres)
                            {
                                /* Loop Objectives
                                1. Check to see if the genre already exist in the database
                                    --> If it does that add it to the new anime's list of genres
                                    --> If it does not:
                                        A. Create the new genre
                                        B. Add is to the new anime's list of genres.
                                */
                                //Add a new genre
                                newGenre = dbContext.Genres.Where(g => g.GenreName == genre.GenreName).FirstOrDefault();
                                if (newGenre == null)
                                {
                                    newGenre = new AnimeGenre()
                                    {
                                        GenreName = genre.GenreName,
                                };
                                NewGenres.Add(genre.GenreName);
                                dbContext.Genres.Add(newGenre);
                            }
                            newGenresList.Add(newGenre);// <-- what's this for???
                                                        //Add a new tag for new genres
                            freshTag = dbContext.Tags.Where(t => t.Name == genre.GenreName).FirstOrDefault();
                            if (freshTag == null)
                            {
                                freshTag = new Tag()
                                {
                                    Name = genre.GenreName,
                                };
                                FreshTags.Add(freshTag);
                                dbContext.Tags.Add(freshTag);
                            }
                        }
                    }
                    //Add new tag for Romaji title
                    freshTag = dbContext.Tags.Where(t => t.Name == animeData.Title_Romaji).FirstOrDefault();
                    if (freshTag == null)
                    {
                        freshTag = new Tag()
                        {
                            Name = animeData.Title_Romaji
                        };
                        dbContext.Tags.Add(freshTag);
                            try
                            {
                                dbContext.SaveChanges();
                            }
                            catch (Exception err)
                            {
                                
                                throw;
                            }
                        FreshTags.Add(freshTag);
                    }
                    //Add new tag for English title
                    if (!String.IsNullOrEmpty(animeData.Title_English))
                    {
                        Tag englishTag = dbContext.Tags.Where(t => t.Name == animeData.Title_English).FirstOrDefault();
                        if (englishTag == null)
                        {
                            englishTag = new Tag()
                            {
                                Name = animeData.Title_English
                            };
                            dbContext.Tags.Add(englishTag);
                            FreshTags.Add(englishTag);
                        }
                    }
                        //Calculate initial static feed priority
                        animeData.FeedPriority = (animeData.Popularity / 100);
                        int minimumFeedPriority = (animeData.Popularity / 500);
                        animeData.AdjustedDate = DateTime.Now;
                        TimeSpan age = DateTime.Now - animeData.AdjustedDate;
                        if (animeData.FeedPriority >= minimumFeedPriority)
                        {
                            animeData.FeedPriority -= (age.Days * 2); 
                        }


                        AnimeModel existingAnime = dbContext.Animes.Include("Genres").Where(a => a.AnilistId == animeData.Id).SingleOrDefault();
                        if (existingAnime == null)
                        {
                            dbContext.Animes.Add(animeData);
                            //Add the new anime's title to a list to display in the view
                            if (!String.IsNullOrEmpty(animeData.Title_Romaji))
                            {
                                NewAnimeTitles.Add(animeData.Title_Romaji);
                            }

                            try
                            {
                                dbContext.SaveChanges();
                            }
                            catch (DbEntityValidationException err)
                            {
                                Console.WriteLine(err.Message);
                                throw;
                            }
                            catch(Exception err)
                            {
                                Console.WriteLine(err.Message);
                                throw;
                            }
                        }
                    }
                });
            }
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

        public string SDQuantifier { get; set; }
        public string EDQuantifier { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PerPage { get; set; } = 10;

        [Display(Name ="Sort")]
        public string SortByDate { get; set; }

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
                if (!String.IsNullOrEmpty(SDQuantifier))
                {
                    var1 = SDQuantifier + ": $startDate,";
                }
                else
                {
                    var1 = "startDate: $startDate,";

                }

            }
            if (EndDate == DateTime.MinValue)
            {
                var2 = "";
                var3 = "";
            }
            else
            {
                var2 = "$endDate:FuzzyDateInt,";
                if (!String.IsNullOrEmpty(EDQuantifier))
                {
                    var3 = EDQuantifier + ": $endDate,";
                }
                else
                {
                    var3 = "endDate: $endDate,";
                }

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
            
            string query = @"query(" + var0 + var2 + var4 + " $page: Int, $perPage:Int, $sort:[MediaSort]){";
            query += " Page(page: $page, perPage: $perPage){";
            query += " pageInfo{ total currentPage perPage";
            query += " } media("+ var1+ var3+ var5 + " sort: $sort, type:ANIME ){";
            query += " id genres episodes popularity description ";
            query += "startDate {year month day} title{ english romaji";
            query += " } coverImage{ medium extraLarge color } } } }";
            var variables = new
            {
                startDate = StartDate.ToString("yyyyMMdd"),
                endDate = EndDate.ToString("yyyyMMdd"),
                releaseStatus = ReleaseStatus,
                page = PageNumber,
                perPage = PerPage,
                sort = SortByDate,
            };

            string endpoint = "https://graphql.anilist.co";
            MainAdminViewModel viewModel = new MainAdminViewModel();
            using (HttpResponseMessage response = await APICommunicator.ApiClient.PostAsJsonAsync(endpoint, new {query = query, variables = variables }))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        //string text = await response.Content.ReadAsStringAsync();
                        AnilistResponseModel content = await response.Content.ReadAsAsync<AnilistResponseModel>();
                        //viewModel = await MainAdminViewModel.IndexAndSaveNewAnimeAsync(content);
                        viewModel.AnimeGridData = IndexAnilistAnime(content.Data.Page);

                    }
                    catch (Exception err)
                    {
                        viewModel.ErrorMessage = err.Message;
                    }
                }
            }
            return viewModel;
        }

        private ImportedAnime IndexAnilistAnime(Page anilistResponse)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            List<AnimeModel> SavedAnime = dbContext.Animes.ToList();
            ImportedAnime imported = new ImportedAnime();
            imported.Animes = new List<AnimeModel>();
            imported.CurrentPage = anilistResponse.PageInfo.CurrentPage;
            imported.TotalAnime = anilistResponse.PageInfo.Total;
            imported.PerPage = anilistResponse.PageInfo.PerPage;
            foreach (Media media in anilistResponse.Media)
            {
                int year;
                int month;
                int day;
                if (!int.TryParse(media.StartDate.day, out day))
                {
                    day = 1;
                };
                if (!int.TryParse(media.StartDate.Month, out month))
                {
                    month = 1;
                };
                if (!int.TryParse(media.StartDate.Year, out year))
                {
                    year = 1990;
                };
                AnimeModel freshAnime = new AnimeModel()
                {
                    Title_English = media.Title.English,
                    Title_Romaji = media.Title.Romaji,
                    ImageUrlLarge = media.CoverImage.ExtraLarge,
                    ImageUrlMedium = media.CoverImage.Medium,
                    Color = media.CoverImage.Color,
                    StartDate = new DateTime(year, month, day),
                    Description = media.Description,
                    Popularity = media.Popularity,
                    LastUpdated = DateTime.Now,
                    FeedPriority = (int)(media.Popularity / 1000) - ((DateTime.Now - StartDate).Days * 2),
                    Genres = new List<AnimeGenre>(),
                    AnilistId = media.Id
                };
                AnimeModel tempTest = SavedAnime.Where(m => m.AnilistId == freshAnime.AnilistId).SingleOrDefault();
                if (tempTest != null)
                {
                    freshAnime.ExistInDB = true;
                }
                foreach (string genreName in media.Genres)
                {
                    freshAnime.Genres.Add(new AnimeGenre() { GenreName = genreName });
                }
                imported.Animes.Add(freshAnime);
            }
            return imported;
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace DiscussionForum.Models
{
    public class Media
    {
        public int Id { get; set; }
        public Title Title { get; set; }
        public string Description { get; set; }
        public CoverImage CoverImage { get; set; }
        public string[] Genres { get; set; }
        public int? Episodes { get; set; }
        public int Popularity { get; set; }
        public int IdMal { get; set; }
        public StartDate StartDate { get; set; }

        public async Task GetAnime(int animeId)
        {
            string query = @"query ($id: Int) { Media(id: $id, type: ANIME) { id title { english } description episodes genres coverImage { large color } } }";
            var variables = new
            {
                id = animeId,
            };

            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Clear();
            APICommunicator.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage httpResponse = await APICommunicator.ApiClient.PostAsJsonAsync("https://graphql.anilist.co", new {query, variables }))
            {
                AnilistResponseModel content = await httpResponse.Content.ReadAsAsync<AnilistResponseModel>();
                Id = content.Data.Media.Id;
                Title = content.Data.Media.Title;
                Description = content.Data.Media.Description;
                CoverImage = content.Data.Media.CoverImage;
                Genres = content.Data.Media.Genres;
                if (content.Data.Media.Episodes != null)
                {
                    Episodes = content.Data.Media.Episodes;
                }
            }
        }

    }

    public class StartDate
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string day { get; set; }
    }

    public class Title
    {
        public string Romaji { get; set; }
        public string English { get; set; }
        public string Native { get; set; }
        public string UserPrefered { get; set; }
    }

    public class CoverImage
    {
        public string Color { get; set; }
        public string Medium { get; set; }
        public string Large { get; set; }
        public string ExtraLarge { get; set; }
    }

    public class Data
    {
        public Page Page { get; set; }
        public Media Media { get; set; }

    }

    public class AnilistResponseModel
    {
        public Data Data { get; set; }
        public Errors Errors { get; set; }
    }

    public class Errors
    {
        public string Message { get; set; }
    }

    public class Page
    {
        public Media[] Media { get; set; }
        public PageInfo PageInfo { get; set; }

    }
    public class PageInfo
    {
        public int CurrentPage { get; set; }
        public bool HasNextPage { get; set; }
        public int lastPage { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
    }
}
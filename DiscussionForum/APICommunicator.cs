using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DiscussionForum
{
    public static class APICommunicator
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitilizeClient()
        {
            ApiClient = new HttpClient();

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models.ViewModel
{
    public class AnimeViewModel
    {
        public Media Media { get; set; }

        public AnimeViewModel()
        {
            Media = new Media();
        }
    }
}
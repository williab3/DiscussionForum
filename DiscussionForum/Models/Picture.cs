using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        [NotMapped]
        [Display(Name ="Picture File")]
        public HttpPostedFile File { get; set; }

        [NotMapped]
        [Range(0,int.MaxValue)]
        public int XCoordinate { get; set; }

        [NotMapped]
        [Range(0, int.MaxValue)]
        public int YCoordinate { get; set; }

        [NotMapped]
        [Range(1,int.MaxValue)]
        public int Height { get; set; }

        [NotMapped]
        [Range(1,int.MaxValue)]
        public int Width { get; set; }
    }
}
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
        public byte[]  ImageData{ get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscussionForum.Models
{
    public class Error
    {
        public string ErrorMessage { get; set; }
        public bool HasErrors { get; set; }
        public string Translation { get; set; }

        public Type ErrorType { get; set; }

    }
}
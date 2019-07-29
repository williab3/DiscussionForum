﻿using System;
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VPWebsite.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string VideoName { get; set; }
        public string VideoPath { get; set; }
        public DateTime DateTime { get; set; }
    }
}
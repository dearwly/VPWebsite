using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VPWebsite.Models
{
    public class VideoView
    {
        public Video Video { get; set; }
        public String UserName { get; set; }
        public string FormattedUploadTime { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using PagedList;
using VPWebsite.Models;

namespace VPWebsite.Controllers
{

    public class HomeController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        public ActionResult Index(int? page, string search, string sort)
        {
            // Page now, default: 1
            int pageNumber = page ?? 1;

            // Video Number every page
            int pageSize = 9;

            // get all videos
            var videos = VPWebsite.Models.Video.GetAllVideos().AsQueryable();

            // search function
            if (!string.IsNullOrEmpty(search))
            {
                videos = videos.Where(v => v.VideoTitle.Contains(search));
            }

            // sort function
            switch (sort)
            {
                case "date_asc":
                    videos = videos.OrderBy(v => v.DateTime);
                    break;
                case "date_desc":
                    videos = videos.OrderByDescending(v => v.DateTime);
                    break;
                case "title_asc":
                    videos = videos.OrderBy(v => v.VideoTitle);
                    break;
                case "title_desc":
                    videos = videos.OrderByDescending(v => v.VideoTitle);
                    break;
                default:
                    videos = videos.OrderByDescending(v => v.DateTime);
                    break;
            }

            // Pagination
            var pagedList = videos.ToPagedList(pageNumber, pageSize);

            // Passing search and sort parameters to the front end
            ViewBag.SearchQuery = search;
            ViewBag.SortOrder = sort;

            return View(pagedList);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}
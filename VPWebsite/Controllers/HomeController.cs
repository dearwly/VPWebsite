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
            // 当前页数，默认为 1
            int pageNumber = page ?? 1;

            // 每页显示的视频数量
            int pageSize = 9;

            // 获取所有视频
            var videos = VPWebsite.Models.Video.GetAllVideos().AsQueryable();

            // 搜索功能
            if (!string.IsNullOrEmpty(search))
            {
                videos = videos.Where(v => v.VideoTitle.Contains(search));
            }

            // 排序功能
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

            // 分页
            var pagedList = videos.ToPagedList(pageNumber, pageSize);

            // 传递搜索和排序参数到前端
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
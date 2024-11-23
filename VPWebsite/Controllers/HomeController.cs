using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using VPWebsite.Models;

namespace VPWebsite.Controllers
{

    public class HomeController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        public ActionResult Index()
        {
            var videos = GetAllVideos();
            return View(videos);
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

        public List<Video> GetAllVideos()
        {
            List<Video> videos = new List<Video>();
            string query = "SELECT VideoId, Title, VideoName, VideoPath, UploadTime, User FROM Videos";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Video video = new Video
                        {
                            Id = Convert.ToInt32(reader["VideoId"]),
                            VideoTitle = reader["Title"].ToString(),
                            VideoName = reader["VideoName"].ToString(),
                            VideoPath = reader["VideoPath"].ToString(),
                            DateTime = Convert.ToDateTime(reader["UploadTime"]),
                            User = Convert.ToInt32(reader["User"])
                        };

                        videos.Add(video);
                    }
                }
            }
            return videos;
        }
    }
}
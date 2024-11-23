using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using MySql.Data.MySqlClient;

namespace VPWebsite.Models
{
    public class Video
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        public int Id { get; set; }
        public string VideoName { get; set; }
        public string VideoPath { get; set; }
        public DateTime DateTime { get; set; }
        public string VideoTitle { get; set; }
        public int User { get; set; }

        public string GetthumbnailsUrl()
        {
            return "~/Content/thumbnails/" + Path.GetFileNameWithoutExtension(VideoName) + ".jpg";
        }

        public static Video GetVideoById(int id)
        {
            Video video = null;
            string query = "SELECT VideoName, VideoPath, UploadTime, User, Title FROM Videos WHERE VideoId = @VideoId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VideoId", id);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        video = new Video
                        {
                            Id = id,
                            VideoName  = reader.GetString("VideoName"),
                            VideoPath = reader.GetString("VideoPath"),
                            VideoTitle = reader.GetString("Title"),
                            DateTime = Convert.ToDateTime(reader["UploadTime"]),
                            User = Convert.ToInt32(reader["User"])
                        };
                    }
                }
            }
            return video;
        }
    }
}
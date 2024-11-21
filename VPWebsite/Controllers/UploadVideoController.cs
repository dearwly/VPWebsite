using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using VPWebsite.Models;


public class UploadVideoController : Controller
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

    [HttpPost]
    public ActionResult UploadVideo(HttpPostedFileBase videoFile)
    {
        if (videoFile != null && videoFile.ContentLength > 0)
        {
            // 设置保存文件的路径
            string fileName = Path.GetFileName(videoFile.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/videos"), fileName);
            DateTime datetime = DateTime.Now;
            Video video = new Video
            {
                VideoName = fileName,
                VideoPath = path,
                DateTime = datetime,
            };
            // 保存文件到服务器
            videoFile.SaveAs(path);

            // 将文件信息存储到数据库
            SaveVideoInfoToDatabase(video);

            ViewBag.Message = "视频上传成功!";
        }
        else
        {
            ViewBag.Message = "请选择一个视频文件上传!";
        }

        return View();
    }

    private void SaveVideoInfoToDatabase(Video video)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO Videos (VideoName, VideoPath, UploadTime) VALUES (@VideoName, @VideoPath, @UploadTime)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@VideoName", video.VideoName);
            cmd.Parameters.AddWithValue("@VideoPath", video.VideoPath);
            cmd.Parameters.AddWithValue("@UploadTime", video.DateTime);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public ActionResult UploadVideo()
    {
        return View();
    }
}

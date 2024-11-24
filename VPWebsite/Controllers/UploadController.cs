using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using VPWebsite.Models;


public class UploadController : Controller
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

    [HttpPost]
    public ActionResult UploadVideo(HttpPostedFileBase videoFile, string videoTitle)
    {
        if (videoFile != null && videoFile.ContentLength > 0)
        {
            // set saving path
            string fileName = Path.GetFileName(videoFile.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/videos"), fileName);
            DateTime datetime = DateTime.Now;
            Video video = new Video
            {
                VideoTitle = videoTitle,
                VideoName = fileName,
                VideoPath = path,
                DateTime = datetime,
                User = (VPWebsite.Models.User.loginedUser != null) ? VPWebsite.Models.User.loginedUser.Id : 0,
            };
            // save file to server
            videoFile.SaveAs(path);

            // save info to database
            SaveVideoInfoToDatabase(video);

            GenerateThumbnail(video);

            ViewBag.Message = "Upload video successful!";
            ViewBag.thumbnailsTitle = video.VideoTitle;
            ViewBag.thumbnailsName = Path.GetFileNameWithoutExtension(video.VideoName) + ".jpg";

        }
        else
        {
            ViewBag.Message = "Please select a valid file!";
        }

        return View();
    }

    [HttpPost]
    public ActionResult UploadAvatar(HttpPostedFileBase avatarFile)
    {
        if (avatarFile != null && avatarFile.ContentLength > 0)
        {
            // set saving path
            string temfileName = Path.GetFileName(avatarFile.FileName);
            string id = VPWebsite.Models.User.loginedUser.Id.ToString();
            string fileName = GetHighestFileVersion(id, Server.MapPath("~/Content/avatars"), Path.GetExtension(avatarFile.FileName)) + Path.GetExtension(avatarFile.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/avatars"), fileName);
            Avatar avatar = new Avatar
            {
                AvatarName = fileName,
                AvatarPath = path,
            };
            // save avatar to server
            avatarFile.SaveAs(path);

            // save data to databese
            SaveAvatarInfoToDatabase(avatar);

            return Json(new { success = true, message = "Upload successful!" });
        }
        else
        {
            return Json(new { success = false, message = "Please select a valid file!" });
        }

    }

    private void SaveVideoInfoToDatabase(Video video)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO Videos (Title, VideoName, VideoPath, UploadTime, User) VALUES (@Title, @VideoName, @VideoPath, @UploadTime, @User)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Title", video.VideoTitle);
            cmd.Parameters.AddWithValue("@VideoName", video.VideoName);
            cmd.Parameters.AddWithValue("@VideoPath", video.VideoPath);
            cmd.Parameters.AddWithValue("@UploadTime", video.DateTime);
            cmd.Parameters.AddWithValue("@User", video.User);


            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private void SaveAvatarInfoToDatabase(Avatar avatar)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string updateQuery = "UPDATE Users SET avatars = @Avatar WHERE Username = @Username AND Password = @Password";
            MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
            cmd.Parameters.AddWithValue("@Avatar", avatar.AvatarName);
            cmd.Parameters.AddWithValue("@Username", VPWebsite.Models.User.loginedUser.Username);
            cmd.Parameters.AddWithValue("@Password", VPWebsite.Models.User.loginedUser.Password);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void GenerateThumbnail(Video video)
    {
        string thumbnailPath = Path.Combine(Server.MapPath("~/Content/thumbnails"), Path.GetFileNameWithoutExtension(video.VideoPath) + ".jpg");
        string ffmpegPath = Server.MapPath("~/ffmpeg/bin/ffmpeg.exe"); // FFmpeg path

        // FFmpeg command
        string args = $"-i \"{video.VideoPath}\" -ss 00:00:02 -vframes 1 -s 854x480 \"{thumbnailPath}\" -y";


        // call FFmpeg
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }).WaitForExit();
    }


    public string GetHighestFileVersion(string id, string directoryPath, string extension)
    {
        int index = 0;
        string filePath;

        while (true)
        {
            // generate file name
            string fileName = ((index == 0) ? $"{id}" : $"{id}_{index}") + extension;
            filePath = Path.Combine(directoryPath, fileName);

            // check exist
            if (!System.IO.File.Exists(filePath))
            {
                break;
            }

            index++;
        }

        return (index == 0) ? id : $"{id}_{index}";
    }

    public ActionResult UploadVideo()
    {
        return View();
    }

    public ActionResult UploadAvatar()
    {
        return View();
    }
}

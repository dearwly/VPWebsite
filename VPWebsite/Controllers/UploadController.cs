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

    [HttpPost]
    public ActionResult UploadAvatar(HttpPostedFileBase avatarFile)
    {
        if (avatarFile != null && avatarFile.ContentLength > 0)
        {
            // 设置保存文件的路径
            string temfileName = Path.GetFileName(avatarFile.FileName);
            string id = VPWebsite.Models.User.loginedUser.Id.ToString();
            string fileName = GetHighestFileVersion(id, Server.MapPath("~/Content/avatars"), Path.GetExtension(avatarFile.FileName)) + Path.GetExtension(avatarFile.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/avatars"), fileName);
            Avatar avatar = new Avatar
            {
                AvatarName = fileName,
                AvatarPath = path,
            };
            // 保存文件到服务器
            avatarFile.SaveAs(path);

            // 将文件信息存储到数据库
            SaveAvatarInfoToDatabase(avatar);

            return Json(new { success = true, message = "上传成功！" });
        }
        else
        {
            return Json(new { success = false, message = "请选择一个有效的文件！" });
        }

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

    public string GetHighestFileVersion(string id, string directoryPath, string extension)
    {
        int index = 0;
        string filePath;

        while (true)
        {
            // 生成文件名
            string fileName = ((index == 0) ? $"{id}" : $"{id}_{index}") + extension;
            filePath = Path.Combine(directoryPath, fileName);

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                break; // 如果文件不存在，则退出循环
            }

            index++; // 检查下一个编号的文件
        }

        // 返回最后一个存在的文件路径（如果没有找到则返回 null）
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

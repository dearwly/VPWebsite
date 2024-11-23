using System.Web.Mvc;
using System;
using VPWebsite.Models;
using PagedList;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

public class VideoController : Controller
{
    // 用于显示视频信息和播放视频
    public ActionResult Play(int videoId)
    {
        try
        {
            // 从数据库获取视频信息
            var video = Video.GetVideoById(videoId);

            if (video == null)
            {
                return HttpNotFound();
            }

            // 获取视频发布者信息
            var username = VPWebsite.Models.User.GetUsername(video.User);

            // 将视频和用户信息传递到视图
            var viewModel = new VideoView
            {
                Video = video,
                UserName = username,
                FormattedUploadTime = video.DateTime.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // 处理任何异常
            ViewBag.Message = "获取视频信息时出错：" + ex.Message;
            return View();
        }
    }



    // 删除视频的方法
    [HttpPost]
    public ActionResult DeleteVideo(int videoId)
    {
        try
        {
            // 删除视频
            var success = DeleteVideoById(videoId);
            if (success)
            {
                ViewBag.Message = "视频删除成功！";
            }
            else
            {
                ViewBag.Message = "删除视频失败！";
            }
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Message = "删除视频时出错：" + ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }

    // 删除视频的数据库操作
    private bool DeleteVideoById(int videoId)
    {
        // 这里执行删除操作
        // 例如，删除数据库中的视频记录
        return true;
    }



    
}

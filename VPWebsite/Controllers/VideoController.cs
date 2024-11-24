using System.Web.Mvc;
using System;
using VPWebsite.Models;
using PagedList;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

public class VideoController : Controller
{
    // To show video info and play video
    public ActionResult Play(int videoId)
    {
        try
        {
            // get content from mysql
            var video = Video.GetVideoById(videoId);

            if (video == null)
            {
                return HttpNotFound();
            }

            // get publisher
            var username = VPWebsite.Models.User.GetUsername(video.User);

            // push video and user to view
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
            // catch exception
            ViewBag.Message = "Error getting video:" + ex.Message;
            return View();
        }
    }




    [HttpPost]
    public ActionResult DeleteVideo(int videoId)
    {
        try
        {
            // delete Video
            var success = Video.DeleteVideoById(videoId);
            if (success)
            {
                ViewBag.Message = "Video deleted successfully";
            }
            else
            {
                ViewBag.Message = "Fail to delete video";
            }
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Error deleting video:" + ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }



    
}

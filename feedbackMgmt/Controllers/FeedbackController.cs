using Microsoft.AspNetCore.Mvc;
using feedbackMgmt.Models;
using feedbackMgmt;

namespace mvcCRUDstudio.Controllers
{
    public class FeedbackController : Controller
    {
        
        public ActionResult Index()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            feedbacks = DBManager.GetFeedbacks();
            ViewData["feedbacks"] = feedbacks;
            return View(feedbacks);
        }

        
        public ActionResult Details(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback newFeedback)
        {
            try
            {
                DBManager.Insert(newFeedback);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       
        public ActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Feedback updateFeedback) 
        {
            try
            {
                ViewBag.feedback = updateFeedback;
                DBManager.Update(updateFeedback);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult Delete(int id)
        {
            ViewData["id"] = id;
            DBManager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
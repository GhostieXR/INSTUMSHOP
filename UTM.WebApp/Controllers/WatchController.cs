using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using UTM.Database;
using UTM.Domain;
using UTM.WebApp.Attr;

namespace UTM.WebApp.Controllers
{
    [Auth("Moderator", "Admin")]
    public class WatchController : Controller
    {
        private WatchStoreContext db = new WatchStoreContext();

        public ActionResult Index()
        {
            return View(db.Watches.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WatchId,Name,Brand,Price,ImageUrl")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                db.Watches.Add(watch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(watch);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WatchId,Name,Brand,Price,ImageUrl")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(watch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(watch);
        }

        [Auth("Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        [Auth("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Watch watch = db.Watches.Find(id);
            db.Watches.Remove(watch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

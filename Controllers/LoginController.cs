using MagicWarehouse.Data;
using System.Linq;
using System.Web.Mvc;

namespace MagicWarehouse.Controllers
{
    public class LoginController : Controller
    {
        private MagicEntities db = new MagicEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public ActionResult Index(User login)
        {
            var user = db.A_Users
                .FirstOrDefault(x => x.username == login.Username && x.Password == login.Password);

            if (user != null)
            {
                // Successful login
                return RedirectToAction("Index","Home");
            }
            else
            {
                // Invalid login, return to the login page
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
    
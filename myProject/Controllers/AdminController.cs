using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using Microsoft.AspNetCore.Authorization;


namespace myProject.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        _AdminDatabaseControlModel _adminDatabaseControlModel;


        public AdminController(_AdminDatabaseControlModel adminDatabaseControlModel)
        {
            _adminDatabaseControlModel = adminDatabaseControlModel;
        }




        /* --------------------------------------------------- Dashboard Page --------------------------------------------------- */
        public IActionResult Index()
        {
            ModelForAdminPages modelForAdminPages = new ModelForAdminPages();
            modelForAdminPages = _adminDatabaseControlModel.Dashboard();

            ViewBag.notActivated = _adminDatabaseControlModel.NotActivated();

            return View(modelForAdminPages);
        }


        /* --------------------------------------------------- Companies Page --------------------------------------------------- */
        [HttpGet]
        public IActionResult Companies()
        {
            ModelForAdminPages combinedViewModelList = _adminDatabaseControlModel.getAllCompanies();
            ViewBag.notActivated = _adminDatabaseControlModel.NotActivated();
            return View(combinedViewModelList);
        }


        /* --------------------------------------------------- Activate Accounts Page --------------------------------------------------- */
        public IActionResult ActivateAccounts()
        {
            ModelForAdminPages combinedViewModelList = _adminDatabaseControlModel.getNotAcitavedCompanies();
            ViewBag.notActivated = _adminDatabaseControlModel.NotActivated();
            return View(combinedViewModelList);
        }



        [HttpPost]

        public async Task<IActionResult> ActivateAccountSubmit(int companyId)
        {

            _adminDatabaseControlModel.ActivateAccount(companyId);
            return RedirectToAction("ActivateAccounts");
        }


        /* --------------------------------------------------- Users Page --------------------------------------------------- */
        [HttpGet]
        public IActionResult Users()
        {
            ModelForAdminPages modelForAdminPages = new ModelForAdminPages();
            modelForAdminPages.users = _adminDatabaseControlModel.getAllUsers();
            ViewBag.notActivated = _adminDatabaseControlModel.NotActivated();
            return View(modelForAdminPages);
        }



        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            _adminDatabaseControlModel.DeleteUser(userId);
            return RedirectToAction("Users");
        }




        /* --------------------------------------------------- Products --------------------------------------------------- */
        [HttpGet]
        public IActionResult Products()
        {
            ModelForAdminPages modelForAdminPages = new ModelForAdminPages();
            modelForAdminPages.products = _adminDatabaseControlModel.getAllProducts();
            ViewBag.notActivated = _adminDatabaseControlModel.NotActivated();
            return View(modelForAdminPages);
        }


        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            _adminDatabaseControlModel.DeleteProduct(productId);
            return RedirectToAction("Products");
        }




        /* --------------------------------------------------- Reviews --------------------------------------------------- */
        [HttpGet]
        public IActionResult Reviews()
        {
            ModelForAdminPages modelForAdminPages = new ModelForAdminPages();
            modelForAdminPages.reviews = _adminDatabaseControlModel.GetAllReviews();
            ViewBag.notActivated = _adminDatabaseControlModel.NotActivated();
            return View(modelForAdminPages);
        }


        [HttpPost]
        public IActionResult DeleteReview(int reviewId)
        {
            _adminDatabaseControlModel.DeleteReview(reviewId);
            return RedirectToAction("Reviews");
        }



        /* --------------------------------------------------- Navbar --------------------------------------------------- */
        public IActionResult Signout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index","Guest");
          
        }





    }
}

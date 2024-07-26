using Microsoft.AspNetCore.Mvc;
using myProject.Models;

namespace myProject.Controllers
{
    public class AdminController : Controller
    {
        _AdminDatabaseControlModel _adminDatabaseControlModel = new _AdminDatabaseControlModel();

        /* --------------------------------------------------- Dashboard Page --------------------------------------------------- */
        public IActionResult Index()
        {
            return View();
        }


        /* --------------------------------------------------- Companies Page --------------------------------------------------- */
        [HttpGet]
        public IActionResult Companies()
        {
            List<ModelForAdminPages> combinedViewModelList = _adminDatabaseControlModel.getAllCompanies();

           return View(combinedViewModelList);
        }


        /* --------------------------------------------------- Activate Accounts Page --------------------------------------------------- */
        public IActionResult ActivateAccounts()
        {
            List<ModelForAdminPages> combinedViewModelList = _adminDatabaseControlModel.getNotAcitavedCompanies();

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

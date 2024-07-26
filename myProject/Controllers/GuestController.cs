using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using System.Diagnostics;

namespace myProject.Controllers
{
    public class GuestController : Controller
    {
       

        private readonly _UserDatabaseControlModel userDatabaseControlModel = new _UserDatabaseControlModel();


        public IActionResult Index()
        {

            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.HomePageProducts();

            ModelForUserPages.companies = userDatabaseControlModel.GetAllCompanies();




            return View(modelForUserPages);
        }



    }
}

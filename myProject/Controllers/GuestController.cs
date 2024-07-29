using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace myProject.Controllers
{
    public class GuestController : Controller
    {


        private readonly _UserDatabaseControlModel userDatabaseControlModel;



        public GuestController(_UserDatabaseControlModel userDatabaseControlModel_)
        {
            userDatabaseControlModel = userDatabaseControlModel_;
        }


        public IActionResult Index()
        {

            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.HomePageProducts();

            ModelForUserPages.companies = userDatabaseControlModel.GetAllCompanies();



            return View(modelForUserPages);
        }


        /* ------------------------------------------------------------------------------------------ */
        /* Ürüne tıklanınca açılan sayfa */
        [HttpGet("Guest/ProductDetails/{productId}")]
        public IActionResult ProductDetails(int productId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.ProductDetail(productId);


            return View(modelForUserPages);
        }


        /* -------------------------------------------------------------------------------------------------------------- */
        /* Şirketin sayfası */
        [HttpGet("Guest/CompanyDetails/{companyId}")]
        public IActionResult CompanyDetails(int companyId)
        {
            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages = userDatabaseControlModel.CompanyDetails(companyId, 0);

            return View(modelForUserPages);
        }




        /* -------------------------------------------------------------------------------------------------------------- */
        /* Kategoriye göre ürün listeleme (filter) */
        public IActionResult ProductsByCategory(string categoryyy)
        {

            ModelForUserPages modelForUserPages = new ModelForUserPages();
            modelForUserPages.subcategory = categoryyy;

            if (categoryyy == "Furniture") { modelForUserPages.productsByCategory = userDatabaseControlModel.GetSubcategoriesByMainCategory(categoryyy); }
            else if (categoryyy == "Decoration") { modelForUserPages.productsByCategory = userDatabaseControlModel.GetSubcategoriesByMainCategory(categoryyy); }
            else if (categoryyy == "all") modelForUserPages.productsByCategory = userDatabaseControlModel.getAllProducts();
            else { modelForUserPages.productsByCategory = userDatabaseControlModel.GetProductsByCategory(categoryyy); }

            return View(modelForUserPages);
        }



    }
}

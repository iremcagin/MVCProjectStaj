using Microsoft.AspNetCore.Mvc;
using myProject.Models;

namespace myProject.Controllers
{
    public class AdminController : Controller
    {
        _AdminDatabaseControlModel _adminDatabaseControlModel = new _AdminDatabaseControlModel();


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Companies()
        {
            List<CombinedViewModel> combinedViewModelList = _adminDatabaseControlModel.getAllCompanies();

           return View(combinedViewModelList);
        }


        public IActionResult ActivateAccounts()
        {
            List<CombinedViewModel> combinedViewModelList = _adminDatabaseControlModel.getNotAcitavedCompanies();

            return View(combinedViewModelList);
        }



        [HttpPost]

        public async Task<IActionResult> ActivateAccountSubmit(int companyId)
        {

            _adminDatabaseControlModel.ActivateAccount(companyId);
            return RedirectToAction("ActivateAccounts");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using System.Reflection;

namespace myProject.Controllers
{
    public class LoginController : Controller
    {
        _LoginDatabaseControlModel _loginDatabaseControlModel = new _LoginDatabaseControlModel();

        public IActionResult Index()
        {
            return View();
        }





        /* ------------------------------------- LOGIN ------------------------------------- */
        [HttpPost]
        public async Task<IActionResult> Login(CombinedViewModel model)
        {
            UserModel loggedUser = new UserModel();
            

            try
            {
                loggedUser = _loginDatabaseControlModel.Login(model.User);
                string role = loggedUser.Role;
             

                if (string.IsNullOrEmpty(loggedUser.Role))
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", loggedUser.UserId);

                    if (role == "admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if(role == "seller")
                    {
                        /* Hesap onaylanmış mı diye kontrol et */
                        if(_loginDatabaseControlModel.CheckIfAccountActivated(loggedUser.UserId)) return RedirectToAction("Index", "Seller");
                        else
                        {
                            TempData["ErrorMessage"] = "Account has not been activated yet.";
                            return RedirectToAction("Index");
                        }
                    }
                    else if(role == "user")
                    {
                        return RedirectToAction("Index", "User");
                    }
                }
              
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to login: " + ex.Message;
                return RedirectToAction("Index");
            }
        }




        /* ------------------------------------- USER SIGN UP ------------------------------------- */
        [HttpPost]
        public async Task<IActionResult> UserSignUp(CombinedViewModel model)
        {

            UserModel userModel = model.User;

            try
            {
                 _loginDatabaseControlModel.UserSignUp(userModel);
                //_loginDatabaseControlModel.CalculateAge(model.Birthdate);

                return RedirectToAction("Index", "Guest");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to sign up: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        /* ------------------------------------- COMPANY SIGN UP ------------------------------------- */
        /* Model for Logo and Banner */
       

        [HttpPost]
        public async Task<IActionResult> CompanySignUp(CombinedViewModel model)
        {
                  string _logosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/_Logos");
                  string _bannersPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/_Banners");

            try
            {
                /* Save the Logo to the folder */
                if (model.Logo != null)
                {
                    var logoFileName = $"{model.Company.CompanyName}_logo{Path.GetExtension(model.Logo.FileName)}";
                    var logoPath = Path.Combine(_logosPath, logoFileName);


                    using (var stream = new FileStream(logoPath, FileMode.Create))
                    {
                        await model.Logo.CopyToAsync(stream);
                    }

                    model.Company.LogoUrl = logoFileName;
                }
                else { model.Company.LogoUrl = "wwwroot/images/placeholder-1.png"; }
                /* Save the Banner to the folder */
                if (model.Banner != null)
                {
                    var bannerFileName = $"{model.Company.CompanyName}_banner{Path.GetExtension(model.Banner.FileName)}";
                    var bannerPath = Path.Combine(_bannersPath, bannerFileName);


                    using (var stream = new FileStream(bannerPath, FileMode.Create))
                    {
                        await model.Banner.CopyToAsync(stream);
                    }

                    model.Company.BannerUrl = bannerFileName;
                }
                else { model.Company.BannerUrl = "wwwroot/images/placeholder-1.png"; }
                _loginDatabaseControlModel.CompanySignUp(model);


                return RedirectToAction("Index", "Guest");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to sign up: " + ex.Message;
                return RedirectToAction("Index");
            }
        }




    }
}

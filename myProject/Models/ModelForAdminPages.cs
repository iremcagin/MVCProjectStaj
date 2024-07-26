using myProject.Models;

namespace myProject.Models
{
    public class ModelForAdminPages
    {
        public UserModel User { get; set; }
        public CompanyModel Company { get; set; }

        public IFormFile Logo { get; set; }
        public IFormFile Banner { get; set; }



        // Users Page için
        public List<UserModel> users { get; set; } = new List<UserModel>();



    }

}


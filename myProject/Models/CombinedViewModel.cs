using myProject.Models;

namespace myProject.Models
{
    public class CombinedViewModel
    {
        public UserModel User { get; set; }
        public CompanyModel Company { get; set; }

        public IFormFile Logo { get; set; }
        public IFormFile Banner { get; set; }
    }

}


using System.ComponentModel.DataAnnotations;

namespace OdeToFood.ViewModels
{
    public class LoginViewModel
    {
        [Required, Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string returnUrl { get; set; }
        //cia kad sugristu ten kur buvo kai prisiligins
    }
}

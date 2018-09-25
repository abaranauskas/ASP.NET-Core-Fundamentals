using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.ViewModels
{
    public class RegistrationViewModel
    {
        
        [Required, MaxLength(256)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        //cia mes nedarom ilgio apribojimu ir pan identy padarys uz mus kai perduosim
        public string ConfirmPassword { get; set; }
    }
}

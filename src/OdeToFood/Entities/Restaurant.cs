using System;
using System.ComponentModel.DataAnnotations;

namespace OdeToFood.Entities
{
    public enum CuisineType
    {
        None,
        American,
        Thai,
        Japanese,
        Italian
    }

    public class Restaurant
    {
        public int Id { get; set; }
        
        [Required, MaxLength(80)]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; }
        public CuisineType Cuisine { get; set; }        
    }
}

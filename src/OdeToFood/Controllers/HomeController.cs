using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Entities;
using OdeToFood.Services;
using OdeToFood.ViewModels;
using System.Collections.Generic;

namespace OdeToFood.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        //private IGreeter _greeter;
        private IRestaurantData _restaurantData;

        public HomeController(IRestaurantData restaurantData/*, IGreeter greeter*/)
        {
            _restaurantData = restaurantData;
            //_greeter = greeter;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _restaurantData.GetAll();

            //var model = new HomePageViewModel();
            //model.Restaurants = _restaurantData.GetAll();
            //model.CurrentMessage = _greeter.GetGreeting();

            return View(model);


            //sterilizuoja onjecta ir siuncia json 
            //return new ObjectResult(model);

            //return Content("Sveiki visi is HomeController");
            //Conten(); returnina ContentResult tipa o jis implemetina
            //IActionResult kaip ir visi kiti tipai sutinkami sitame kontekste

        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
                //redirektina i virsui esanti action Index
                //jei tokio id nera
            }
            return View(model);
        }

        
        [HttpGet]  //cia kai uzkraut formai
        public IActionResult Create()
        {
            return View();
        }


        //sitas actionas reikalingas pridet nauja retorana. padarem nauja tipa
        //restaurantNieMOdel tam kad galimai nepakenti Restaurant
        [HttpPost]  //cia kai pasiimt formos duomenis
        [ValidateAntiForgeryToken] //cia atpazinimas pagal cookies. pasiaiskinti dar
        public IActionResult Create(RestaurantEditViewModel model)
        {
            if (ModelState.IsValid)  //sitas butinas kad pagautu kur dataanotations reikalavimus
            {
                var newRestaurant = new Restaurant();
                newRestaurant.Name = model.Name;
                newRestaurant.Cuisine = model.Cuisine;

                newRestaurant = _restaurantData.Add(newRestaurant);
                _restaurantData.Commit();
                //return View("Details", newRestaurant); //nugreipia tiesiai i view  is pradziu scottas rode sita buda bet negerai jis. nes kai sukuria jieka ant to paties action ir refresinat galima dumblikuoti 
                
                //Post - Redirect - get pattern
                return RedirectToAction(nameof(Details), new { id = newRestaurant.Id }); //nukreipia i kontroleri pirmiau buvau sumastes pries parodant
            }

            return View();  //grazina kai nevalidu su ta pacia forma plius esrro messages
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _restaurantData.Get(id);
            if (model==null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RestaurantEditViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var editRestaurant = _restaurantData.Get(id);
                editRestaurant.Name = model.Name;
                editRestaurant.Cuisine = model.Cuisine;
                _restaurantData.Commit();

                return RedirectToAction("Details", new {id=editRestaurant.Id });
            }

            return View(_restaurantData.Get(id));
        }
    }
}

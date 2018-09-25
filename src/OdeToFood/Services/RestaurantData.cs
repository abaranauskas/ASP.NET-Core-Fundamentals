using System.Collections.Generic;
using System;
using OdeToFood.Entities;
using System.Linq;

namespace OdeToFood.Services
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int id);
        Restaurant Add(Restaurant newRestaurant);
        void Commit();
    }

    public class SqlRestaurantData : IRestaurantData
    {
        private OdeToFoodDbContext _context;

        public SqlRestaurantData(OdeToFoodDbContext context)
        {
            _context = context;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            _context.Add(newRestaurant);           
            return newRestaurant;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public Restaurant Get(int id)
        {
            return _context.Restaurants.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _context.Restaurants;
        }
    }


    public class InMemoryRestaurantData : IRestaurantData
    {
        static List<Restaurant> _restaurants;
        static InMemoryRestaurantData()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant { Id=1, Name="Tavenrn Too"},
                new Restaurant { Id=2, Name="House of kobe"},
                new Restaurant { Id=3, Name="P.J. Lick"},
                new Restaurant { Id=4, Name="Lazy gecko"}
            };                
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants;
        }

        public Restaurant Get(int id)
        {
            //trumpesnis pagal tutorial
            return _restaurants.FirstOrDefault(r => r.Id == id);

            //mano sprendimas neziurejus
            //foreach (var restaurant in _restaurants)
            //{
            //    if (restaurant.Id == id) return restaurant;
            //}
            //return null;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            //newRestaurant.Id = _restaurants.Count+1; //mano budas 

            newRestaurant.Id = _restaurants.Max(r=>r.Id)+1; //Scott budas
            _restaurants.Add(newRestaurant);
            return newRestaurant;
        }

        public void Commit()
        {
            // no operatio
        }
    }
}

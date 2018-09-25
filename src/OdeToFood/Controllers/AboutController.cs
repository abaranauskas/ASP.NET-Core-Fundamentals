using Microsoft.AspNetCore.Mvc;

namespace OdeToFood.Controllers
{
    //atributte based routing
    //[Route("about")]
    [Route("company/[controller]/[action]")]
    public class AboutController
    {
        //[Route("")] //paliekam tucia ir ateis default i sita action
        //su salyga jei kiti actionai sitam controller ture atributes ne su tusciom ""
        public string Phone()
        {
            return "+370-678-47388";
        }

        //[Route("[action]")] //[] token naudojami kaiaction name ir 
        //norima routin sutampa. pakeisi action name nereikes keit route tarp kabuciu
        public string Address()
        {
            return "Vilnius, Lithuania";
        }
    }
}

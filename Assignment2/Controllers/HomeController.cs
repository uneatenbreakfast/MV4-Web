using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "M";

            using (EmployeesContext db = new EmployeesContext())
            {
               /* int j = db.Employees.Count();
                ViewBag.Message = String.Format("We have {0} records.", j);
                var data = db.Employees.ToList();
                return View(data);*/

                var cabincrew = 
                    from e in db.crew 
                    from p in db.personDetails
                    from f in db.flight
                    from a in db.aircraftDetails
                    from at in db.airType
                    from r in db.routeDetails
                    from ap in db.airportDetails
                    from ap2 in db.airportDetails
                    where 
                    e.cabinCrewId == p.id &&
                    f.id == e.flightId &&
                    f.aircraft == a.id &&
                    a.aircraftType == at.id &&
                    f.route == r.id &&
                    r.fromAirport == ap.id &&
                    r.toAirport == ap2.id
                    
                   // orderby e.Surname 
                    select new crewGrid { 
                        name = p.name,
                        flightDay = f.flightDay,
                        aircraftModel = at.model,
                        fromAirport = ap.code,
                        toAirport = ap2.code,
                        startDate = e.startDate

                    };

               cabincrew.ToList();

               return View(cabincrew.ToList());

            }
           //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult newCabinAssignment()
        {
            ViewBag.Message = "Your contact page.x";

            Employee e = new Employee();
            return View(e);
        }

        [HttpPost]
        public ActionResult newCabinAssignment(Employee e)
        {
            if (ModelState.IsValid) {
                using (EmployeesContext db = new EmployeesContext()){
                    db.crew.Add(e);

                    System.Diagnostics.Debug.WriteLine("---------------------------");
                    System.Diagnostics.Debug.WriteLine(e.cabinCrewId);
                    System.Diagnostics.Debug.WriteLine(e.flightId);
                    System.Diagnostics.Debug.WriteLine(e.startDate);

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.Message="You got an error.";
            // only get here if invalid
            return View(e);
        }
    }
}

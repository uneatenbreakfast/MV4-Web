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
                return View(data);


                 var results = from r in Ratings
              join u1 in Users on u1.userid = r.rated
              join u2 in Users on u2.userid = r.rater
              join cm in ClassMembers on cm.userid = r.rated
              join c in Class on cm.teamid = c.teamid
              join s in Scores on s.ratingsid = r.ratingsid
              join sbj in Subjects on sbj.subjectid = s.subjectid
              select new 
                     {
                        Date = r.date, 
                        Rated = u1.username,
                        Rater = u2.username,
                        ClassName = c.name,
                        Ratings = s.ratings,
                        Subject = sbj.name
                      };
                */

                var cabincrew = from e in db.crew 
                    join p in db.personDetails on e.cabinCrewId equals p.id 
                    join f in db.flight on e.flightId equals f.id
                    join a in db.aircraftDetails on f.aircraft equals a.id
                    join at in db.airType on a.aircraftType equals at.id
                    join r in db.routeDetails on f.route equals r.id
                    join ap in db.airportDetails on r.fromAirport equals ap.id
                    join ap2 in db.airportDetails on r.toAirport equals ap2.id
                
                   // orderby e.Surname 
                    select new crewGrid { 
                        name = p.name,
                        flightDay = f.flightDay,
                        aircraftModel = at.model,
                        fromAirport = ap.code,
                        toAirport = ap2.code,
                        startDate = e.startDate

                    };


                  /*
                 * 
                 *  var cabincrew = 
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
                 * 
                 * */

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

            using (EmployeesContext db = new EmployeesContext())
            {
                var p = db.personDetails.ToList();
                ViewBag.list = new SelectList(p.ToList(), "id", "name");               
            }            

            Employee e = new Employee();
            return View(e);
        }

        [HttpPost]
        public ActionResult newCabinAssignment(Employee e)
        {
            if (ModelState.IsValid) {
                using (EmployeesContext db = new EmployeesContext()){
                    db.crew.Add(e);
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

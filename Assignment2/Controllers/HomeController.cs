using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment2.Models;
using System.Data;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Message = "M";

           using (EmployeesContext db = new EmployeesContext())
            {
                var cabincrew = from e in db.crew
                                join p in db.personDetails on e.cabinCrewId equals p.id
                                join f in db.flight on e.flightId equals f.id
                                join a in db.aircraftDetails on f.aircraft equals a.id
                                join at in db.airType on a.aircraftType equals at.id
                                join r in db.routeDetails on f.route equals r.id
                                join ap in db.airportDetails on r.fromAirport equals ap.id
                                join ap2 in db.airportDetails on r.toAirport equals ap2.id
                                select new crewGrid
                                {
                                    name = p.name,
                                    flightDay = f.flightDay,
                                    aircraftModel = at.model,
                                    fromAirport = ap.code,
                                    toAirport = ap2.code,
                                    startDate = e.startDate,
                                    crewId = e.cabinCrewId,
                                    flightId = f.id
                                };
                return View(cabincrew.ToList());
            }  
            
        }



        private IQueryable<crewGrid> getStaffFlights()
        {

            using (EmployeesContext db = new EmployeesContext())
            {
                var cabincrew = from e in db.crew
                                join p in db.personDetails on e.cabinCrewId equals p.id
                                join f in db.flight on e.flightId equals f.id
                                join a in db.aircraftDetails on f.aircraft equals a.id
                                join at in db.airType on a.aircraftType equals at.id
                                join r in db.routeDetails on f.route equals r.id
                                join ap in db.airportDetails on r.fromAirport equals ap.id
                                join ap2 in db.airportDetails on r.toAirport equals ap2.id
                                select new crewGrid
                                {
                                    name = p.name,
                                    flightDay = f.flightDay,
                                    aircraftModel = at.model,
                                    fromAirport = ap.code,
                                    toAirport = ap2.code,
                                    startDate = e.startDate,
                                    crewId = e.cabinCrewId,
                                    flightId = f.id
                                };
                //return View(cabincrew.ToList());
                return cabincrew;
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "All about this assignment";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Get in touch";

            return View();
        }

        public ActionResult DeleteCrewFlight(int cId, int fId)
        {
            using (EmployeesContext db = new EmployeesContext())
            {
                Employee e = db.crew.Find(cId, fId);
                if (e == null)
                    return HttpNotFound(); // it really should be found, unless
                // the user edited the URL string

                    var cabincrew = from ex in db.crew
                                    join p in db.personDetails on cId equals p.id
                                    join f in db.flight on fId equals f.id
                                    join a in db.aircraftDetails on f.aircraft equals a.id
                                    join at in db.airType on a.aircraftType equals at.id
                                    join r in db.routeDetails on f.route equals r.id
                                    join ap in db.airportDetails on r.fromAirport equals ap.id
                                    join ap2 in db.airportDetails on r.toAirport equals ap2.id
                                    select new crewGrid
                                    {
                                        name = p.name,
                                        flightDay = f.flightDay,
                                        aircraftModel = at.model,
                                        fromAirport = ap.code,
                                        toAirport = ap2.code,
                                        startDate = e.startDate,
                                        crewId = e.cabinCrewId,
                                        flightId = f.id
                                    };

                    crewGrid c = cabincrew.First();
                    return View(c);
            } 
        }

        [HttpPost]
        public ActionResult DeleteCrewFlight(int cId, int fId, int? jj)
        {
            using (EmployeesContext db = new EmployeesContext())
            {
                Employee ex = db.crew.Find(cId, fId);
                db.Entry(ex).State = EntityState.Deleted; 
                db.SaveChanges();

                return RedirectToAction("Index");
            }            
        }


        public ActionResult newCabinAssignment()
        {
            using (EmployeesContext db = new EmployeesContext())
            {
                var crew = GetAllCrew();
                ViewBag.list = new SelectList(crew.ToList(), "id", "name");

                var flightlist = GetAllFlights();
                ViewBag.flist = new SelectList(flightlist.ToList(), "id", "flightmes");  
            }            

            Employee ex = new Employee();
            return View(ex);
        }

        public ActionResult QualificationCrew()
        {
            using (EmployeesContext db = new EmployeesContext())
            {
                

                var cq = (from c in db.allcrew
                          join p in db.personDetails on c.person equals p.id
                          join a in db.airType on c.forAircraftType equals a.id
                          select new
                          {
                              name = p.name,
                              aircrafts = a.model
                          }).ToList().GroupBy(user => user.name);

                List<qualifyGrid> allData = new List<qualifyGrid>();
                foreach (var group in cq)
                {
                    string jData = "";
                    foreach (var entry in group)
                    {
                        jData += entry.aircrafts + " , ";
                    }

                    jData = jData.Substring(0,jData.Length -3);

                    qualifyGrid q = new qualifyGrid{
                        name = group.Key,
                        aircrafts = jData
                    };
                    allData.Add(q);                    
                }
                

                return View(allData.OrderBy(x => x.name).ToList());
            }
        }

        private List<flightAssignmentCrewList> GetAllCrew()
        {
            using (EmployeesContext db = new EmployeesContext())
            {
                var crew = (from e in db.allcrew
                            join p in db.personDetails on e.person equals p.id
                            select new flightAssignmentCrewList
                            {
                                id = e.person,
                                name = p.name
                            }).Distinct().OrderBy(c => c.name);

                return crew.ToList();
            }
        }

        private List<flightslist> GetAllFlights()
        {
            using (EmployeesContext db = new EmployeesContext())
            {
                var dayIndex = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                var flightlist = (from fl in db.flight
                                  join r in db.routeDetails on fl.route equals r.id
                                  join a in db.airportDetails on r.fromAirport equals a.id
                                  join a2 in db.airportDetails on r.toAirport equals a2.id
                                  select new
                                  {
                                      idx = fl.id,
                                      f = fl.flightDay,
                                      a1 = a.code,
                                      a2 = a2.code
                                  }).AsEnumerable().Select(x => new flightslist()
                                  {
                                      id = x.idx,
                                      day = x.f,
                                      fromairport = x.a1,
                                      flightmes = String.Format("{0} | {1} => {2}", x.f, x.a1, x.a2)
                                  }).OrderBy(c => dayIndex.IndexOf(c.day)).ThenBy(c => c.fromairport);

                return flightlist.ToList();
            }
        }

        [HttpPost]
        public ActionResult newCabinAssignment(Employee e)
        {

            if (ModelState.IsValid)
            {
                using (EmployeesContext db = new EmployeesContext()){

                    var isQualified = (
                                from cc in db.allcrew
                                from att in db.flight
                                where 
                                cc.person == e.cabinCrewId &&
                                att.id == e.flightId &&
                                cc.forAircraftType == att.aircraft
                                select new {
                                    cc.person
                                }).Count();

                    if (isQualified > 0)
                    {
                        var isDuplicate = (
                               from ee in db.crew
                               where
                               ee.cabinCrewId == e.cabinCrewId &&
                               ee.flightId == e.flightId
                               select new
                               {
                                   ee.cabinCrewId
                               }).Count();

                        if (isDuplicate == 0)
                        {
                            db.crew.Add(e);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Message = "Crew member already assigned to flight";
                        } 
                    }
                    else
                    {
                        ViewBag.Message = "Crew member not qualified for the specified flight";
                    }   
                }
            } 

            var crew = GetAllCrew();
            ViewBag.list = new SelectList(crew.ToList(), "id", "name");

            var flightlist = GetAllFlights();
            ViewBag.flist = new SelectList(flightlist.ToList(), "id", "flightmes");  

            Employee ex = new Employee();
            return View(ex); 
        }
    }
}

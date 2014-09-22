using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Assignment2.Models
{
    
    public class EmployeesContext : DbContext{
        
        public EmployeesContext() : base("MySqlConnection"){}
        public DbSet<Employee> crew { get; set; } 
        public DbSet<Person> personDetails { get; set; }
        public DbSet<Flight> flight { get; set; }
        public DbSet<Aircraft> aircraftDetails {get;set;}
        public DbSet<AircraftType> airType { get; set; }
        public DbSet<Route> routeDetails { get; set; }
        public DbSet<Airport> airportDetails { get; set; }
    }


    /*
     *  Person name 
     *  The flight day (i.e. “Monday” or such) 
     *  The “From” airport for the route (show the 3-digit code, e.g. “AKL”) 
     *  The “To” airport for the route (also as 3-digit code)
     *  The aircraft model for that route (e.g. “A380”) 
     *  The start date of that person’s assignment to crew that route in yyyy-mm-dd format 
     * 
     * 
     * 
     * */


    public class crewGrid { 
        public string name { get; set; } 
        public string flightDay { get; set; } 
        public string fromAirport { get; set; } 
        public string toAirport { get; set; } 
        public string aircraftModel { get; set; }
        public DateTime startDate { get; set; }
    }

    [Table("Aircraft")]
    public class Aircraft
    {
        [Key]
        public int id { get; set; }
        public int aircraftType { get; set; }
    }
     [Table("AircraftType")]
    public class AircraftType
    {
        [Key]
        public int id { get; set; }
        public string model { get; set; }
    }

    [Table("Route")]
     public class Route
     {
        [Key]
        public int id { get; set; }
        public int fromAirport{get;set;}
        public int toAirport{get;set;}
        public int distance{get;set;}
     }

    [Table("Airport")]
    public class Airport
    {
        [Key]
        public int id { get; set; }
        public int city { get; set; }
        public string code { get; set; }
    }

    
    [Table("flight")]
    public class Flight
    {
        [Key]
        public int id { get; set; }
        public int route { get; set; }
        public string flightDay { get; set; }
        public int aircraft { get; set; }
    }

    [Table("Person")]
    public class Person
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }





    [Table("FlightCabinCrew")]
    public class Employee
    {
        private DateTime d;

        [Key]
        public int cabinCrewId { get; set; }
        public int flightId { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateAttributeLow]
        public DateTime startDate
        {

            get { return (d.Day > 1 ? d : DateTime.Now); }
            set { d = value; }
        }
    }



  

    public class DateAttributeLow : RangeAttribute { 
        public DateAttributeLow() : base(typeof(DateTime),
            DateTime.Now.ToShortDateString(),
            DateTime.MaxValue.ToShortDateString()) { 
            this.ErrorMessage = "Start date cannot be earlier than today"; 
        } 
    }
}
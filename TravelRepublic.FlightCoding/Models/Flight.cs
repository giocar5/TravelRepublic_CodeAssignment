using System.Collections.Generic;
namespace TravelRepublic.FlightCoding
{
    //base class Flight
    public class Flight
    {
        public IList<Segment> Segments { get; set; }

    }

    public class BookableFlight : Flight
    {
        public double Price {get;set;}

        public int CompanyId {get;set;}

        public int AvailableSeats {get;set;}

    }

}
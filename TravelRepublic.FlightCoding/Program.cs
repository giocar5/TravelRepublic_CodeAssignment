using System;
using System.Collections.Generic;
using System.Linq;
using  TravelRepublic.FlightCoding.Extensions;

namespace TravelRepublic.FlightCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            FlightProvider provider; 
            //TODO: Loading Provider Type from config
            provider = new FlightBuilder();
            //TODO: Loading criteria default Attributes from config
            FlightSearchCriteria criteria = new FlightSearchCriteria();
            provider.FlightSearchCriteria = criteria;
            IEnumerable<Flight> fs = provider.GetFlights(FlightFilters.IsValid,FlightFilters.IsCurrent,FlightFilters.IsGroundTimeLessThan2Hours);
            foreach (Flight f in fs)
            {
                Console.WriteLine($"New flight found: ");
                for (int iSeg = 0; iSeg < f.Segments.Count; iSeg++)
                {
                    Segment seg = f.Segments[iSeg];
                    Console.WriteLine($"Segment n. {iSeg} departs at: {seg.DepartureDate} arrives at: {seg.ArrivalDate}");
                }
            }
        }
    }
}

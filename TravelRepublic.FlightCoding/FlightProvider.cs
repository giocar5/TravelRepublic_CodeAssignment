using System;
using System.Collections.Generic;
using System.Linq;
using TravelRepublic.FlightCoding.Extensions;

namespace TravelRepublic.FlightCoding
{
    //base class for any type of Flight provider class (could be a service, an azure functions, a FileParser)
    public abstract class FlightProvider
    {
        public FlightSearchCriteria FlightSearchCriteria{get; set;}
        protected abstract IEnumerable<Flight> GetFlightsCore(); 
        
        public bool ShouldOrderSegments{get;set;}
        protected virtual IEnumerable<Flight> GetFlightsOrderedSegments()
        {
            foreach (Flight flight in GetFlightsCore())
            {
                if (!ShouldOrderSegments)
                    flight.Segments = flight.Segments.OrderBy(seg => seg.DepartureDate).ToList();
                yield return flight;
            }
        }
        public virtual IEnumerable<Flight> GetFlights(params Predicate<Flight>[] filters)
        {
            return PostFilterFlights(GetFlightsOrderedSegments(),filters);
        }

        protected virtual IEnumerable<Flight> PostFilterFlights(IEnumerable<Flight> flights,params Predicate<Flight>[] filters)
        {
            return flights.MultiFilterAnd(filters);
        }
    }
}


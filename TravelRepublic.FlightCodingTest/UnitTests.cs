using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;
using TravelRepublic.FlightCoding;
using TravelRepublic.FlightCoding.Extensions;

namespace TravelRepublic.FlightCodingTest
{
   public class FlightTester
    {
        [Fact]
        public void Check_IsCurrentFlight()
        {
            FlightProvider flightBuilder= new FlightBuilder();
            IEnumerable<Flight> flights = flightBuilder.GetFlights(FlightFilters.IsCurrent);
            Assert.DoesNotContain(flights, (f=>f.Segments.Any(seg=> seg.DepartureDate < DateTime.Now)));
        }
        [Fact]
        public void Check_IsGroundTimeLessThan2HoursFlight()
        {
            FlightProvider flightBuilder= new FlightBuilder();
            IEnumerable<Flight> flights = flightBuilder.GetFlights(FlightFilters.IsGroundTimeLessThan2Hours);
            Assert.DoesNotContain(flights,FlightFilters.HasOverlappingSegments);
            Assert.DoesNotContain(flights, (flight => flight.GetTotalTravelTime().Subtract(flight.GetFlightTime()) > TimeSpan.FromHours(2)));
        }

        [Fact]
        public void Check_FlightHasValidDates()
        {
            FlightProvider flightBuilder= new FlightBuilder();
            IEnumerable<Flight> flights = flightBuilder.GetFlights(FlightFilters.IsValid);
            Assert.DoesNotContain(flights, (f=>f.Segments.Any(seg=> seg.DepartureDate > seg.ArrivalDate)));
        }
        
        // Segments in each Flight should be already ordered by DepartureDate
        [Fact]
        public void Check_FlightSegmentsAreOrdered()
        {
            FlightProvider flightBuilder= new FlightBuilder();
            IEnumerable<Flight> flights = flightBuilder.GetFlights();
            foreach(Flight f in flights)
            {
                for (int i = 1; i< f.Segments.Count; i++)
                {
                    Assert.False(f.Segments[i].DepartureDate < f.Segments[i-1].DepartureDate, "Unordered Segments found in flights");
                }
            }
        }    
        // Segments in some Flights have been misordered with ShouldOrderSegments set to false
        [Fact]
        public void Check_FlightSegmentsAreOrderedForced()
        {
            FlightProvider flightBuilder= new FlightBuilder();
            flightBuilder.ShouldOrderSegments = false;
            IEnumerable<Flight> flights = flightBuilder.GetFlights();
            bool ordered = true;
            foreach(Flight f in flights)
            {
                for (int i = 1; i< f.Segments.Count; i++)
                {
                    if (f.Segments[i].DepartureDate < f.Segments[i-1].DepartureDate)
                    {
                        ordered = false;
                        break;
                    }
                }
            }
            Assert.True(ordered, "Unordered Segments have been reordered in flights");
        }    
    }
}
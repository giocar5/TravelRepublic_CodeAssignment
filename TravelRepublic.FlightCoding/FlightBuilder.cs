using System;
using System.Collections.Generic;
using System.Linq;
using TravelRepublic.FlightCoding;

namespace TravelRepublic.FlightCoding
{
    public class FlightBuilder : FlightProvider
    {
        public FlightBuilder()
        {
            ShouldOrderSegments = false;
        }
        protected override IEnumerable<Flight> GetFlightsCore()
        {
            var threeDaysFromNow = DateTime.Now.AddDays(3);

            return new List<Flight>
            {
                // Another flight with more than two hours ground time
                CreateFlight( threeDaysFromNow.AddHours(6), threeDaysFromNow.AddHours(7),threeDaysFromNow.AddHours(3), threeDaysFromNow.AddHours(4), threeDaysFromNow, threeDaysFromNow.AddHours(2))
,                // A normal flight with two hour duration
                CreateFlight(threeDaysFromNow, threeDaysFromNow.AddHours(2)),

                // A normal multi-segment flight
                CreateFlight(threeDaysFromNow, threeDaysFromNow.AddHours(2), threeDaysFromNow.AddHours(3), threeDaysFromNow.AddHours(5)),
                
                // A flight departing in the past
                CreateFlight(threeDaysFromNow.AddDays(-6), threeDaysFromNow),

                // A flight that departs before it arrives
                CreateFlight(threeDaysFromNow, threeDaysFromNow.AddHours(-6)),

                // A flight with more than two hours ground time
                CreateFlight(threeDaysFromNow, threeDaysFromNow.AddHours(2), threeDaysFromNow.AddHours(5), threeDaysFromNow.AddHours(6)),

            };
        }

        private static Flight CreateFlight(params DateTime[] dates)
        {
            if (dates.Length % 2 != 0) 
                throw new ArgumentException("You must pass an even number of dates,", "dates");

            var departureDates = dates.Where((date, index) => index % 2 == 0);
            var arrivalDates = dates.Where((date, index) => index % 2 == 1);

            var segments = departureDates
                .Zip(arrivalDates, (departureDate, arrivalDate) => new Segment { DepartureDate = departureDate, ArrivalDate = arrivalDate })
                .ToList();

            return new Flight { Segments = segments };
        }
    }

}


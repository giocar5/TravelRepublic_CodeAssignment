using System.Collections.Generic;
using System.Linq;
using System;
using TravelRepublic.FlightCoding;

namespace TravelRepublic.FlightCoding.Extensions
{
    public static class FlightFilters
    {
        public static bool IsCurrent(this Flight f)
        {
            return f.Segments!= null && f.Segments.All(FlightExtensions.IsCurrent);
        }
        public static bool IsValid(this Flight f)
        {
            return f.Segments!= null && f.Segments.All(FlightExtensions.IsValid);
        }
        public static bool IsGroundTimeLessThan2Hours(this Flight f)
        {
            return IsGroundTimeLessThan(f,TimeSpan.FromHours(2));
        }
        public static bool IsGroundTimeLessThan(this Flight f,TimeSpan t)
        {
          // it's less elegant than a Aggregate function, but it could perform better and does not need to iterate the whole collection
            TimeSpan groundTime = TimeSpan.Zero;
            for (int i = 1; groundTime < t && i< f.Segments.Count; i++)
            {
                //if there's an overlap
                if (f.Segments[i].DepartureDate < f.Segments[i-1].ArrivalDate)
                {
                    groundTime = TimeSpan.MaxValue;
                    break;
                }
                groundTime += f.Segments[i].DepartureDate.Subtract(f.Segments[i-1].ArrivalDate);
            }
            return groundTime < t;
        }
        public static bool HasOverlappingSegments(this Flight f)
        {
            bool overlapFound = false;
            for (int i = 1;i< f.Segments.Count; i++)
            {
                if (f.Segments[i].DepartureDate < f.Segments[i-1].ArrivalDate)
                {
                    overlapFound  = true;
                    break;
                }
            }
            return overlapFound;
        }

    }
  //Since I did not know If I were allowed to modify Flight and Segment Class, I expanded them via these extension methods
    public static class FlightExtensions
    {
        public static DateTime? GetDepartureDate(this Flight f)
        {
            return f.Segments!= null ? new DateTime?(f.Segments.Min(s=>s.DepartureDate)) : null;
        }
        public static DateTime? GetArrivalDate(this Flight f)
        {
            return f.Segments!= null ? new DateTime?(f.Segments.Max(s=>s.ArrivalDate)) : null;
        }

        public static TimeSpan GetTotalTravelTime(this Flight f)
        {
            return f.GetDepartureDate().GetValueOrDefault().Subtract(f.GetArrivalDate().GetValueOrDefault());
        }
        public static TimeSpan GetFlightTime(this Flight f)
        {
            return (f.Segments!= null) ? f.Segments.Aggregate(TimeSpan.Zero,(ts, seg) => GetDuration(seg)) : TimeSpan.Zero;
        }

        public static TimeSpan GetDuration(this Segment seg)
        {
            return seg.DepartureDate.Subtract(seg.ArrivalDate);
        }
        public static bool IsValid(this Segment s)
        {
        return s.DepartureDate < s.ArrivalDate;
        }
        public static bool IsCurrent(this Segment s)
        {
        return s.DepartureDate > DateTime.Now;
        }

        public static IEnumerable<T> MultiFilterAnd<T>(this IEnumerable<T> collection,params Predicate<T>[] filters)
        {
            return collection.Where(e => filters.All(predicate=>predicate(e)));
        }
        public static IEnumerable<T> MultiFilterOr<T>(this IEnumerable<T> collection,params Predicate<T>[] filters)
        {
            return collection.Where(e => filters.Any(predicate=>predicate(e)));
        }
    }
    
}
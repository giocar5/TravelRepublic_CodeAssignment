using System.Collections.Generic;
using System.Linq;
using System;

namespace TravelRepublic.FlightCoding
{
  public class Segment
  {
    public virtual DateTime DepartureDate { get; set; }
    public virtual DateTime ArrivalDate { get; set; }

  }

  public class Airport
  {
    public string LongName{get;set;}
    public string Code{get;set;}
    public int NationId{get;set;}

    public string City{get;set;}
  }
  public class RouteSegment: Segment
  {
    public Airport Origin{get;set;}
    public Airport Destination{get;set;}
    public int Mileage{get;set;}
  }

}
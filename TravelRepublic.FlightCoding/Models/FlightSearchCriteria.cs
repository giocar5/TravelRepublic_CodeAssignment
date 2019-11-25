using System.Collections.Generic;
using System.Linq;
using System;

namespace TravelRepublic.FlightCoding
{
  //this class should be used to pre-filter data (not in the test scenario, but could be useful for a real time scenario like a request to a service)
    public class FlightSearchCriteria
    {
      public DateTime? DepartureDateFrom {get;set;}   
      public TimeSpan? MaxGroundTime {get;set;} 

      public TimeSpan? MaxTotalTime {get;set;}

      public int? MaxNumberOfChanges {get;set;} 
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib
{
  /// <summary>
  /// Container for handling data of a person
  /// </summary>
  public class PersonHandler
  {
    public long PId { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
  }
}

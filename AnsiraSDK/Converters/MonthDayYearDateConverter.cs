using Newtonsoft.Json.Converters;

namespace Ansira.Converters
{
  public class MonthDayYearDateConverter : IsoDateTimeConverter
  {
    public MonthDayYearDateConverter()
    {
      DateTimeFormat = "yyyy-MM-dd";
    }
  }
}

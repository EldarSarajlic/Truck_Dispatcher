using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Domain.Entities.Location
{
    public class CountryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string PhoneCode { get; set; }
        public string Currency { get; set; }
        public string TimeZone { get; set; }
       
        public IReadOnlyCollection<CityEntity> Cities { get; private set; } = new List<CityEntity>();


        public static class Constraints
        {
            public const int NameMaxLength = 100;
            public const int CountryCodeMaxLength = 10;
            public const int PhoneCodeMaxLength = 10;
            public const int CurrencyMaxLength = 50;
            public const int TimeZoneMaxLength = 50;
        }

    }
}
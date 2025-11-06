using Dispatcher.Domain.Common;

namespace Dispatcher.Domain.Entities.Location
{
    public class CityEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? PostalCode { get; set; }
        public int CountryId { get; set; }

        public CountryEntity Country { get; set; }

        public static class Constraints
        {
            public const int NameMaxLength = 100;
            public const int PostalCodeMaxLength = 20;
        }
    }
}
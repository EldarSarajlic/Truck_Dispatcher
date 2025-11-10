 public class GetUserByIdQueryDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string Role { get; set; }
        public bool IsEnabled { get; set; }

        public int? CityId { get; set; }
        public string? CityName { get; set; }

        public string? ProfilePhotoUrl { get; set; }
    }
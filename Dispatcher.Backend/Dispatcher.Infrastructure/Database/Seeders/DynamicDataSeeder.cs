using Dispatcher.Domain.Entities.Chat;
using Dispatcher.Domain.Entities.Location;
using Dispatcher.Domain.Entities.Media;
using Dispatcher.Domain.Entities.Services;
using Dispatcher.Domain.Entities.Shipments;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Infrastructure.Database.Seeders;

/// <summary>
/// Dynamic seeder that runs at application startup.
/// Seeds test/demo data for all entities in the system.
/// </summary>
public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Ensure database exists
        await context.Database.EnsureCreatedAsync();

        // Seed in order of dependencies
        await SeedCountriesAsync(context);
        await SeedCitiesAsync(context);
        await SeedUsersAsync(context);
        await SeedVehicleStatusesAsync(context);
        await SeedTrucksAsync(context);
        await SeedTrailersAsync(context);
        await SeedServiceCompaniesAsync(context);
        await SeedTruckServiceAssignmentsAsync(context);
        await SeedShipmentsAsync(context);
        await SeedRoutesAsync(context);
        await SeedMessagesAsync(context);
        await SeedNotificationsAsync(context);
        await SeedPhotosAsync(context);

        Console.WriteLine("✅ Dynamic seed completed: All test data added.");
    }

    #region Location Entities

    /// <summary>
    /// Seeds countries for the system
    /// </summary>
    private static async Task SeedCountriesAsync(DatabaseContext context)
    {
        if (await context.Country.AnyAsync())
            return;

        var countries = new List<CountryEntity>
        {
            new CountryEntity
            {
                Name = "Bosnia and Herzegovina",
                CountryCode = "BA",
                PhoneCode = "+387",
                Currency = "BAM",
                TimeZone = "Europe/Sarajevo"
            },
            new CountryEntity
            {
                Name = "Croatia",
                CountryCode = "HR",
                PhoneCode = "+385",
                Currency = "EUR",
                TimeZone = "Europe/Zagreb"
            },
            new CountryEntity
            {
                Name = "Serbia",
                CountryCode = "RS",
                PhoneCode = "+381",
                Currency = "RSD",
                TimeZone = "Europe/Belgrade"
            },
            new CountryEntity
            {
                Name = "Germany",
                CountryCode = "DE",
                PhoneCode = "+49",
                Currency = "EUR",
                TimeZone = "Europe/Berlin"
            },
            new CountryEntity
            {
                Name = "Austria",
                CountryCode = "AT",
                PhoneCode = "+43",
                Currency = "EUR",
                TimeZone = "Europe/Vienna"
            }
        };

        context.Country.AddRange(countries);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded countries.");
    }

    /// <summary>
    /// Seeds cities for the system
    /// </summary>
    private static async Task SeedCitiesAsync(DatabaseContext context)
    {
        if (await context.City.AnyAsync())
            return;

        // Get countries
        var ba = await context.Country.FirstAsync(c => c.CountryCode == "BA");
        var hr = await context.Country.FirstAsync(c => c.CountryCode == "HR");
        var rs = await context.Country.FirstAsync(c => c.CountryCode == "RS");
        var de = await context.Country.FirstAsync(c => c.CountryCode == "DE");
        var at = await context.Country.FirstAsync(c => c.CountryCode == "AT");

        var cities = new List<CityEntity>
        {
            // Bosnia and Herzegovina
            new CityEntity { Name = "Sarajevo", PostalCode = "71000", CountryId = ba.Id },
            new CityEntity { Name = "Banja Luka", PostalCode = "78000", CountryId = ba.Id },
            new CityEntity { Name = "Tuzla", PostalCode = "75000", CountryId = ba.Id },
            new CityEntity { Name = "Mostar", PostalCode = "88000", CountryId = ba.Id },
            new CityEntity { Name = "Zenica", PostalCode = "72000", CountryId = ba.Id },

            // Croatia
            new CityEntity { Name = "Zagreb", PostalCode = "10000", CountryId = hr.Id },
            new CityEntity { Name = "Split", PostalCode = "21000", CountryId = hr.Id },
            new CityEntity { Name = "Rijeka", PostalCode = "51000", CountryId = hr.Id },
            new CityEntity { Name = "Osijek", PostalCode = "31000", CountryId = hr.Id },

            // Serbia
            new CityEntity { Name = "Belgrade", PostalCode = "11000", CountryId = rs.Id },
            new CityEntity { Name = "Novi Sad", PostalCode = "21000", CountryId = rs.Id },
            new CityEntity { Name = "Niš", PostalCode = "18000", CountryId = rs.Id },

            // Germany
            new CityEntity { Name = "Berlin", PostalCode = "10115", CountryId = de.Id },
            new CityEntity { Name = "Munich", PostalCode = "80331", CountryId = de.Id },
            new CityEntity { Name = "Hamburg", PostalCode = "20095", CountryId = de.Id },
            new CityEntity { Name = "Frankfurt", PostalCode = "60311", CountryId = de.Id },

            // Austria
            new CityEntity { Name = "Vienna", PostalCode = "1010", CountryId = at.Id },
            new CityEntity { Name = "Graz", PostalCode = "8010", CountryId = at.Id },
            new CityEntity { Name = "Salzburg", PostalCode = "5020", CountryId = at.Id }
        };

        context.City.AddRange(cities);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded cities.");
    }

    #endregion

    #region Identity Entities

    /// <summary>
    /// Seeds demo users if they don't exist
    /// </summary>
    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<UserEntity>();

        // Get Sarajevo city for users
        var sarajevo = await context.City.FirstOrDefaultAsync(c => c.Name == "Sarajevo");

        var users = new List<UserEntity>
        {
            new UserEntity
            {
                FirstName = "Admin",
                LastName = "User",
                DisplayName = "Admin User",
                NormalizedDisplayName = "ADMIN USER",
                Email = "admin@dispatcher.local",
                PhoneNumber = "+387 61 123 456",
                DateOfBirth = new DateTime(1980, 1, 1),
                PasswordHash = hasher.HashPassword(null!, "Admin123!"),
                Role = UserRole.Admin,
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                CityId = sarajevo?.Id
            },
            new UserEntity
            {
                FirstName = "John",
                LastName = "Dispatcher",
                DisplayName = "John Dispatcher",
                NormalizedDisplayName = "JOHN DISPATCHER",
                Email = "dispatcher@dispatcher.local",
                PhoneNumber = "+387 61 234 567",
                DateOfBirth = new DateTime(1985, 5, 15),
                PasswordHash = hasher.HashPassword(null!, "Dispatcher123!"),
                Role = UserRole.Dispatcher,
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                CityId = sarajevo?.Id
            },
            new UserEntity
            {
                FirstName = "Mike",
                LastName = "Driver",
                DisplayName = "Mike Driver",
                NormalizedDisplayName = "MIKE DRIVER",
                Email = "driver@dispatcher.local",
                PhoneNumber = "+387 61 345 678",
                DateOfBirth = new DateTime(1990, 8, 20),
                PasswordHash = hasher.HashPassword(null!, "Driver123!"),
                Role = UserRole.Driver,
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                CityId = sarajevo?.Id
            },
            new UserEntity
            {
                FirstName = "Sarah",
                LastName = "Driver",
                DisplayName = "Sarah Driver",
                NormalizedDisplayName = "SARAH DRIVER",
                Email = "sarah@dispatcher.local",
                PhoneNumber = "+387 61 567 890",
                DateOfBirth = new DateTime(1992, 3, 25),
                PasswordHash = hasher.HashPassword(null!, "Driver123!"),
                Role = UserRole.Driver,
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                CityId = sarajevo?.Id
            },
            new UserEntity
            {
                FirstName = "Test",
                LastName = "Client",
                DisplayName = "Test Client",
                NormalizedDisplayName = "TEST CLIENT",
                Email = "string",
                PhoneNumber = "+387 61 456 789",
                DateOfBirth = new DateTime(1995, 12, 10),
                PasswordHash = hasher.HashPassword(null!, "string"),
                Role = UserRole.Client,
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                CityId = sarajevo?.Id
            },
            new UserEntity
            {
                FirstName = "Jane",
                LastName = "Client",
                DisplayName = "Jane Client",
                NormalizedDisplayName = "JANE CLIENT",
                Email = "jane@client.com",
                PhoneNumber = "+387 61 678 901",
                DateOfBirth = new DateTime(1988, 7, 30),
                PasswordHash = hasher.HashPassword(null!, "Client123!"),
                Role = UserRole.Client,
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                CityId = sarajevo?.Id
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded users.");
    }

    #endregion

    #region Vehicle Entities

    /// <summary>
    /// Seeds vehicle statuses
    /// </summary>
    private static async Task SeedVehicleStatusesAsync(DatabaseContext context)
    {
        if (await context.VehicleStatuses.AnyAsync())
            return;

        var statuses = new List<VehicleStatusEntity>
        {
            new VehicleStatusEntity
            {
                StatusName = "Available",
                Description = "Vehicle is available for assignment"
            },
            new VehicleStatusEntity
            {
                StatusName = "In Transit",
                Description = "Vehicle is currently on a delivery route"
            },
            new VehicleStatusEntity
            {
                StatusName = "In Maintenance",
                Description = "Vehicle is undergoing maintenance or repairs"
            },
            new VehicleStatusEntity
            {
                StatusName = "Out of Service",
                Description = "Vehicle is temporarily out of service"
            },
            new VehicleStatusEntity
            {
                StatusName = "Reserved",
                Description = "Vehicle is reserved for a specific task"
            }
        };

        context.VehicleStatuses.AddRange(statuses);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded vehicle statuses.");
    }

    /// <summary>
    /// Seeds trucks
    /// </summary>
    private static async Task SeedTrucksAsync(DatabaseContext context)
    {
        if (await context.Trucks.AnyAsync())
            return;

        var availableStatus = await context.VehicleStatuses.FirstAsync(s => s.StatusName == "Available");
        var inTransitStatus = await context.VehicleStatuses.FirstAsync(s => s.StatusName == "In Transit");
        var maintenanceStatus = await context.VehicleStatuses.FirstAsync(s => s.StatusName == "In Maintenance");

        var trucks = new List<TruckEntity>
        {
            new TruckEntity
            {
                LicensePlateNumber = "SA-123-AB",
                VinNumber = "WDB9630451L123456",
                Make = "Mercedes-Benz",
                Model = "Actros 1851",
                Year = 2020,
                Capacity = 24.5m,
                EngineCapacity = 12800,
                KW = 375,
                LastMaintenanceDate = DateTime.UtcNow.AddMonths(-2),
                NextMaintenanceDate = DateTime.UtcNow.AddMonths(4),
                RegistrationExpiration = DateTime.UtcNow.AddYears(1),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(6),
                GPSDeviceId = "GPS-MB-001",
                VehicleStatusId = availableStatus.Id
            },
            new TruckEntity
            {
                LicensePlateNumber = "SA-456-CD",
                VinNumber = "YV2A20A51GA123789",
                Make = "Volvo",
                Model = "FH16 750",
                Year = 2021,
                Capacity = 26.0m,
                EngineCapacity = 16100,
                KW = 551,
                LastMaintenanceDate = DateTime.UtcNow.AddMonths(-1),
                NextMaintenanceDate = DateTime.UtcNow.AddMonths(5),
                RegistrationExpiration = DateTime.UtcNow.AddYears(1).AddMonths(3),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(8),
                GPSDeviceId = "GPS-VLV-002",
                VehicleStatusId = inTransitStatus.Id
            },
            new TruckEntity
            {
                LicensePlateNumber = "SA-789-EF",
                VinNumber = "WMAN26ZZ4KW123456",
                Make = "MAN",
                Model = "TGX 18.500",
                Year = 2019,
                Capacity = 25.0m,
                EngineCapacity = 12400,
                KW = 368,
                LastMaintenanceDate = DateTime.UtcNow.AddDays(-5),
                NextMaintenanceDate = DateTime.UtcNow.AddDays(25),
                RegistrationExpiration = DateTime.UtcNow.AddMonths(9),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(5),
                GPSDeviceId = "GPS-MAN-003",
                VehicleStatusId = maintenanceStatus.Id
            },
            new TruckEntity
            {
                LicensePlateNumber = "SA-321-GH",
                VinNumber = "YS2R4X20005123456",
                Make = "Scania",
                Model = "R 500",
                Year = 2022,
                Capacity = 27.0m,
                EngineCapacity = 13000,
                KW = 368,
                LastMaintenanceDate = DateTime.UtcNow.AddMonths(-1),
                NextMaintenanceDate = DateTime.UtcNow.AddMonths(5),
                RegistrationExpiration = DateTime.UtcNow.AddYears(2),
                InsuranceExpiration = DateTime.UtcNow.AddYears(1),
                GPSDeviceId = "GPS-SCN-004",
                VehicleStatusId = availableStatus.Id
            },
            new TruckEntity
            {
                LicensePlateNumber = "SA-654-IJ",
                VinNumber = "XLRTE47MS0E123456",
                Make = "DAF",
                Model = "XF 480",
                Year = 2021,
                Capacity = 25.5m,
                EngineCapacity = 12900,
                KW = 353,
                LastMaintenanceDate = DateTime.UtcNow.AddMonths(-3),
                NextMaintenanceDate = DateTime.UtcNow.AddMonths(3),
                RegistrationExpiration = DateTime.UtcNow.AddYears(1).AddMonths(2),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(7),
                GPSDeviceId = "GPS-DAF-005",
                VehicleStatusId = availableStatus.Id
            }
        };

        context.Trucks.AddRange(trucks);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded trucks.");
    }

    /// <summary>
    /// Seeds trailers
    /// </summary>
    private static async Task SeedTrailersAsync(DatabaseContext context)
    {
        if (await context.Trailers.AnyAsync())
            return;

        var availableStatus = await context.VehicleStatuses.FirstAsync(s => s.StatusName == "Available");
        var inTransitStatus = await context.VehicleStatuses.FirstAsync(s => s.StatusName == "In Transit");

        var trailers = new List<TrailerEntity>
        {
            new TrailerEntity
            {
                LicensePlateNumber = "SA-T01-AB",
                Make = "Schmitz",
                Model = "Cargobull S.CS",
                Year = 2020,
                Type = "Refrigerated",
                Length = 13.6m,
                Capacity = 33.0m,
                RegistrationExpiration = DateTime.UtcNow.AddYears(1),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(8),
                VehicleStatusId = availableStatus.Id
            },
            new TrailerEntity
            {
                LicensePlateNumber = "SA-T02-CD",
                Make = "Krone",
                Model = "SD",
                Year = 2021,
                Type = "Standard Curtain",
                Length = 13.6m,
                Capacity = 33.0m,
                RegistrationExpiration = DateTime.UtcNow.AddYears(1).AddMonths(5),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(10),
                VehicleStatusId = inTransitStatus.Id
            },
            new TrailerEntity
            {
                LicensePlateNumber = "SA-T03-EF",
                Make = "Schwarzmüller",
                Model = "Box Trailer",
                Year = 2019,
                Type = "Box Trailer",
                Length = 13.6m,
                Capacity = 32.0m,
                RegistrationExpiration = DateTime.UtcNow.AddMonths(11),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(6),
                VehicleStatusId = availableStatus.Id
            },
            new TrailerEntity
            {
                LicensePlateNumber = "SA-T04-GH",
                Make = "Kögel",
                Model = "Mega",
                Year = 2022,
                Type = "Mega Trailer",
                Length = 13.6m,
                Capacity = 34.0m,
                RegistrationExpiration = DateTime.UtcNow.AddYears(2),
                InsuranceExpiration = DateTime.UtcNow.AddYears(1),
                VehicleStatusId = availableStatus.Id
            },
            new TrailerEntity
            {
                LicensePlateNumber = "SA-T05-IJ",
                Make = "Schmitz",
                Model = "Platform",
                Year = 2020,
                Type = "Flatbed",
                Length = 13.6m,
                Capacity = 27.0m,
                RegistrationExpiration = DateTime.UtcNow.AddYears(1).AddMonths(3),
                InsuranceExpiration = DateTime.UtcNow.AddMonths(9),
                VehicleStatusId = availableStatus.Id
            }
        };

        context.Trailers.AddRange(trailers);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded trailers.");
    }

    #endregion

    #region Service Entities

    /// <summary>
    /// Seeds service companies
    /// </summary>
    private static async Task SeedServiceCompaniesAsync(DatabaseContext context)
    {
        if (await context.ServiceCompanies.AnyAsync())
            return;

        var sarajevo = await context.City.FirstAsync(c => c.Name == "Sarajevo");
        var banjaLuka = await context.City.FirstAsync(c => c.Name == "Banja Luka");
        var zagreb = await context.City.FirstAsync(c => c.Name == "Zagreb");

        var companies = new List<ServiceCompanyEntity>
        {
            new ServiceCompanyEntity
            {
                CompanyName = "AutoServis Prima",
                ContactPerson = "Haris Hadžić",
                PhoneNumber = "+387 33 123 456",
                Email = "info@autoservisprima.ba",
                Address = "Butmirska cesta 56",
                CityId = sarajevo.Id,
                MaintenanceDate = DateTime.UtcNow.AddMonths(-1),
                ContractEndDate = DateTime.UtcNow.AddYears(2),
                Notes = "Specialized in Mercedes and Volvo maintenance"
            },
            new ServiceCompanyEntity
            {
                CompanyName = "Truck Service Center",
                ContactPerson = "Marko Marković",
                PhoneNumber = "+387 51 234 567",
                Email = "kontakt@truckservice.ba",
                Address = "Industrijska zona bb",
                CityId = banjaLuka.Id,
                MaintenanceDate = DateTime.UtcNow.AddMonths(-2),
                ContractEndDate = DateTime.UtcNow.AddYears(1).AddMonths(6),
                Notes = "Full service for all truck brands"
            },
            new ServiceCompanyEntity
            {
                CompanyName = "Zagreb Truck Solutions",
                ContactPerson = "Ivan Kovač",
                PhoneNumber = "+385 1 345 6789",
                Email = "info@zagrebtruck.hr",
                Address = "Slavonska avenija 22",
                CityId = zagreb.Id,
                MaintenanceDate = DateTime.UtcNow.AddMonths(-3),
                ContractEndDate = DateTime.UtcNow.AddYears(3),
                Notes = "Premium service partner for international fleets"
            }
        };

        context.ServiceCompanies.AddRange(companies);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded service companies.");
    }

    /// <summary>
    /// Seeds truck service assignments
    /// </summary>
    private static async Task SeedTruckServiceAssignmentsAsync(DatabaseContext context)
    {
        if (await context.TruckServiceAssignment.AnyAsync())
            return;

        var trucks = await context.Trucks.Take(3).ToListAsync();
        var companies = await context.ServiceCompanies.ToListAsync();

        var assignments = new List<TruckServiceAssignmentEntity>
        {
            new TruckServiceAssignmentEntity
            {
                TruckId = trucks[0].Id,
                ServiceCompanyId = companies[0].Id,
                AssignedDate = DateTime.UtcNow.AddMonths(-2),
                Cost = 1250.00m
            },
            new TruckServiceAssignmentEntity
            {
                TruckId = trucks[1].Id,
                ServiceCompanyId = companies[1].Id,
                AssignedDate = DateTime.UtcNow.AddMonths(-1),
                Cost = 890.50m
            },
            new TruckServiceAssignmentEntity
            {
                TruckId = trucks[2].Id,
                ServiceCompanyId = companies[0].Id,
                AssignedDate = DateTime.UtcNow.AddDays(-5),
                Cost = 2100.00m
            },
            new TruckServiceAssignmentEntity
            {
                TruckId = trucks[0].Id,
                ServiceCompanyId = companies[2].Id,
                AssignedDate = DateTime.UtcNow.AddMonths(-4),
                Cost = 750.00m
            }
        };

        context.TruckServiceAssignment.AddRange(assignments);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded truck service assignments.");
    }

    #endregion

    #region Shipment Entities

    /// <summary>
    /// Seeds shipments
    /// </summary>
    private static async Task SeedShipmentsAsync(DatabaseContext context)
    {
        if (await context.Shipments.AnyAsync())
            return;

        var shipments = new List<ShipmentEntity>
        {
            new ShipmentEntity
            {
                Weight = 18500m,
                Volume = 75.5m,
                PickupLocation = "Sarajevo Warehouse District",
                Status = "In Transit",
                Description = "Electronics and consumer goods for German market"
            },
            new ShipmentEntity
            {
                Weight = 22000m,
                Volume = 82.0m,
                PickupLocation = "Zagreb Industrial Zone",
                Status = "Pending",
                Description = "Automotive parts shipment"
            },
            new ShipmentEntity
            {
                Weight = 15750m,
                Volume = 68.3m,
                PickupLocation = "Belgrade Distribution Center",
                Status = "Delivered",
                Description = "Food and beverage products"
            },
            new ShipmentEntity
            {
                Weight = 24500m,
                Volume = 90.0m,
                PickupLocation = "Munich Loading Dock",
                Status = "In Transit",
                Description = "Machinery and industrial equipment"
            },
            new ShipmentEntity
            {
                Weight = 12300m,
                Volume = 55.5m,
                PickupLocation = "Vienna Logistics Hub",
                Status = "Pending",
                Description = "Medical supplies and equipment"
            },
            new ShipmentEntity
            {
                Weight = 19800m,
                Volume = 78.0m,
                PickupLocation = "Split Port Terminal",
                Status = "In Transit",
                Description = "Construction materials"
            },
            new ShipmentEntity
            {
                Weight = 21000m,
                Volume = 85.0m,
                PickupLocation = "Frankfurt Freight Center",
                Status = "Delivered",
                Description = "Textiles and clothing"
            }
        };

        context.Shipments.AddRange(shipments);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded shipments.");
    }

    /// <summary>
    /// Seeds routes
    /// </summary>
    private static async Task SeedRoutesAsync(DatabaseContext context)
    {
        if (await context.Routes.AnyAsync())
            return;

        var sarajevo = await context.City.FirstAsync(c => c.Name == "Sarajevo");
        var zagreb = await context.City.FirstAsync(c => c.Name == "Zagreb");
        var belgrade = await context.City.FirstAsync(c => c.Name == "Belgrade");
        var vienna = await context.City.FirstAsync(c => c.Name == "Vienna");
        var munich = await context.City.FirstAsync(c => c.Name == "Munich");
        var berlin = await context.City.FirstAsync(c => c.Name == "Berlin");

        var routes = new List<RouteEntity>
        {
            new RouteEntity
            {
                StartLocationId = sarajevo.Id,
                EndLocationId = zagreb.Id,
                EstimatedDuration = TimeSpan.FromHours(5.5)
            },
            new RouteEntity
            {
                StartLocationId = zagreb.Id,
                EndLocationId = vienna.Id,
                EstimatedDuration = TimeSpan.FromHours(4)
            },
            new RouteEntity
            {
                StartLocationId = vienna.Id,
                EndLocationId = munich.Id,
                EstimatedDuration = TimeSpan.FromHours(5)
            },
            new RouteEntity
            {
                StartLocationId = munich.Id,
                EndLocationId = berlin.Id,
                EstimatedDuration = TimeSpan.FromHours(7)
            },
            new RouteEntity
            {
                StartLocationId = sarajevo.Id,
                EndLocationId = belgrade.Id,
                EstimatedDuration = TimeSpan.FromHours(6)
            },
            new RouteEntity
            {
                StartLocationId = belgrade.Id,
                EndLocationId = vienna.Id,
                EstimatedDuration = TimeSpan.FromHours(9)
            },
            new RouteEntity
            {
                StartLocationId = zagreb.Id,
                EndLocationId = berlin.Id,
                EstimatedDuration = TimeSpan.FromHours(12)
            }
        };

        context.Routes.AddRange(routes);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded routes.");
    }

    #endregion

    #region Chat Entities

    /// <summary>
    /// Seeds messages between users
    /// </summary>
    private static async Task SeedMessagesAsync(DatabaseContext context)
    {
        if (await context.Messages.AnyAsync())
            return;

        var dispatcher = await context.Users.FirstAsync(u => u.Role == UserRole.Dispatcher);
        var driver1 = await context.Users.FirstAsync(u => u.Role == UserRole.Driver && u.FirstName == "Mike");
        var driver2 = await context.Users.FirstAsync(u => u.Role == UserRole.Driver && u.FirstName == "Sarah");
        var admin = await context.Users.FirstAsync(u => u.Role == UserRole.Admin);

        var messages = new List<MessageEntity>
        {
            new MessageEntity
            {
                SenderId = dispatcher.Id,
                ReceiverId = driver1.Id,
                Content = "Hi Mike, are you available for a delivery to Munich tomorrow?",
                SentAt = DateTime.UtcNow.AddHours(-2),
                IsRead = true
            },
            new MessageEntity
            {
                SenderId = driver1.Id,
                ReceiverId = dispatcher.Id,
                Content = "Yes, I'm available. What time should I pick up the load?",
                SentAt = DateTime.UtcNow.AddHours(-1.5),
                IsRead = true
            },
            new MessageEntity
            {
                SenderId = dispatcher.Id,
                ReceiverId = driver1.Id,
                Content = "Please be at the warehouse at 6 AM. The shipment details will be sent to your phone.",
                SentAt = DateTime.UtcNow.AddHours(-1),
                IsRead = true
            },
            new MessageEntity
            {
                SenderId = dispatcher.Id,
                ReceiverId = driver2.Id,
                Content = "Sarah, can you take the Vienna route this weekend?",
                SentAt = DateTime.UtcNow.AddMinutes(-30),
                IsRead = false
            },
            new MessageEntity
            {
                SenderId = admin.Id,
                ReceiverId = dispatcher.Id,
                Content = "Please review the new shipment protocols I sent to your email.",
                SentAt = DateTime.UtcNow.AddHours(-3),
                IsRead = true
            },
            new MessageEntity
            {
                SenderId = driver1.Id,
                ReceiverId = dispatcher.Id,
                Content = "Truck SA-123-AB needs an oil change soon. Can we schedule maintenance?",
                SentAt = DateTime.UtcNow.AddDays(-1),
                IsRead = true
            }
        };

        context.Messages.AddRange(messages);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded messages.");
    }

    /// <summary>
    /// Seeds notifications for users
    /// </summary>
    private static async Task SeedNotificationsAsync(DatabaseContext context)
    {
        if (await context.Notifications.AnyAsync())
            return;

        var dispatcher = await context.Users.FirstAsync(u => u.Role == UserRole.Dispatcher);
        var driver1 = await context.Users.FirstAsync(u => u.Role == UserRole.Driver && u.FirstName == "Mike");
        var driver2 = await context.Users.FirstAsync(u => u.Role == UserRole.Driver && u.FirstName == "Sarah");
        var admin = await context.Users.FirstAsync(u => u.Role == UserRole.Admin);

        var notifications = new List<NotificationEntity>
        {
            new NotificationEntity
            {
                UserId = driver1.Id,
                Title = "New Route Assignment",
                Message = "You have been assigned to the Sarajevo-Munich route. Departure time: 6:00 AM tomorrow.",
                IsRead = false
            },
            new NotificationEntity
            {
                UserId = driver1.Id,
                Title = "Maintenance Reminder",
                Message = "Your assigned truck SA-123-AB is due for maintenance in 2 weeks.",
                IsRead = true,
                ReadAt = DateTime.UtcNow.AddHours(-12)
            },
            new NotificationEntity
            {
                UserId = driver2.Id,
                Title = "Route Change",
                Message = "Your route has been updated. Please check the new itinerary.",
                IsRead = false
            },
            new NotificationEntity
            {
                UserId = dispatcher.Id,
                Title = "Shipment Delivered",
                Message = "Shipment #3 has been successfully delivered to Belgrade Distribution Center.",
                IsRead = true,
                ReadAt = DateTime.UtcNow.AddDays(-1)
            },
            new NotificationEntity
            {
                UserId = dispatcher.Id,
                Title = "New Shipment Request",
                Message = "New shipment request from Vienna Logistics Hub requires assignment.",
                IsRead = false
            },
            new NotificationEntity
            {
                UserId = admin.Id,
                Title = "System Update",
                Message = "Monthly performance reports are now available for review.",
                IsRead = false
            },
            new NotificationEntity
            {
                UserId = driver1.Id,
                Title = "Payment Processed",
                Message = "Your payment for last month's deliveries has been processed.",
                IsRead = true,
                ReadAt = DateTime.UtcNow.AddDays(-2)
            },
            new NotificationEntity
            {
                UserId = driver2.Id,
                Title = "Insurance Expiration",
                Message = "The insurance for your assigned vehicle expires in 30 days. Please coordinate with admin.",
                IsRead = false
            }
        };

        context.Notifications.AddRange(notifications);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded notifications.");
    }

    #endregion

    #region Media Entities

    /// <summary>
    /// Seeds sample photo records (metadata only, no actual files)
    /// </summary>
    private static async Task SeedPhotosAsync(DatabaseContext context)
    {
        if (await context.Photos.AnyAsync())
            return;

        var admin = await context.Users.FirstAsync(u => u.Role == UserRole.Admin);
        var dispatcher = await context.Users.FirstAsync(u => u.Role == UserRole.Dispatcher);

        var photos = new List<PhotoEntity>
        {
            new PhotoEntity
            {
                OriginalFileName = "truck-mercedes-actros.jpg",
                StoredFileName = "a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg",
                FilePath = "photos/vehicles/trucks/2024/",
                Url = "https://cdn.dispatcher.local/photos/vehicles/trucks/2024/a1b2c3d4.jpg",
                ContentType = "image/jpeg",
                FileSizeBytes = 2456789,
                Width = 1920,
                Height = 1080,
                ThumbnailUrl = "https://cdn.dispatcher.local/photos/vehicles/trucks/2024/thumbs/a1b2c3d4.jpg",
                PhotoCategory = "VehiclePhoto",
                AltText = "Mercedes-Benz Actros 1851",
                UploadedByUserId = admin.Id
            },
            new PhotoEntity
            {
                OriginalFileName = "trailer-schmitz.jpg",
                StoredFileName = "b2c3d4e5-f6g7-8901-bcde-fg2345678901.jpg",
                FilePath = "photos/vehicles/trailers/2024/",
                Url = "https://cdn.dispatcher.local/photos/vehicles/trailers/2024/b2c3d4e5.jpg",
                ContentType = "image/jpeg",
                FileSizeBytes = 1987654,
                Width = 1920,
                Height = 1080,
                ThumbnailUrl = "https://cdn.dispatcher.local/photos/vehicles/trailers/2024/thumbs/b2c3d4e5.jpg",
                PhotoCategory = "VehiclePhoto",
                AltText = "Schmitz Cargobull Refrigerated Trailer",
                UploadedByUserId = admin.Id
            },
            new PhotoEntity
            {
                OriginalFileName = "warehouse-sarajevo.jpg",
                StoredFileName = "c3d4e5f6-g7h8-9012-cdef-gh3456789012.jpg",
                FilePath = "photos/locations/warehouses/2024/",
                Url = "https://cdn.dispatcher.local/photos/locations/warehouses/2024/c3d4e5f6.jpg",
                ContentType = "image/jpeg",
                FileSizeBytes = 3124567,
                Width = 2560,
                Height = 1440,
                ThumbnailUrl = "https://cdn.dispatcher.local/photos/locations/warehouses/2024/thumbs/c3d4e5f6.jpg",
                PhotoCategory = "LocationPhoto",
                AltText = "Sarajevo Warehouse District",
                UploadedByUserId = dispatcher.Id
            },
            new PhotoEntity
            {
                OriginalFileName = "damage-report-001.jpg",
                StoredFileName = "d4e5f6g7-h8i9-0123-defg-hi4567890123.jpg",
                FilePath = "photos/reports/damage/2024/11/",
                Url = "https://cdn.dispatcher.local/photos/reports/damage/2024/11/d4e5f6g7.jpg",
                ContentType = "image/jpeg",
                FileSizeBytes = 1543210,
                Width = 1280,
                Height = 720,
                ThumbnailUrl = "https://cdn.dispatcher.local/photos/reports/damage/2024/11/thumbs/d4e5f6g7.jpg",
                PhotoCategory = "DocumentScan",
                AltText = "Minor damage report - rear bumper",
                UploadedByUserId = dispatcher.Id
            }
        };

        context.Photos.AddRange(photos);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Seeded photos.");
    }

    #endregion
}
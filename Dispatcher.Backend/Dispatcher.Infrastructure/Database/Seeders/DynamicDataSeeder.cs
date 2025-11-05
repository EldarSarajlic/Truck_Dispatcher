namespace Dispatcher.Infrastructure.Database.Seeders;

/// <summary>
/// Dynamic seeder koji se pokreće u runtime-u,
/// obično pri startu aplikacije (npr. u Program.cs).
/// Koristi se za unos demo/test podataka koji nisu dio migracije.
/// </summary>
public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Osiguraj da baza postoji (bez migracija)
        await context.Database.EnsureCreatedAsync();

        await SeedUsersAsync(context);
    }

    /// <summary>
    /// Kreira demo korisnike ako ih još nema u bazi.
    /// </summary>
    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<UserEntity>();

        var admin = new UserEntity
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
            AccessFailedCount = 0
        };

        var dispatcher = new UserEntity
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
            AccessFailedCount = 0
        };

        var driver = new UserEntity
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
            AccessFailedCount = 0
        };

        var client = new UserEntity
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
            AccessFailedCount = 0
        };

        context.Users.AddRange(admin, dispatcher, driver, client);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: demo users added.");
    }
}
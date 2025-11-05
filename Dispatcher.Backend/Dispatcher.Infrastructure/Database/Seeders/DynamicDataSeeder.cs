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

        await SeedProductCategoriesAsync(context);
        await SeedUsersAsync(context);
    }

    private static async Task SeedProductCategoriesAsync(DatabaseContext context)
    {
       
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
            Email = "admin@dispatcher.local",
            PasswordHash = hasher.HashPassword(null!, "Admin123!"),
            Role = UserRole.Admin,
            IsEnabled = true,
        };

        var dispatcher = new UserEntity
        {
            Email = "dispatcher@dispatcher.local",
            PasswordHash = hasher.HashPassword(null!, "Dispatcher123!"),
            Role = UserRole.Dispatcher,
            IsEnabled = true,
        };

        var driver = new UserEntity
        {
            Email = "driver@dispatcher.local",
            PasswordHash = hasher.HashPassword(null!, "Driver123!"),
            Role = UserRole.Driver,
            IsEnabled = true,
        };

        var client = new UserEntity
        {
            Email = "string",
            PasswordHash = hasher.HashPassword(null!, "string"),
            Role = UserRole.Client,
            IsEnabled = true,
        };

        context.Users.AddRange(admin, dispatcher, driver, client);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dynamic seed: demo users added.");
    }
}
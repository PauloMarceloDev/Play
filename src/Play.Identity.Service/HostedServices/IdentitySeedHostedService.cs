using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Play.Identity.Service.Entities;
using Play.Identity.Service.Settings;

namespace Play.Identity.Service.HostedServices;

public sealed class IdentitySeedHostedService(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<IdentitySettings> identitySettingsOptions,
    ILogger<IdentitySeedHostedService> logger)
    : IHostedService
{
    private readonly IdentitySettings _identitySettings = identitySettingsOptions.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        await CreateRoleIfNotExistsAsync(Roles.Admin, roleManager);
        await CreateRoleIfNotExistsAsync(Roles.Player, roleManager);

        var adminUser = await userManager.FindByEmailAsync(_identitySettings.AdminUserEmail);

        if (adminUser is null)
        {
            logger.LogInformation("Admin user not exists");
            adminUser = new ApplicationUser
            {
                UserName = _identitySettings.AdminUserEmail,
                Email = _identitySettings.AdminUserEmail,
            };

            await userManager.CreateAsync(adminUser, _identitySettings.AdminUserPassword);
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            logger.LogInformation("Admin user created");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task CreateRoleIfNotExistsAsync(
        string role,
        RoleManager<ApplicationRole> roleManager)
    {
        var roleExists = await roleManager.RoleExistsAsync(role);

        if (roleExists is false)
        {
            logger.LogInformation("{Role} role not exists", role);
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
            logger.LogInformation("{Role} role created", role);
        }
    }
}

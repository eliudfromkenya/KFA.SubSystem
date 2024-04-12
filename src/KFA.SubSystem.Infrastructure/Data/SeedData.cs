using KFA.SubSystem.Core.Classes;
using KFA.SubSystem.Core.ContributorAggregate;
using KFA.SubSystem.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KFA.SubSystem.Infrastructure.Data;

public static class SeedData{

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null);

    AsyncUtil.RunSync(() => PayrollGroups.Process(dbContext));
    AsyncUtil.RunSync(() => EndPointsAccessRights.Process(dbContext));

    // Look for any and populate default values.
  }
}

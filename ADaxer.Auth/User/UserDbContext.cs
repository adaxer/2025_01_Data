using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ADaxer.Auth.User;

public class UserDbContext : IdentityDbContext<ApplicationUser>
{
  public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
  {
  }

}

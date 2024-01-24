using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SourceName.Contracts.Persistence.Models;

namespace SourceName.Contracts.Persistence.DbContext;

public class SourceNameDbContext : IdentityDbContext<AppUser>
{
	
}
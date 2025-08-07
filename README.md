# affaz_new


## Code First Approach

## Install packages required
-- MSSQL
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

-- AutoMapper
```
dotnet add package automapper
```


### To create new migration file
-- Crete new migraiton file "initialMigrations_TIMESTAMP" with sql script
dotnet ef migrations add initialMigrations
dotnet ef migrations add initialMigrations -o ./Data/Migrations

dotnet ef migrations add initialMigrations -o \\Data\\Migrations



### To apply migration against DataBase
dotnet ef database update


### To View Migrations
dotnet ef migrations list

### To Remove Migrations
dotnet ef migrations remove


## To add db context
```
// MyAppDbContext.cs
using Microsoft.EntityFrameworkCore;

public class MyAppDbContext : DbContext
{
    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } // Example entity
}

```


### Make Edits in Program.cs
```
// ✅ Add DbContext and SQL Server connection string
builder.Services.AddDbContext<MyAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

```

Set connectionStirng in appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

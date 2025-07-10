# Database Configuration 
At App.xaml.cs
 optionsBuilder.UseSqlServer("Server=serverName;Database=SalesOrderDB;Integrated Security=True;TrustServerCertificate=True;");




### IF NEEDED TO INSTALL PACKAGES
# All projects
Install-Package Microsoft.Extensions.DependencyInjection
Install-Package MaterialDesignThemes
Install-Package MaterialDesignColors

# Core & Infrastructure
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.0
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.0

# SalesOrder & Modules
Install-Package Prism.DryIoc -Version 8.1.97
Install-Package Prism.Core -Version 8.1.97

# SalesOrder
Install-Package Microsoft.Extensions.Configuration
Install-Package Microsoft.Extensions.Configuration.FileExtensions
Install-Package Microsoft.Extensions.Configuration.Json


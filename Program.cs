using Aasaan_API.DbService;
using Aasaan_API.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


try
{
  var builder = WebApplication.CreateBuilder(args);

  builder.Services.AddControllersWithViews();
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(options =>
  {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
      Title = "JWTToken_Auth_API",
      Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
      Name = "Authorization",
      Type = SecuritySchemeType.ApiKey,
      Scheme = "Bearer",
      BearerFormat = "JWT",
      In = ParameterLocation.Header,
      Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input   below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
              {
                  new OpenApiSecurityScheme {
                      Reference = new OpenApiReference {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                      }
                  },
                  new string[] {}
              }
      });
  });

  builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

  }).AddJwtBearer(options =>
  {
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,

      ValidIssuer = builder.Configuration["Jwt:Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
  });


  // Add services to the container.

  builder.Services.AddControllers();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  builder.Services.AddScoped<IAccount, AccountService>();
  builder.Services.AddScoped<IAdmin, AdminDetailsService>();

  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowSpecificOrigin",
          policyBuilder => policyBuilder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod());
  });

  builder.Services.AddHttpClient();

  var app = builder.Build();
  var logger = app.Services.GetRequiredService<ILogger<Program>>();
  var configuration = app.Configuration;

  try
  {
    string workingDirectory = Directory.GetCurrentDirectory();
    logger.LogInformation("Application working directory: {WorkingDirectory}", workingDirectory);

    string serilogPathKey = "Serilog:WriteTo:0:Args:path";
    string? configuredLogPathTemplate = configuration[serilogPathKey];

    if (!string.IsNullOrEmpty(configuredLogPathTemplate))
    {
      logger.LogInformation("Log path template configured in appsettings (at key '{ConfigKey}'): {LogPathTemplate}", serilogPathKey, configuredLogPathTemplate);

      if (!Path.IsPathRooted(configuredLogPathTemplate))
      {
        logger.LogInformation("This configured path is relative. It will be combined with the working directory.");
        try
        {
          string? directoryPart = Path.GetDirectoryName(configuredLogPathTemplate);
          if (!string.IsNullOrEmpty(directoryPart))
          {
            string expectedDirectory = Path.GetFullPath(Path.Combine(workingDirectory, directoryPart));
            logger.LogInformation("Expected full log directory based on configuration: {ExpectedLogDirectory}", expectedDirectory);
          }
          else
          {
            logger.LogInformation("Log files expected directly in working directory based on template: {WorkingDirectory}", workingDirectory);
          }
          logger.LogInformation("Actual file names will depend on the rolling interval (e.g., adding date).");
        }
        catch (ArgumentException argEx)
        {
          logger.LogWarning(argEx, "Could not parse the configured path template to determine the directory part reliably.");
        }
        catch (Exception pathEx)
        {
          logger.LogWarning(pathEx, "Could not reliably determine full expected path from template and working directory.");
        }
      }
      else
      {
        logger.LogInformation("This configured path is absolute.");
        logger.LogInformation("Actual file names will depend on the rolling interval (e.g., adding date).");
      }
    }
    else
    {
      logger.LogWarning("Could not find or read the log file path configuration in appsettings.json at key '{ConfigKey}'. Please check Serilog configuration.", serilogPathKey);
    }
  }
  catch (Exception ex)
  {
    logger.LogError(ex, "Error determining or logging the configured log path.");
  }
  // Configure the HTTP request pipeline.
  app.UseSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTToken_Auth_API v1"));

  app.UseCors("AllowSpecificOrigin");
  app.UseHttpsRedirection();
  app.UseStaticFiles();

  app.UseRouting();

  app.UseAuthentication();
  app.UseAuthorization();

  app.UseEndpoints(endpoints =>
  {
    endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
  });

  app.Run();

}
catch (Exception ex)
{

}
finally
{

}
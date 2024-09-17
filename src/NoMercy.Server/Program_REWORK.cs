// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AspNetCore.Swagger.Themes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NoMercy.Api.Controllers.Socket;
using NoMercy.Api.Middleware;
using NoMercy.Data.Repositories;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.MediaProcessing.Collections;
using NoMercy.MediaProcessing.Episodes;
using NoMercy.MediaProcessing.Libraries;
using NoMercy.MediaProcessing.Movies;
using NoMercy.MediaProcessing.People;
using NoMercy.MediaProcessing.Seasons;
using NoMercy.MediaProcessing.Shows;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Queue;
using NoMercy.Server.App.Helper;
using NoMercy.Server.Swagger;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Security.Claims;
using CollectionRepository=NoMercy.Data.Repositories.CollectionRepository;
using ICollectionRepository=NoMercy.Data.Repositories.ICollectionRepository;
using ILibraryRepository=NoMercy.Data.Repositories.ILibraryRepository;
using IMovieRepository=NoMercy.Data.Repositories.IMovieRepository;
using LibraryRepository=NoMercy.Data.Repositories.LibraryRepository;
using MovieRepository=NoMercy.Data.Repositories.MovieRepository;

namespace NoMercy.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program2 {
    public async static Task MainAltered(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        // - SeriLog logging overwrite -
        builder.Logging.ClearProviders();// Removes the old Microsoft Logging
        builder.Services.AddLogging(loggingBuilder => {
            loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        });
        builder.Logging.AddSerilog(Log.Logger);
        builder.Services.AddSingleton(Log.Logger);// Else Injecting from Serilog.ILogger won't work
        
        
        // - Kestrel -
        builder.WebHost.ConfigureKestrel(Certificate.KestrelConfig);
        
        // - Use Urls -
        var url = new UriBuilder
        {
            Host = IPAddress.Any.ToString(),
            Port = NoMercyConfig.InternalServerPort,
            Scheme = "https"
        };
        builder.WebHost.UseUrls(url.ToString());
        
        // - Databases - 
        builder.Services.AddDbContext<QueueContext>(optionsAction =>
        {
            optionsAction.UseSqlite($"Data Source={AppFiles.QueueDatabase}");
        });
        builder.Services.AddTransient<QueueContext>();

        builder.Services.AddDbContext<MediaContext>(optionsAction =>
        {
            optionsAction.UseSqlite($"Data Source={AppFiles.MediaDatabase} Pooling=True",
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
        builder.Services.AddTransient<MediaContext>();
        
        // - Authorization -
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("api", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
                policy.RequireClaim("scope", "openid", "profile");
                policy.AddRequirements(new AssertionRequirement(context =>
                {
                    using MediaContext mediaContext = new();
                    User? user = mediaContext.Users
                        .FirstOrDefault(user =>
                            user.Id == Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                string.Empty));
                    return user is not null;
                }));
            });
        
        // - Authentication -
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = NoMercyConfig.AuthBaseUrl;
                options.RequireHttpsMetadata = true;
                options.Audience = "nomercy-ui";
                options.Audience = "nomercy-server";
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        StringValues accessToken = context.Request.Query["access_token"];
                        string[] result = accessToken.ToString().Split('&');

                        if (result.Length > 0 && !string.IsNullOrEmpty(result[0])) context.Token = result[0];

                        return Task.CompletedTask;
                    }
                };
            });
        
        // - Swagger -
        builder.Services.AddSwaggerGen(options => { options.OperationFilter<SwaggerDefaultValues>(); });
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
        // - Cors - 
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowNoMercyOrigins",
                policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://nomercy.tv")
                        .WithOrigins("https://app-dev.nomercy.tv")
                        .WithOrigins("https://app.nomercy.tv")
                        .WithOrigins("https://dev.nomercy.tv")
                        .WithOrigins("https://cast.nomercy.tv")
                        .WithOrigins("https://vscode.nomercy.tv")
                        .WithOrigins("https://hlsjs.video-dev.org")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithHeaders("Access-Control-Allow-Private-Network", "true")
                        .AllowAnyHeader();
                });
        });
        
        // - Signal R -
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
                o.MaximumReceiveMessageSize = 1024 * 1000 * 100;
            })
            .AddNewtonsoftJsonProtocol(options => { options.PayloadSerializerSettings = JsonHelper.Settings; });
        
        // - SERVICES -
        builder.Services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>(); 
        builder.Services.AddSingleton<ISunsetPolicyManager, DefaultSunsetPolicyManager>();

        builder.Services.AddMemoryCache();
        
        builder.Services.AddSingleton<JobQueue>();
        builder.Services.AddSingleton<Helpers.Monitoring.ResourceMonitor>();
        builder.Services.AddSingleton<Networking.Networking>();
        
        builder.Services.AddScoped<IEncoderRepository, EncoderRepository>();
        builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IFolderRepository, FolderRepository>();
        builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
        builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<ITvShowRepository, TvShowRepository>();

        builder.Services.AddScoped<ILibraryManager, LibraryManager>();
        builder.Services.AddScoped<IMovieManager, MovieManager>();
        builder.Services.AddScoped<ICollectionManager, CollectionManager>();
        builder.Services.AddScoped<IShowManager, ShowManager>();
        builder.Services.AddScoped<ISeasonManager, SeasonManager>();
        builder.Services.AddScoped<IEpisodeManager, EpisodeManager>();
        builder.Services.AddScoped<IPersonManager, PersonManager>();
        
        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
        
        builder.Services.AddTransient<DynamicStaticFilesMiddleware>();

        // ------------------------------------------------------------------------------------------------------------- 
        // End of builder
        // -------------------------------------------------------------------------------------------------------------    
        WebApplication app = builder.Build();
       
        app.UseRouting();
        app.UseCors("AllowNoMercyOrigins");

        // Security Middleware
        app.UseHsts();
        app.UseHttpsRedirection();

        // Performance Middleware
        app.UseResponseCompression();
        app.UseRequestLocalization();
        // app.UseResponseCaching();

        // Custom Middleware
        app.UseMiddleware<LocalizationMiddleware>();
        app.UseMiddleware<TokenParamAuthMiddleware>();

        // Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Logging Middleware
        app.UseMiddleware<AccessLogMiddleware>();

        // Static Files Middleware
        app.UseMiddleware<DynamicStaticFilesMiddleware>();

        // Development Tools
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(ModernStyle.Dark, options =>
        {
            options.RoutePrefix = string.Empty;
            options.DocumentTitle = "NoMercy MediaServer API";
            options.OAuthClientId("nomercy-server");
            options.OAuthScopes("openid");
            options.EnablePersistAuthorization();
            options.EnableTryItOutByDefault();

            foreach (ApiVersionDescription description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });

        // MVC
        app.UseMvcWithDefaultRoute();

        // WebSockets
        app.UseWebSockets()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapHub<VideoHub>("/socket", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                    options.CloseOnAuthenticationExpiration = true;
                });

                endpoints.MapHub<DashboardHub>("/dashboardHub", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                    options.CloseOnAuthenticationExpiration = true;
                });
            });

        // Static Files
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(AppFiles.TranscodePath),
            RequestPath = new PathString("/transcode"),
            ServeUnknownFileTypes = true,
            HttpsCompression = HttpsCompressionMode.Compress
        });

        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = new PhysicalFileProvider(AppFiles.TranscodePath),
            RequestPath = new PathString("/transcode")
        });

        // Initialize Dynamic Static Files Middleware
        MediaContext mediaContext = new();
        List<Folder> folderLibraries = mediaContext.Folders.ToList(); // Todo always interact with the db async

        foreach (Folder folder in folderLibraries.Where(folder => Directory.Exists(folder.Path)))
            DynamicStaticFilesMiddleware.AddPath(folder.Id, folder.Path);
        
        // - Start actual thing - 
        await app.RunAsync();
    }
}

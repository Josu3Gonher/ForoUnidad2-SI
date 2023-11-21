using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json.Serialization;
using WebApiAutores.Entities;
using WebApiAutores.Filters;
using WebApiAutores.Middlewares;
using WebApiAutores.Services;

namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async void ConfigureServices(IServiceCollection services)
        {
           services.AddControllers(options =>
           {
               options.Filters.Add(typeof(ExceptionFilter));
           })
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = 
                ReferenceHandler.IgnoreCycles);

            // Add DBContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<MiFiltro>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddAutoMapper(typeof(Startup));

            //Add Firebase
            // Configuración de Firebase con token de actualización
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("C:\\Users\\Josue Gonzalez\\Documents\\Sistemas de Informacion\\Unidad 1\\WebApiAutores\\WebApiAutores\\apiautores2023-firebase-adminsdk-pyo6c-935cfc3947.json"),
                ProjectId = "apiautores2023"
            });

            //services.AddSingleton(new FirebaseStorageService("apiautores2023.appspot.com", token));

            //Add Identity
            services.AddIdentity<IdentityUser, IdentityRole>( options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            //Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            

            // Add Cache Filter
            services.AddResponseCaching();

            // Add CORS
            services.AddCors(opciones =>
            {
                opciones.AddPolicy("CorsRule", rule =>
                {
                    rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });
            });

           services.AddEndpointsApiExplorer();
           services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //app.Map("/semitas", app =>
            //{
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("Interceptando la pipeline de procesos");
            //    });
            //});

            app.UseLogginResponseHttp();

            //if (env.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseCors("CorsRule");

            app.UseAuthorization();

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapControllers();
            });
        }
    }
}

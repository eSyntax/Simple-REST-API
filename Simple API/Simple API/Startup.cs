using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Simple_API.Enums;
using Simple_API.Helpers;
using Simple_API.Models;
using System.Collections.Generic;
using System.Text;

namespace Simple_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API V1", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                c.SchemaFilter<EnumSchemaFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddMvc();

            services.AddControllers().AddNewtonsoftJson(opt =>
                //Ignore looping models
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Models serialization and display
            //object p = services.AddControllers().AddNewtonsoftJson(o =>
            //{
            //    // Used for convert Enum values to attribute values
            //    o.SerializerSettings.Converters = new List<JsonConverter> { new StringEnumConverter {} };
            //    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    o.SerializerSettings.Formatting = Formatting.Indented;
            //});

            services.AddDbContext<AppDBContext>(option => option.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitializingDbData(app);
        }

        //Resetting tables data on every startup
        private static void InitializingDbData(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDBContext>();

            context.Database.ExecuteSqlRaw("TRUNCATE TABLE todolist;");
            //Cannot TRUNCATE this table because of foreign key restriction
            context.Database.ExecuteSqlRaw("DELETE FROM userinfo;");

            var users = new List<UserInfo>()
            {
                new UserInfo { 
                    Email = "admin@admin.com",
                    Password = "test123456789",
                    Privileges = UserPrivileges.Admin
                },
                new UserInfo {
                    Email = "user@user.com",
                    Password = "test123456789",
                    Privileges = UserPrivileges.User
                },
                new UserInfo {
                    Email = "user2@user2.com",
                    Password = "test123456789",
                    Privileges = UserPrivileges.User
                }
            };

            context.UserInfo.AddRange(users);
            context.SaveChanges();

            var todoList = new List<ToDoList>()
            {
                new ToDoList
                {
                    Description = "admin not completed",
                    TaskCompleted = TaskStatus.NotCompleted,
                    UserId = users[0].UserId
                },
                new ToDoList
                {
                    Description = "admin completed task",
                    TaskCompleted = TaskStatus.Completed,
                    UserId = users[0].UserId
                },
                new ToDoList
                {
                    Description = "user not compl",
                    TaskCompleted = TaskStatus.NotCompleted,
                    UserId = users[1].UserId
                },
                new ToDoList
                {
                    Description = "user2 compl",
                    TaskCompleted = TaskStatus.Completed,
                    UserId = users[2].UserId
                },
                new ToDoList
                {
                    Description = "user2 not compl",
                    TaskCompleted = TaskStatus.Completed,
                    UserId = users[2].UserId
                },
                new ToDoList
                {
                    Description = "user2 test",
                    TaskCompleted = TaskStatus.Completed,
                    UserId = users[2].UserId
                },
            };

            context.ToDoList.AddRange(todoList);
            context.SaveChanges();

        }
    }
}

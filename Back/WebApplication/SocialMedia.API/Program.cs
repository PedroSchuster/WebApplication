using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.API.HubConfig;
using SocialMedia.Application;
using SocialMedia.Application.Contratos;
using SocialMedia.Domain.Contratos;
using SocialMedia.Domain.Identity;
using SocialMedia.Persistence;
using SocialMedia.Persistence.Contextos;
using SocialMedia.Persistence.Contratos;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration;
// Add services to the container.
builder.Services.AddDbContext<SocialMediaContext>(opt =>
{
    opt.UseSqlServer((builder.Configuration.GetConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))));
});

builder.Services.AddIdentityCore<User>(x =>
{
    x.Password.RequireDigit = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireUppercase = false;
    x.Password.RequiredLength = 4;
})
    .AddSignInManager<SignInManager<User>>()
    .AddEntityFrameworkStores<SocialMediaContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // bearer eh o portador do jwt
             .AddJwtBearer(x =>
             {
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])), // descriptografar o token recebido pela requisiçao
                     ValidateIssuer = false,
                     ValidateAudience = false
                 }; 
             });



builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    // quando for serializar, vai converter para string o enum e n o id em int
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
    Newtonsoft.Json.ReferenceLoopHandling.Ignore); // evitar aqueles loops infinitos de muitos para muitos;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

//procura quem esta herdando de profile e tbm injeta onde precisar
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserPersist, UserPersist>();
builder.Services.AddScoped<IPostPersist, PostPersist>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessagePersist, MessagePersist>();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventos.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = @"Bearer + token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        }, new List<string>()
                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(cors => cors.AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowAnyOrigin());


app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Images")),
    RequestPath = new PathString("/Resources/Images")
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chat");
});

app.MapControllers();
app.Run();

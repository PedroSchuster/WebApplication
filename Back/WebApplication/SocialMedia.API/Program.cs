using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Persistence;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration;
// Add services to the container.
builder.Services.AddDbContext<SocialMediaContext>(opt =>
{
    opt.UseSqlServer((builder.Configuration.GetConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

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

app.MapControllers();
app.Run();

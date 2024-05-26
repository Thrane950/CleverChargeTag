using Microsoft.EntityFrameworkCore;
using CleverTagProject.DataContext; // Replace with your actual namespace
using CleverTagProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<CleverTagContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Charge"),
        new MySqlServerVersion(new Version(11, 3, 2)) // Use the actual version of your MariaDB server
    )
);// Configure DbContext to use MariaDB
builder.Services.AddDbContext<CleverTagContext>(opt =>
    opt.UseInMemoryDatabase("TagCharge"));

// Add the rest of the service configurations
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

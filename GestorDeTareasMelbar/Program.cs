using GestorDeTareasMelbar.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Define la política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddRazorPages();

// Configuración de MySQL con EF Core
builder.Services.AddDbContext<MelbarDB>(options => {
    string connnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(connnectionString, ServerVersion.AutoDetect(connnectionString)); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Usa CORS antes del middleware de controladores
app.UseCors("PermitirAngular");

app.MapControllers();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

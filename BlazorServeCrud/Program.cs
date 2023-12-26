using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Blazored.Toast;
using BlazorServeCrud.AuthProviders;
using BlazorServeCrud.Data;
using BlazorServeCrud.Models;
using BlazorServeCrud.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredSessionStorage();
// Local Storage//
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn")), ServiceLifetime.Transient);
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<AuthenticationStateProvider, TestAuthStateProvider>();
builder.Services.AddScoped<ClaimsPrincipal>(s =>
{
    var stateprovider = s.GetRequiredService<AuthenticationStateProvider>();
    var state = stateprovider.GetAuthenticationStateAsync().Result;
    return state.User;
});
//builder.Services.AddScoped<ToastService>();

builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            

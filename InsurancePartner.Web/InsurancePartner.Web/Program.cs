using FluentValidation;
using FluentValidation.AspNetCore;
using InsurancePartner.Data.DependencyInjection;
using InsurancePartner.Logic.DependencyInjection;
using InsurancePartner.Web.Models.PartnerViewModels;
using InsurancePartner.Web.Models.PolicyViewModels;
using InsurancePartner.Web.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddLogicLayer();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<IValidator<CreatePartnerViewModel>, CreatePartnerViewModelValidator>();
builder.Services.AddScoped<IValidator<EditPartnerViewModel>, EditPartnerViewModelValidator>();

builder.Services.AddScoped<IValidator<CreatePolicyViewModel>, CreatePolicyViewModelValidator>();
builder.Services.AddScoped<IValidator<EditPolicyViewModel>, EditPolicyViewModelValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

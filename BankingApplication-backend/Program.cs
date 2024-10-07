using BankingApplication_backend.Data;
using BankingApplication_backend.Repository;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using CloudinaryDotNet;


var builder = WebApplication.CreateBuilder(args);
var account = new Account(
    "dtdcdlkf9", // Replace with your cloud_name
    "616397431425944", // Replace with your api_key
    "DMilBZ6uBXR_2hNZcm35n0OVNL0" // Replace with your api_secret
);
Cloudinary cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankingAppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("database")));
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IInboundService, InboundService>();
builder.Services.AddScoped<ISalaryRepository, SalaryRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();//
builder.Services.AddScoped<IAdminRepo, AdminRepo>();//
builder.Services.AddScoped<IOrgService, OrgService>();//
builder.Services.AddScoped<IOrgRepo, OrgRepo>();//
builder.Services.AddScoped<IBankService, BankService>();//
builder.Services.AddScoped<IBankRepo, BankRepo>();//
builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();//
builder.Services.AddScoped<IDocumentService, DocumentService>();//
builder.Services.AddScoped<IEmpTransactionRepository, EmpTransactionRepository>();//
builder.Services.AddScoped<IEmpTransactionService, EmpTransactionService>();//
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthRepo>();//
builder.Services.AddScoped<IMailService, MailService>();//
builder.Services.AddScoped<IEmpRepo, EmpRepo>();//
builder.Services.AddScoped<IEmpService, EmpService>();//
builder.Services.AddScoped<IClientTransactionRepo, ClientTransactionRepo>();//
builder.Services.AddScoped<IClientTransactionService, ClientTransactionService>();//
builder.Services.AddScoped<IDownloadRepo, DownloadRepo>();
builder.Services.AddScoped<IDownloadService, DownloadService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

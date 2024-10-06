using BankingApplication_backend.Data;
using BankingApplication_backend.Repository;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankingAppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("database2")));

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
builder.Services.AddScoped<AuthService>();//
builder.Services.AddScoped<AuthRepo>();//
builder.Services.AddScoped<IMailService, MailService>();//
builder.Services.AddScoped<IEmpRepo, EmpRepo>();//
builder.Services.AddScoped<IEmpService, EmpService>();//
builder.Services.AddScoped<IClientTransactionRepo, ClientTransactionRepo>();//
builder.Services.AddScoped<IClientTransactionService, ClientTransactionService>();//

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

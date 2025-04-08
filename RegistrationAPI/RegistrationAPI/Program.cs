using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Data;
using RegistrationAPI.Services;
using RegistrationAPI.Utils;
using RegistrationAPI.Validators;

//BUILDER
var builder = WebApplication.CreateBuilder(args);

//GLOBAL CONFIG
Configuration.Settings = builder.Configuration;

//MODEL CUSTOM RESPONSE
builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options => {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        return HttpErrors.BadRequest(data: "Invalid data model");
                    };
                });

//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));

//SERVICES
builder.Services.AddDbContext<EmployeesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

//VALIDATORS
builder.Services.AddScoped<IEmployeeValidator, EmployeeValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();

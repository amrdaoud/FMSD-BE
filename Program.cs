using FMSD_BE.Data;
using FMSD_BE.Services.DashboardService;
using FMSD_BE.Services.ReportService.AlarmService;
using FMSD_BE.Services.ReportService.DistributionTransactionService;
using FMSD_BE.Services.ReportService.TankService;
using FMSD_BE.Services.ReportService.TransactionDetailService;
using FMSD_BE.Services.SharedService;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CentralizedFmsCloneContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAlarmService, AlarmService>();
builder.Services.AddScoped<ITankService, TankService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<IDistributionTransactionService, DistributionTransactionService>();
builder.Services.AddScoped<ITransactionDetailService, TransactionDetailService>();


builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin you can change here to allow localhost:4200
            .AllowCredentials();
}));


builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();

// <copyright file="Program.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Reflection;
using Microsoft.OpenApi.Models;
using Pomodoro.Api.ActionFilterAttributes;
using Pomodoro.DataAccess.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var pomodoroSpecificOrigins = "_pomodoroSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAppDbContext(builder.Configuration.GetConnectionString("LocalDB"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: pomodoroSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

// setup Serilog
 // builder.Host.UseSerilog((ctx, lc) => lc //
 //   .ReadFrom.Configuration(ctx.Configuration) //
 //   .WriteTo.MSSqlServer( //
 //       connectionString: //
 //       ctx.Configuration.GetConnectionString("PomodoroBE"), //
 //       restrictedToMinimumLevel: LogEventLevel.Information, //
 //       sinkOptions: new MSSqlServerSinkOptions //
 //       { //
 //           TableName = "LogEvents", //
 //           AutoCreateSqlTable = true, //
 //       } //
 //       ) //
 //   .WriteTo.Console() //
 // ); //
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pomodoro",
        Version = "v1",
        Description = "Time tracker",
    });
    option.EnableAnnotations();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// uncomment, if want logging HTTP requests
// app.UseSerilogRequestLogging(); //
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(pomodoroSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

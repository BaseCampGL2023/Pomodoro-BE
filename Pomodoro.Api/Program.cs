// <copyright file="Program.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Pomodoro.Api.ActionFilterAttributes;
using Pomodoro.Api.Extensions;
using Pomodoro.Api.Services;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.DataAccess.Extensions;
using Pomodoro.Services.Realizations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var pomodoroSpecificOrigins = "_pomodoroSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddAppDbContext(builder.Configuration.GetConnectionString("LocalDB"));

builder.Services.AddRepositories();

builder.Services.AddIdentityEF();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IFrequencyService, FrequencyService>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ISettingsService, SettingsService>();

builder.Services
    .AddJwtAuthentication(builder.Configuration)
    .AddGoogleAuthentication(builder.Configuration);

builder.Services.AddCookiesForExternalAuth();

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

    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        },
    };

    option.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() },
    });
});

var app = builder.Build();

app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.

// uncomment, if want logging HTTP requests
// app.UseSerilogRequestLogging(); //
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseUpdateDb();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(pomodoroSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

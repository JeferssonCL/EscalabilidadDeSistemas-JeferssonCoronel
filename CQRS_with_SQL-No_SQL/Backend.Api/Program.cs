using Backend.Data.Data.SQL_Read;
using Backend.Data.Data.SQL_Write;
using Backend.Business;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policyBuilder => policyBuilder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});

builder.Configuration.AddEnvironmentVariables();

string connectionString = Env.GetString("POSTGRES_SQL_CONNECTION")?? throw new ArgumentNullException("POSTGRES_SQL_CONNECTION environment variable is not set.");

builder.Services.AddDbContext<DbContext, PostgresContext>(options =>
    options.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("Backend.Api"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information)
);

string mongoConnection = Env.GetString("MONGO_CONNECTION") ?? throw new ArgumentNullException("MONGO_CONNECTION environment variable is not set.");
builder.Services.Configure<MongoSettings>(options =>
{
    options.ConnectionString = mongoConnection;
    options.DatabaseName = Env.GetString("MONGO_DB_NAME") ?? "default_db";
});

builder.Services.AddSingleton<MongoContext>();

builder.Services.AddConfigurations();  
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
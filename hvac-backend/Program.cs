using System.Text.Json.Serialization;
using hvac_backend.config;
using hvac_backend.database;
using hvac_backend.products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using hvac_backend.utilities;
using hvac_backend.global.transfomers;

var builder = WebApplication.CreateBuilder(args);
// DotEnv //
// Xác định môi trường hiện tại
var envName = builder.Environment.EnvironmentName.ToLower();
// Load file env theo môi trường
var envFile = $".env.{envName}";
if (File.Exists(envFile)) {
  Env.Load(envFile);
  Console.WriteLine($"Loaded {envFile}");
}
else {
  Console.WriteLine($"{envFile} was not found. Skipping loading env file.");
}
// Khởi tạo port cho server
string host = Environment.GetEnvironmentVariable("HOST") ?? builder.Configuration["AppSetting:Host"] ?? "localhost";
string port = Environment.GetEnvironmentVariable("PORT") ?? builder.Configuration["AppSetting:Port"] ?? "5000";

// builder.WebHost.UseUrls($"https://{host}:{port}");
builder.WebHost.ConfigureKestrel(options => {
  options.ListenAnyIP(8000); // http
  if (short.TryParse(port, out short portNumber)) {
    options.ListenAnyIP(portNumber, listenOptions => {
      listenOptions.UseHttps(); // https
    });
  }
});
Console.WriteLine($"Server is running in port: {port}");
// Kết nối Database
string? rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (rawConnectionString != null) {
  string connectionString = Interpolator.EnvInterpolate(rawConnectionString);
  builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
}
else {
  Console.WriteLine("DefaultConnection is null");
}


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ProductService>();
builder.Services.AddControllers().AddJsonOptions(options => {
  options.JsonSerializerOptions.Converters.Add(new JsonEnumConverterFactory());
  options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
  options.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
});

// Fluent Validation
FluentValidationConfig.Register(builder);
// Mapster
MapsterConfig.Register();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();

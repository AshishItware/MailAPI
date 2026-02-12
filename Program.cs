using MailAPI.Services;
using MailAPI.Services.IService;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Services
// =============================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmailService, EmailService>();

// Increase upload size limit (important for attachments)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
});

// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// =============================
// App build
// =============================

var app = builder.Build();

// =============================
// Middleware
// =============================

// Swagger (enabled in production for testing)
app.UseSwagger();
app.UseSwaggerUI();

// CORS must come early
app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

// Redirect root ? Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();

// Render port binding
app.Urls.Add("http://0.0.0.0:10000");

app.Run();

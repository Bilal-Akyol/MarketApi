using MarketApi.Extensions;
using MarketData.Concrete.Ef;
using MarketEntity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// DbContext'i connection string ile net bağla (kurumsal doğru kullanım)
builder.Services.AddDbContext<MarketDbContext>();

// Jwt ayarlarını appsettings.json -> "Jwt" bölümünden bind et
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<Jwt>();

// Jwt config eksikse başta patlat ki runtime'da gizli hata olmasın
if (jwt == null || string.IsNullOrWhiteSpace(jwt.Key) ||
    string.IsNullOrWhiteSpace(jwt.Issuer) || string.IsNullOrWhiteSpace(jwt.Audience))
    throw new Exception("Jwt config eksik (Key/Issuer/Audience).");

// JWT Bearer doğrulama kuralları
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,                 // Issuer kontrolü
            ValidIssuer = jwt.Issuer,

            ValidateAudience = true,               // Audience kontrolü
            ValidAudience = jwt.Audience,

            ValidateIssuerSigningKey = true,       // İmza kontrolü (Key)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),

            ValidateLifetime = true,               // Süre kontrolü
            ClockSkew = TimeSpan.Zero              // 5dk toleransı kapat
        };
    });

// Base64 gibi büyük JSON body gelirse limit (10MB)
builder.WebHost.ConfigureKestrel(o =>
    o.Limits.MaxRequestBodySize = 10 * 1024 * 1024
);
builder.Services.AddDependency(); //DI kayıtlarını uygular


builder.Services.AddControllers();

// Swagger (basit kurulum)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Market API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Bearer token gir. Örnek: Bearer eyJhbGciOi..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Swagger sadece Development'ta
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  Token'ı okuyup kullanıcıyı oluşturur (User.Identity) yoksa Authorize çalışmaz
app.UseAuthentication();

// Authorization, Authentication'dan sonra olmalı
app.UseAuthorization();

app.MapControllers();


app.Run();

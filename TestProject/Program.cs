using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestProject.DbContexts;
using TestProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmployContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:EmployDbConnectionString"]));
builder.Services.AddScoped<IEmplyInfoRepository, EmployInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options => {
    options.TokenValidationParameters = new() {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
               Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
    };
});
builder.Services.AddAuthorization(options => {
    options.AddPolicy("MustBeOverTheAgeOf30", policy => {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion((context) => {
            var claim = context.User.FindFirst("age");
            if (claim is null) return false;
            return int.Parse(claim.Value) >= 30;
        });
    });
});

builder.Services.AddApiVersioning(setupAction => {
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
}).AddMvc();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

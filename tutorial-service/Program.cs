using OlympicsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        //choose the websites we want to allow in our cors policy (dev mode)
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

    options.AddPolicy("ProdCors", (corsBuilder) =>
    {
        //choose the websites we want to allow in our cors policy (production)
        corsBuilder.WithOrigins("https://myProductionSite.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

});
//KEY - Creating an link between the interface that models call and the base repository class
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();

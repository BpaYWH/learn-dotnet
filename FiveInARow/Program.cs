using Microsoft.EntityFrameworkCore;
using FiveInARow.Context;
using FiveInARow.Services.FiveInARow;

// for dependency injection & configuration
var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IGameRecordService, GameRecordService>();

    builder.Services.AddDbContext<FiveInARowDbContext>(options => 
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("GomokuDb"));
    });

    // Add services to the container.
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    // Authorization, Authentication, CORS, etc.

    app.Run();
}

using Microsoft.EntityFrameworkCore;
using FiveInARow.Context;
using FiveInARow.Services.FiveInARow;
using FiveInARow.Hubs;
using FiveInARow.Repositories;

// for dependency injection & configuration
var builder = WebApplication.CreateBuilder(args);

{

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
    });

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

    builder.Services.AddSignalR();
    builder.Services.AddMvc();
    builder.Services.AddScoped<IGameRepository, GameRepository>();
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

    app.UseCors();
    app.MapHub<GameHub>("/gameHub");
    // Authorization, Authentication, CORS, etc.

    app.Run();
}

using API_Dotnet_PostSQL.Data;
using API_Dotnet_PostSQL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<Aerowdb>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add a new employee

app.MapPost("/employees/", async (Employee e, Aerowdb db) =>
{
    db.Employees.Add(e);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/{e.Id}", e);
});

// Find an employee by ID

app.MapGet("/employees/{id:int}", async (int id, Aerowdb db) =>
{
    return await db.Employees.FindAsync(id)
    // If the employee exists
        is Employee e
        // Then
        ? Results.Ok(e)
        // Else
        : Results.NotFound();
});

// Show all employees

app.MapGet("/employees", async (Aerowdb db) =>
    await db.Employees.ToListAsync());

// Update employee's info

app.MapPut("/employees/{id:int}", async (int id, Employee e, Aerowdb db) =>
{
    // If the user id is different than the one in the URL
    if (e.Id != id)
        return Results.BadRequest();

    var employee = await db.Employees.FindAsync(id);

    // If empty
    if (employee is null)
        return Results.NotFound();

    // If not null then we update
    employee.FirstName = e.FirstName;
    employee.LastName = e.LastName;
    employee.Branch = e.Branch;
    employee.Seniority = e.Seniority;

    await db.SaveChangesAsync();
    return Results.Ok(employee);
});

// Delete an employee

app.MapDelete("/employees/{id:int}", async (int id, Aerowdb db) =>
{
    var employee = await db.Employees.FindAsync(id);
    // If empty
    if (employee is null)
        return Results.NotFound();
    // Else
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
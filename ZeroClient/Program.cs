using NetMQ;
using NetMQ.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/sendmessage", () =>
{
    using (var client = new RequestSocket())
    {
        client.Connect("tcp://ZeroServer:5555");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Sending Hello");
            client.SendFrame("Hello");
            var message = client.ReceiveFrameString();
            Console.WriteLine("Received {0}", message);
        }
    }
})
.WithName("SendMessage")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

using apicooperativa.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Controllers
builder.Services.AddControllers();

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 MySQL Context
builder.Services.AddSingleton<MySqlContext>();


builder.Services.AddAuthorization();

var app = builder.Build();

// 🔹 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔐 ORDEN IMPORTANTE
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


using apicooperativa.Data;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// CONFIGURACIÓN DE SERVICIOS
// ==============================

// Controllers
builder.Services.AddControllers();

// Swagger (opcional, pero recomendado)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MySQL Context
builder.Services.AddSingleton<MySqlContext>();

// CORS (por si consumes la API desde frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ==============================
// BUILD APP
// ==============================

var app = builder.Build();

// ==============================
// MIDDLEWARE
// ==============================

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANTE PARA CLEVER CLOUD
// Usa el puerto que Clever inyecta
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// ==============================
// RUN
// ==============================

app.Run();

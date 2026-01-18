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

// Swagger SIEMPRE activo
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Cooperativa v1");
});


// IMPORTANTE PARA CLEVER CLOUD
// Usa el puerto que Clever inyecta


app.MapControllers();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// ==============================
// RUN
// ==============================

app.Run();

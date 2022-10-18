using BlazorAppMagicOnion.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// must be added after UseRouting and before UseEndpoints 
//app.UseGrpcWeb();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

//app.UseAuthorization();

//app.MapControllers();

app.UseCors();

////// map to and register the gRPC service
//app.MapGrpcService<IMyFirstService>().EnableGrpcWeb();

//// Configure the HTTP request pipeline.
app.MapMagicOnionService().EnableGrpcWeb().RequireCors("AllowAll");

//app.MapMagicOnionService().EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
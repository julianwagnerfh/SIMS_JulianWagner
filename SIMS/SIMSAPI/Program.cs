
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;

namespace SIMSAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis:6379"));

            builder.Services.AddControllers();

            builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// JSON kein Loop vorsorgen; kein Camelcase vorsorgen
			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
				options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


			var app = builder.Build();

			//Enable CORS
			app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}

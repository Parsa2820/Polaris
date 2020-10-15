using API.Services.EdgeBusiness;
using API.Services.GraphBusiness;
using API.Services.Importer;
using API.Services.NodeBusiness;
using Database.Communication;
using Database.Communication.Elastic.Nest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Banking;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            NestClientFactory.GetInstance().CreateInitialClient(Configuration["ElasticAddress"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddSingleton<IEntityHandler<BankAccount, string>, NestEntityHandler<BankAccount, string>>();
            services.AddSingleton<IEntityHandler<Transaction, string>, NestEntityHandler<Transaction, string>>();
            services.AddSingleton<IImporterService<BankAccount>, ElasticImporterService<BankAccount>>();
            services.AddSingleton<IImporterService<Transaction>, ElasticImporterService<Transaction>>();
            services.AddSingleton<INodeService<BankAccount, string>, NodeService<BankAccount, string>>();
            services.AddSingleton<IEdgeService<Transaction, string, string>, EdgeService<Transaction, string, string>>();
            services.AddSingleton<IGraphService<string, BankAccount, string, Transaction>, GraphService<string, BankAccount, string, Transaction>>();
            services.AddSingleton<IDatabaseHandler<BankAccount>, NestElasticHandler<BankAccount>>();
            services.AddSingleton<IDatabaseHandler<Transaction>, NestElasticHandler<Transaction>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>  // can be checked
                        options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

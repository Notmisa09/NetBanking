using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Interfaces.IServices;
using NetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void IdentityLayerRegistration(this IServiceCollection service, IConfiguration configuration)
        {
            #region Context
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<ApplicationContext>(options =>
                                                        options.UseInMemoryDatabase("Twitter"));
            }
            else
            {
                service.AddSingleton<UpdateAuditableEntitiesInterceptor>();
                service.AddDbContext<ApplicationContext>((sp, options) =>
                {
                    var interceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
                    options.UseSqlServer(configuration.GetConnectionString("Default"),
                    m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)).AddInterceptors(interceptor);
                });

            }
            #endregion

            #region Context
            #endregion

            service.AddTransient<ApplicationContext, ApplicationContext>();
        }
    }
}

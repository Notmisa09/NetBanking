using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Infrastructure.Persistence.Contexts;
using NetBanking.Infrastructure.Persistence.Interceptor;
using NetBanking.Infrastructure.Persistence.Repositories;

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

            #region Repositories 
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddTransient<IBeneficiaryRepository, BeneficiaryRepository>();
            service.AddTransient<ICreditCardRepository, CreditCardRepository>();
            service.AddTransient<ILoanRepository, LoanRepository>();
            service.AddTransient<ISavingsAccountRepository, SavingsAccountRepository>();
            service.AddTransient<ITransactionRepository, TransactionRepository>();
            #endregion
        }
    }
}

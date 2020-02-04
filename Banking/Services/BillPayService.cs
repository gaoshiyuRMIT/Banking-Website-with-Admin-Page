using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using BankingLib.Models;
using BankingLib.Data;

namespace Banking.Services
{
    public class BillPayService : IHostedService, IDisposable
    {
        private IServiceProvider _services;
        private static readonly TimeSpan interval = new TimeSpan(0, 0, 1);
        private System.Timers.Timer _timer;
        private ILogger<BillPayService> _logger;

        public BillPayService(IServiceProvider services)
        {
            _services = services;
            _logger = services.GetRequiredService<ILogger<BillPayService>>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleBillPay(cancellationToken);
        }

        public async Task ScheduleBillPay(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer(interval.TotalMilliseconds);
            _timer.Elapsed += async (sender, args) =>
            {
                _timer.Stop();
                await ExecuteBillPays(cancellationToken);
                await ScheduleBillPay(cancellationToken);
            };
            _timer.Start();
            await Task.CompletedTask;
        }

        private async Task ExecuteBillPays(CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var context = new BankingContext(
                        scope.ServiceProvider
                            .GetRequiredService<DbContextOptions<BankingContext>>());
                    var now = DateTime.UtcNow;
                    var billPaysQ = context.BillPay.Where(x =>
                        x.Status != BillPayStatus.Blocked &&
                        x.ScheduleDate > now - interval && x.ScheduleDate <= now);
                    var billPays = await billPaysQ.ToListAsync();
                    foreach (var billPay in billPays)
                    {
                        string errMsg;
                        if (billPay.ExecuteBillPay(out errMsg))
                        {
                            await context.SaveChangesAsync();
                            _logger.LogInformation($"Executed BillPay {billPay.BillPayID}");
                        }
                        else
                        {                            
                            await context.SaveChangesAsync();
                            _logger.LogError($"BillPay {billPay.BillPayID}({billPay.ScheduleDateLocal}) execution failed. {errMsg}");
                        }
                    }
                }
            } catch (Exception e)
            {
                _logger.LogError(0, e, "An exception occured when executing bill pays");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
using Azure;
using remindME.Core;
using remindME.Core.Models;

namespace reminderMEService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<Reminder> lista = new List<Reminder>();
            while (!stoppingToken.IsCancellationRequested)
            {
                await reminderMEService.reminders.getReminders(DateTime.UtcNow);
                foreach (var item in lista)
                {
                    await reminderMEService.reminders.sentReminder(item.title, item.message);
                }
                _logger.LogInformation("Procesando: {time}", DateTime.UtcNow);
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
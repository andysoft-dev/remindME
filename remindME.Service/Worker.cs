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
                _logger.LogInformation("Procesando: {time} UTC ({local})", DateTime.UtcNow, DateTime.Now);
                lista = await reminderMEService.reminders.getReminders(DateTime.UtcNow);
                foreach (var item in lista)
                {
                    bool resp = await reminderMEService.reminders.sentReminder(item.title, item.message);
                    if (resp)
                    {
                        item.sent = true;
                        bool repSave = await reminderMEService.reminders.setReminder(item);
                    }
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
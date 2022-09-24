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
                _logger.LogInformation("Processing: {time} UTC ({local})", DateTime.UtcNow, DateTime.Now);

                try
                {
                    lista = await reminderMEService.reminders.getReminders(DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error accesing to CosmosDB");
                }
                try
                {
                    foreach (var item in lista)
                    {
                        bool resp = await reminderMEService.reminders.sentReminder(item);
                        if (resp)
                        {
                            item.sent = true;
                            bool repSave = await reminderMEService.reminders.setReminder(item);
                        }
                        GC.Collect();

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending reminder");
                }
                    
                
                
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
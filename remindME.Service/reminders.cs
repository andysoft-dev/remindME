using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reminderMEService
{
    public  class reminders
    {
        public static async Task<List<remindME.Core.Models.Reminder>> getReminders(DateTime fec)
        {
            string dataEncr = "";
            string connCosmos = "";
            remindME.Core.Models.Reminder item = new remindME.Core.Models.Reminder();
            string ruta = "";

            ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "remindME");

            dataEncr = andysoft.utiles.ReaderConfig.leeArchivoConfig(Path.Combine(ruta, "config.properties"));
            connCosmos = andysoft.utiles.ReaderConfig.leePropiedadDesdeStringEncriptado(dataEncr, "cosmos");


            var dbClient = await remindME.Core.CosmosClass.InitializeCosmosClientInstanceAsync(connCosmos, "reminders", "reminder");

            return await dbClient.GetReminders(fec);
            //dbClient.GetItemsAsync()

            //IQueryable<remindME.Core.Models.Reminder> list = container.GetItemLinqQueryable<Order>(allowSynchronousQueryExecution: true).Where(o => o.ShipDate >= DateTime.UtcNow.AddDays(-3));
        }

        public static async Task sentReminder(string title, string message)
        {
            string ruta = "";
            string dataEncr = "";
            string paraMail = "";
            string apiKey = "";

            ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "remindME");

            dataEncr = andysoft.utiles.ReaderConfig.leeArchivoConfig(Path.Combine(ruta, "config.properties"));
            paraMail = andysoft.utiles.ReaderConfig.leePropiedadDesdeStringEncriptado(dataEncr, "email");
            apiKey = andysoft.utiles.ReaderConfig.leePropiedadDesdeStringEncriptado(dataEncr, "apiKey");

            await andysoft.utiles.Envios.enviarEmail(apiKey, title, paraMail, paraMail, message, message);
        }
    }
}

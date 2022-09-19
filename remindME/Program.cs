//reminderME
//Creator: Andy Jara
//andy@desarrollador.cl
//Create a short messages in console and receive reminders in your e-mail
// See https://aka.ms/new-console-template for more information

using andysoft.utiles;
using Microsoft.Azure.Cosmos;
using remindME.Core.Cosmos;
using System.CommandLine;
using System.Configuration;
using System.Runtime.InteropServices;

namespace remindME;


class Program
{
    static async Task<int> Main(string[] args)
    {
        string? apiKey = "";
        string? email = "";

        List<string> valoresConfig = new List<string>();
        string desencriptado = "";
        string? encriptado = "";
        string? cosmos = "";


        andysoft.utiles.Crypto3DES cr = new andysoft.utiles.Crypto3DES();


        if (args.Length > 0)
        {

            var mensajeOption = new Option<string?>(name: "--message", description: "Reminder message.");
            var tituloOption = new Option<string?>(name: "--title", description: "Reminder title");
            var fechaRecordatorioOption = new Option<string?>(name: "--time", description: "Reminder date in yyyy-mm-dd hh:mm format");



            var rootCommand = new RootCommand("remindMe: Create reminders from the command line. (Author: Andy Jara, Santiago Chile)");
            var grabaCommand = new Command("save", "Save a new reminder.")
            {
                mensajeOption,
                tituloOption,
                fechaRecordatorioOption,
            };

            rootCommand.AddCommand(grabaCommand);

            rootCommand.AddOption(mensajeOption);


            grabaCommand.SetHandler(async (mensaje, titulo, fecha) =>
            {

                string dataEncr = "";
                string connCosmos = "";
                string ruta = "";
                Core.Models.Reminder item = new Core.Models.Reminder();

                ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "remindME");

                dataEncr = andysoft.utiles.ReaderConfig.leeArchivoConfig(Path.Combine(ruta, "config.properties"));
                andysoft.utiles.ReaderConfig.leePropiedadDesdeStringEncriptado(dataEncr, "apiKey");
                andysoft.utiles.ReaderConfig.leePropiedadDesdeStringEncriptado(dataEncr, "email");
                connCosmos = andysoft.utiles.ReaderConfig.leePropiedadDesdeStringEncriptado(dataEncr, "cosmos");

                if (String.IsNullOrEmpty(titulo))
                {
                    Console.WriteLine("Please type the title");
                    return;
                }
                if (String.IsNullOrEmpty(mensaje))
                {
                    Console.WriteLine("Please type the message");
                    return;
                }
                if (String.IsNullOrEmpty(fecha))
                {
                    Console.WriteLine("Please type the datetime");
                    return;
                }
                DateTime fec;
                if (!DateTime.TryParse(fecha, out fec))
                {
                    Console.WriteLine("The datetime is not valid");
                    return;
                }

                Console.WriteLine("Recording reminder...");
                var dbClient = await Core.CosmosClass.InitializeCosmosClientInstanceAsync(connCosmos, "reminders", "reminder");
                //var dbClient = await InitializeCosmosClientInstanceAsync(connCosmos, "reminders", "reminder");
                item.title = titulo;
                item.message = mensaje;
                item.datetime = fec.ToUniversalTime();
                item.id = System.Guid.NewGuid().ToString();
                item.sent = false;
                
                await dbClient.AddItemAsync(item);
                Console.WriteLine("Reminder added!");


            }, 
            mensajeOption, tituloOption, fechaRecordatorioOption);

            return await rootCommand.InvokeAsync(args);
        }
        else
        {
            //Si no hay argumentos, entro en modo configuracion
            Console.WriteLine("Please ingress the SendGrid API Key (Permission for to send mails)");
            apiKey = Console.ReadLine();
            if (apiKey == "")
            {
                Console.WriteLine("Please type de api key");
                return(1);
            }
            Console.WriteLine("Please type a valid e-mail for reminders");
            email = Console.ReadLine();
            if (email == "")
            {
                Console.WriteLine("Please type a email");
                return(1);
            }
            Console.WriteLine("Please ingress the connection string from CosmosDB Azure Account");
            cosmos = Console.ReadLine();
            if (cosmos == "")
            {
                Console.WriteLine("Please type a correct connection string from CosmosDB");
                return (1);
            }

            valoresConfig = new List<string>();
            valoresConfig.Add(String.Concat("apiKey=", apiKey));
            valoresConfig.Add(String.Concat("email=", email));
            valoresConfig.Add(String.Concat("cosmos=", cosmos));


            desencriptado = string.Join(Environment.NewLine, valoresConfig);
            encriptado = cr.Encrypt3DES(desencriptado);
            andysoft.utiles.WriteConfig gr = new andysoft.utiles.WriteConfig();
            string ruta ="";

            ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "remindME");
            if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);

            gr.grabaData(Path.Combine(ruta, "config.properties"), encriptado);


            //Console.WriteLine(ReaderConfig.leePropiedadDesdeStringEncriptado(encriptado, "email"));
            return (0);
        }




    }

    /// <summary>
    /// Creates a Cosmos DB database and a container with the specified partition key. 
    /// </summary>
    /// <returns></returns>
    private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(string connectionString, string db, string container)
    {
        
        Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(connectionString);
        CosmosDbService cosmosDbService = new CosmosDbService(client, db, container);
        Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(db);
        await database.Database.CreateContainerIfNotExistsAsync(container, "/datetime");

        return cosmosDbService;
    }
}



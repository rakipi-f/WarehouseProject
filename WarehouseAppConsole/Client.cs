using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace WarehouseAppConsole
{
    internal class Client
    {
        //Creo questa classe per i movimenti che riceverò
        public class WarehouseMovementDto
        {
            public int Id { get; set; }
            public System.DateTime Date { get; set; }
            public int ProductId { get; set; }
            public double Qty { get; set; }

        }
        // creo questa classe per deserializzare poi il json
        public class ViewModelConsole
        {
            public List<WarehouseMovementDto> Movements { get; set; }
            public double Giacenza { get; set; }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Benvenuto nel sistema di monitoraggio del magazzino\n");

            while (true)
            {
                try
                {
                    Console.WriteLine("Inserisci l'id di un prodotto:");
                    var id = Console.ReadLine();
                    Console.Clear();
                    string BaseUrl = WebConfigurationManager.AppSettings["BaseUrl"];
                    //Console.WriteLine(BaseUrl);
                    var client = new RestClient(BaseUrl);
                    var request = new RestRequest("{id}");
                    request.AddParameter("id", id, ParameterType.UrlSegment);
                    //faccio la chiamata
                    Console.WriteLine("Caricamento...");
                    var response = await client.GetAsync(request);
                    Console.Clear();

                    
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("Errore: Id non esistente o nessun movimento tracciato");
                    }
                    else if (response.IsSuccessful)
                    {
                        ViewModelConsole movements = JsonConvert.DeserializeObject<ViewModelConsole>(response.Content);
                        //le specifiche per l'app console non danno informazioni precise così decido che nel caso
                        //essa sia negativa, stampo 0, ovvero che il prodotto non è presente in magazzino
                        if (movements.Giacenza < 0)
                            movements.Giacenza = 0;


                        Console.WriteLine("Giacenza totale: " + movements.Giacenza + "\n");

                        foreach (var item in movements.Movements)
                        {
                            Console.WriteLine("Id movimento: " + item.Id);
                            Console.WriteLine("Data: " + item.Date);
                            Console.WriteLine("Quantità: " + item.Qty + "\n");
                        }
                    }

                    else
                    {
                        Console.WriteLine("Errore: " + response.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errore: " + ex.Message);

                }
            }

        }

    }
}

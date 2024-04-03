using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel;



namespace AirTek
{
    internal class Program
    {
        private static List<Flight> flights = new List<Flight>();
        static void Main(string[] args)
        {
            Flight flight1 = new Flight(1, "Flight: 1", "YUL", "YYZ", "day: 1");
            Flight flight2 = new Flight(2, "Flight: 2", "YUL", "YYC", "day: 1");
            Flight flight3 = new Flight(3, "Flight: 3", "YUL", "YVR", "day: 1");
            Flight flight4 = new Flight(4, "Flight: 4", "YUL", "YYZ", "day: 2");
            Flight flight5 = new Flight(5, "Flight: 5", "YUL", "YYC", "day: 2");
            Flight flight6 = new Flight(6, "Flight: 6", "YUL", "YVR", "day: 2");

            flights.Add(flight1);
            flights.Add(flight2);
            flights.Add(flight3);
            flights.Add(flight4);
            flights.Add(flight5);
            flights.Add(flight6);

            Dictionary<string, int> flightload = new Dictionary<string, int>();
            flightload.Add("Flight: 1", 0);
            flightload.Add("Flight: 2", 0);
            flightload.Add("Flight: 3", 0);
            flightload.Add("Flight: 4", 0);
            flightload.Add("Flight: 5", 0);
            flightload.Add("Flight: 6", 0);

            // User Story 1
            foreach (Flight f in flights) 
            {
                Console.WriteLine(f.Name+ ",Departure: "+f.Departure+ ", Arrival:"+ f.Arrival + "," + f.Day);
            }

            // User Story 2
            using (StreamReader r = new StreamReader("C:\\Users\\srini\\Downloads\\coding-assigment-orders.json"))
            {
                string json = r.ReadToEnd();               
                var Root = JsonConvert.DeserializeObject<Dictionary<string,Order>>(json);
                
                bool orderscheduled = false;
                foreach(var item in Root)
                {
                    var filteredflights = flights.Where(flight => flight.Arrival == item.Value.destination).OrderBy(x => x.Number);
                    foreach(Flight flight in filteredflights)
                    {                        
                        if (flightload[flight.Name] < 20 && !orderscheduled)
                        {
                            Console.WriteLine("order: " + item.Key + ", flight number: " + flight.Number + ", departure "
                                + flight.Departure + ", arrival " + flight.Arrival + ", Day " + flight.Day);
                            flightload[flight.Name] = flightload[flight.Name] + 1;
                            orderscheduled = true;                            
                        }                                                
                    }
                    
                    if (orderscheduled == false)
                    {
                        Console.WriteLine("order: " + item.Key + ", flightNumber: not scheduled");
                    }
                    orderscheduled = false;
                }
                
            }
        }
    }

    public class Flight
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Day { get; set; }
        public Flight() { }
        public Flight(int number, string name, string departure, string arrival, string day)
        {
            Number = number;
            Name = name;
            Departure = departure;
            Arrival = arrival;
            Day = day;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Order
    {
        public string destination { get; set; }
    }


}

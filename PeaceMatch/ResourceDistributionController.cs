using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace PeaceMatch
{
    public class ResourceDistributionController
    {
        // Simple HTTP server to handle API requests
        private HttpListener listener;
        private readonly string url = "http://localhost:8080/";
        
        public ResourceDistributionController()
        {
            // Initialize HTTP listener
            listener = new HttpListener();
            listener.Prefixes.Add(url);
        }
        
        public void Start()
        {
            listener.Start();
            Console.WriteLine("Listening for requests on " + url);
            
            // Start listening for requests
            Task.Run(() => HandleRequests());
        }
        
        public void Stop()
        {
            listener.Stop();
        }
        
        private async Task HandleRequests()
        {
            while (listener.IsListening)
            {
                try
                {
                    // Wait for a request
                    var context = await listener.GetContextAsync();
                    var request = context.Request;
                    var response = context.Response;
                    
                    // Set CORS headers to allow requests from the web browser
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("Access-Control-Allow-Methods", "GET");
                    response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                    
                    // Handle resource distribution data requests
                    if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/api/resource-distribution")
                    {
                        // Load data from database
                        Database.LoadPersons();
                        
                        // Generate resource distribution data
                        var responseData = GenerateResourceDistributionData();
                        
                        // Convert to JSON and send response
                        string json = new JavaScriptSerializer().Serialize(responseData);
                        byte[] buffer = Encoding.UTF8.GetBytes(json);
                        
                        // Set response details
                        response.ContentType = "application/json";
                        response.ContentLength64 = buffer.Length;
                        
                        // Write response
                        using (var output = response.OutputStream)
                        {
                            await output.WriteAsync(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        // Handle unsupported requests
                        response.StatusCode = 404;
                    }
                    
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error handling request: " + ex.Message);
                }
            }
        }
        
        private object GenerateResourceDistributionData()
        {
            try
            {
                // Dictionary for storing country coordinates
                var countryCoordinates = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Lebanon", new { lat = 33.8547, lng = 35.8623 } },
                    { "Syria", new { lat = 35.0178, lng = 38.5078 } }
                    // Add more countries as needed
                };
                
                // Dictionary for storing city coordinates
                var cityCoordinates = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Beirut", new { lat = 33.8938, lng = 35.5018 } },
                    { "Tripoli", new { lat = 34.4333, lng = 35.8333 } },
                    { "Damascus", new { lat = 33.5138, lng = 36.2765 } },
                    { "Aleppo", new { lat = 36.2021, lng = 37.1343 } }
                    // Add more cities as needed
                };
                
                // Group persons by country
                var countriesData = Database.allPersons
                    .GroupBy(p => p.Country)
                    .Select(countryGroup => {
                        var countryName = countryGroup.Key;
                        
                        // Calculate total resources for the country
                        var countryTotals = new
                        {
                            TotalFood = countryGroup.Sum(p => p.Food),
                            TotalWater = countryGroup.Sum(p => p.Water),
                            TotalClothes = countryGroup.Sum(p => p.Clothes),
                            TotalShelter = countryGroup.Sum(p => p.Shelter),
                            TotalWarmth = countryGroup.Sum(p => p.Warmth),
                            TotalSleep = countryGroup.Sum(p => p.SleepEssentials),
                            TotalSanitary = countryGroup.Sum(p => p.SanitaryProducts),
                            TotalFemaleSanitary = countryGroup.Sum(p => p.FemaleSanitaryProducts)
                        };
                        
                        // Group by cities and calculate resource percentages
                        var cities = countryGroup
                            .GroupBy(p => p.City)
                            .Select(cityGroup => {
                                var cityName = cityGroup.Key;
                                
                                // Get city coordinates
                                var position = cityCoordinates.ContainsKey(cityName) 
                                    ? cityCoordinates[cityName] 
                                    : new { lat = 0.0, lng = 0.0 };
                                
                                // Calculate resource percentages
                                return new
                                {
                                    name = cityName,
                                    position = position,
                                    resources = new
                                    {
                                        food = countryTotals.TotalFood > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.Food) / countryTotals.TotalFood * 100, 2) : 0,
                                        water = countryTotals.TotalWater > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.Water) / countryTotals.TotalWater * 100, 2) : 0,
                                        clothes = countryTotals.TotalClothes > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.Clothes) / countryTotals.TotalClothes * 100, 2) : 0,
                                        shelter = countryTotals.TotalShelter > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.Shelter) / countryTotals.TotalShelter * 100, 2) : 0,
                                        warmth = countryTotals.TotalWarmth > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.Warmth) / countryTotals.TotalWarmth * 100, 2) : 0,
                                        sleepEssentials = countryTotals.TotalSleep > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.SleepEssentials) / countryTotals.TotalSleep * 100, 2) : 0,
                                        sanitaryProducts = countryTotals.TotalSanitary > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.SanitaryProducts) / countryTotals.TotalSanitary * 100, 2) : 0,
                                        femaleSanitaryProducts = countryTotals.TotalFemaleSanitary > 0 ?
                                            Math.Round((double)cityGroup.Sum(p => p.FemaleSanitaryProducts) / countryTotals.TotalFemaleSanitary * 100, 2) : 0
                                    }
                                };
                            })
                            .ToList();
                        
                        // Get country coordinates
                        var center = countryCoordinates.ContainsKey(countryName) 
                            ? countryCoordinates[countryName] 
                            : new { lat = 0.0, lng = 0.0 };
                        
                        return new
                        {
                            name = countryName,
                            center = center,
                            cities = cities
                        };
                    })
                    .ToList();
                
                return new
                {
                    countries = countriesData
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating resource distribution data: " + ex.Message);
                return new { error = ex.Message };
            }
        }
    }
}
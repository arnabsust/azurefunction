using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

public class Source 
{
    public string id { get; set; }
    public string name { get; set; }
}
public class DataObject
{
    public string status { get; set; }
    public IEnumerable<Source> sources { get; set; }
}

public static void Run(TimerInfo timer, TraceWriter log)
{
    log.Info("============ HELLO ============");
    log.Info("############ CHECKING CONTINUOUS INTEGRATION ############");
    log.Info(ConfigurationManager.AppSettings["GET_SOURCE_URL"]);
    log.Info(ConfigurationManager.AppSettings["API_KEY"]);

    string urlParameters = "?api_key="+ConfigurationManager.AppSettings["API_KEY"];
    
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["GET_SOURCE_URL"]);

    // Add an Accept header for JSON format.
    client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

    // List data response.
    HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
    
    if (response.IsSuccessStatusCode)
    {
        // Parse the response body. Blocking!
        var dataObject = response.Content.ReadAsAsync<DataObject>().Result;
        log.Info(dataObject.status);
        foreach (var d in dataObject.sources)
        {
            log.Info(d.name);
        }
    }
    else
    {
        log.Info("failed");
    }  
}

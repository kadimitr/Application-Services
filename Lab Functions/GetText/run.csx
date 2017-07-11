  #r "System.IO"
  #r "System.Runtime"
  #r "System.Threading.Tasks"
  #r "Microsoft.WindowsAzure.Storage"
  #r "Newtonsoft.Json"

  using System;
  using System.Net;
  using System.IO;
  using System.Text;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.WindowsAzure.Storage.Table;
  using Newtonsoft.Json;

  public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IQueryable<ImageText> inputTable,  TraceWriter log)
  {
      log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

      var result = new List<SimpleImageText>();

      var query = from ImageText in inputTable select ImageText;
      //log.Info($"original query --> {JsonConvert.SerializeObject(query)}");

      foreach (ImageText imageText in query)
      {
          result.Add( new SimpleImageText(){Text = imageText.Text, Uri = imageText.Uri});
          //log.Info($"{JsonConvert.SerializeObject()}");
      }
    log.Info($"list of results --> {JsonConvert.SerializeObject(result)}");
      var jsonObject = JsonConvert.SerializeObject(result);
      var response = req.CreateResponse(HttpStatusCode.OK);
    response.Content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
    return response;
  }

  // used to get rows from table
  public class ImageText : TableEntity
  {
      public string Text { get; set; }
      public string Uri {get; set; }
  }

  public class SimpleImageText
  {
      public string Text { get; set; }
      public string Uri {get; set; }
  }
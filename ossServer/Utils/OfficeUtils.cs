using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ossServer.BaseResults;
using ossServer.Controllers.Dokumentum;
using RestSharp;
using System;
using System.Net;

namespace ossServer.Utils
{
    public class OfficeUtils
    {
        public static byte[] ToPdf(IConfiguration config, OfficeParam op)
        {
            string act;

            if (op.Ext == ".xls" | op.Ext == ".xlsx")
                act = "exceltopdf";
            else if (op.Ext == ".doc" | op.Ext == ".docx")
                act = "wordtopdf";
            else
                throw new Exception($"A(z) {op.Ext} fájlok nem konvertálhatók!");

            var url = config.GetValue<string>("OssOffice:url");
            var client = new RestClient
            {
                BaseUrl = new Uri(url + "api/office/" + act)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(op), ParameterType.RequestBody);

            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(response.Content);

            var result = JsonConvert.DeserializeObject<ByteArrayResult>(response.Content);
            if (!string.IsNullOrEmpty(result.Error))
                throw new Exception(result.Error);

            return result.Result;
        }
    }
}

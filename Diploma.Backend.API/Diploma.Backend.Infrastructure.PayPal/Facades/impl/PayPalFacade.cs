using RestSharp;
using Microsoft.Extensions.Configuration;
using Diploma.Backend.Infrastructure.PayPal.Models;
using Newtonsoft.Json;
using Diploma.Backend.Domain.Enums;

namespace Diploma.Backend.Infrastructure.PayPal.Facades.impl
{
    public class PayPalFacade : IPayPalFacade
    {
        private readonly IRestClient restClient;

        public PayPalFacade(IConfiguration provideConfiguration,
                                IRestClient restClient)
        {
            this.restClient = restClient;
            var configuration = new PayPalConfig(provideConfiguration);
            restClient.Timeout = configuration.TimeOut;
            restClient.BaseUrl = new Uri(configuration.BaseUrl);
        }

        public T Post<T>(string url,
                           object body = null,
                           Dictionary<string, string> urlParams = null,
                           Dictionary<string, string> headers = null,
                           string contentType = "application/json"
                           ) where T : new()
        {
            return Action<T>(url, Method.POST, body, urlParams, headers, contentType);
        }

        public T Get<T>(string url,
                           object body = null,
                           Dictionary<string, string> urlParams = null,
                           Dictionary<string, string> headers = null,
                           string contentType = "application/json"
                           ) where T : new()
        {
            return Action<T>(url, Method.GET, body, urlParams, headers, contentType);
        }

        public T Patch<T>(string url,
                   object body = null,
                   Dictionary<string, string> urlParams = null,
                   Dictionary<string, string> headers = null,
                   string contentType = "application/json"
                   ) where T : new()
        {
            return Action<T>(url, Method.PATCH, body, urlParams, headers, contentType);
        }

        private T Action<T>(string url, Method method, object body, Dictionary<string, string> queryParams, Dictionary<string, string> headers, string contentType) where T : new()
        {
            var request = new RestRequest(url, method);

            AddQueryParams(ref request, queryParams);
            AddHeaders(ref request, headers);
            AddBody(ref request, body, contentType);

            var response = restClient.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(ErrorCodes.PayPalCommunicationError.ToString());
            }

            return string.IsNullOrEmpty(response.Content) ? new T() : JsonConvert.DeserializeObject<T>(response.Content);
        }

        private void AddBody(ref RestRequest request, object body, string contentType)
        {
            if (body != null)
            {
                switch (contentType)
                {
                    case "application/json":
                        AddJsonBody(ref request, body);
                        break;
                    case "application/x-www-form-urlencoded":
                        AddFormBody(ref request, body as Dictionary<string, string>);
                        break;
                }
            }
        }

        private void AddFormBody(ref RestRequest request, Dictionary<string, string> body)
        {
            if (body != null && body.Any())
            {
                foreach (var parameter in body)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        }

        private void AddJsonBody(ref RestRequest request, object body)
        {
            if (body != null)
            {
                var jsonBody = body is string ? body : JsonConvert.SerializeObject(body);
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            }
            request.AddHeader("Content-Type", "application/json");
        }

        private void AddQueryParams(ref RestRequest request, Dictionary<string, string> urlParams)
        {
            if (urlParams != null)
            {
                foreach (var key in urlParams.Keys)
                {
                    request.AddParameter(key, urlParams[key], ParameterType.QueryString);
                }
            }
        }

        private void AddHeaders(ref RestRequest request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    request.AddParameter(key, headers[key], ParameterType.HttpHeader);
                }
            }
        }
    }
}

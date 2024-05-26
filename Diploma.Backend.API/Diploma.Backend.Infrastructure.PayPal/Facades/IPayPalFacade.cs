using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.PayPal.Facades
{
    public interface IPayPalFacade
    {
        T Post<T>(string url, object body = null, Dictionary<string, string> urlParams = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new();
        T Get<T>(string url, object body = null, Dictionary<string, string> urlParams = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new();
        T Patch<T>(string url, object body = null, Dictionary<string, string> urlParams = null, Dictionary<string, string> headers = null, string contentType = "application/json") where T : new();
    }
}

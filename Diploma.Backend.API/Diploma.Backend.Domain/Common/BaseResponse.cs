using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Common
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public Error Error { get; set; }
    }
}

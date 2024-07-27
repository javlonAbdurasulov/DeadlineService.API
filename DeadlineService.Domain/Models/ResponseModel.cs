using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models
{
    public class ResponseModel <T>
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ResponseModel()
        {
        }
        public ResponseModel(HttpStatusCode status, string? message = null)
        {
            Status = status;
            Message = message;
        }
        public ResponseModel(HttpStatusCode status, T data, string? message = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }
       

    }
}

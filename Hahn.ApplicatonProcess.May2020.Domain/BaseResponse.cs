using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.May2020.Domain
{
    public class BaseResponse<T>
    {
        public BaseResponse(bool isSuccessful, string message, T result)
        {
            this.IsSuccessful = isSuccessful;
            this.Message = message;
            this.Result = result;
        }

        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }

    }
}

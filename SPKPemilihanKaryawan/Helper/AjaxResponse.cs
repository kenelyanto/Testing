using System;

namespace SistemPendukungKeputusan.Helper
{
    public class AjaxResponse<TResult>
    {
        public bool Success { get; set; }

        public TResult Result { get; set; }

        public ErrorInfo Error { get; set; }

        public bool UnAuthorizedRequest { get; set; }

        public AjaxResponse(TResult result)
        {
            this.Result = result;
            this.Success = true;
        }

        public AjaxResponse()
        {
            this.Success = true;
        }

        public AjaxResponse(bool success)
        {
            this.Success = success;
        }

        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            this.Error = error;
            this.UnAuthorizedRequest = unAuthorizedRequest;
            this.Success = false;
        }
    }
        
    public class AjaxResponse : AjaxResponse<object>
    {
        public AjaxResponse()
        {
        }

        public AjaxResponse(bool success)
          : base(success)
        {
        }

        public AjaxResponse(object result)
          : base(result)
        {
        }

        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
          : base(error, unAuthorizedRequest)
        {
        }
    }

    public class ErrorInfo
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public ErrorInfo()
        {
        }

        public ErrorInfo(string message)
        {
            this.Message = message;
        }

        public ErrorInfo(int code)
        {
            this.Code = code;
        }

        public ErrorInfo(int code, string message)
          : this(message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
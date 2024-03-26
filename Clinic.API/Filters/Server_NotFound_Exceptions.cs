using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Filters
{
    internal class Server_NotFound_Exceptions : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is KeyNotFoundException)
            {
                context.Result = new ContentResult { StatusCode = 404, Content = context.Exception.Message };
            }
            else if(context.Exception is Exception)
            {
                if (context.Exception.InnerException is not null)
                {
                    context.Result = new ContentResult { StatusCode = 500, Content = context.Exception.InnerException.Message };
                }
                else
                {
                    context.Result = new ContentResult { StatusCode = 500, Content = context.Exception.Message };
                }
            }
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;

namespace _2.ExpenseManagement.Api.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        private readonly ILogger _logger;

        public MyControllerBase(ILogger logger)
        {
            _logger = logger;
        }
    }
}


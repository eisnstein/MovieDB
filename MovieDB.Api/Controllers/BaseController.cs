using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Controllers
{
    [Controller]
    public class BaseController : ControllerBase
    {
        public Account? Account => HttpContext.Items.ContainsKey("Account") && HttpContext.Items["Account"] is not null
            ? (Account) HttpContext.Items["Account"]!
            : null;
    }
}
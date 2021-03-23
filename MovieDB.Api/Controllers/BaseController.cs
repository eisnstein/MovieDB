using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Controllers
{
    [Controller]
    public class BaseController : ControllerBase
    {
        protected Account? Account => HttpContext.Items.ContainsKey(nameof(Account)) && HttpContext.Items[nameof(Account)] is not null
            ? (Account) HttpContext.Items[nameof(Account)]!
            : null;
    }
}
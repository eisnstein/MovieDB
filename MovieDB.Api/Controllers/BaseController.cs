using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Controllers;

[Controller]
public class BaseController : ControllerBase
{
    public Account? Account => HttpContext.Items[nameof(Account)] as Account;
}

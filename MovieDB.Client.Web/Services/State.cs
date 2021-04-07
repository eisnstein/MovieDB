using MovieDB.Client.Web.Models;

namespace MovieDB.Client.Web.Services
{
    public class State
    {
        public bool IsAuthenticated { get; set; } = true;
        public bool IsAuthenticating { get; set; } = false;
        public TUser User { get; set; }

        public void Toggle()
        {
            IsAuthenticated = !IsAuthenticated;
        }
    }
}

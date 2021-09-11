namespace MovieDB.Client.Web.Models
{
    public enum AlertType
    {
        Success,
        Error,
        Info,
        Warning
    }

    public record Alert
    {
        public string Id { get; set; }
        public AlertType Type { get; init; }
        public string Message { get; init; }
        public bool AutoClose { get; init; }
        public bool KeepAfterRouteChange { get; init; }
        public bool Fade { get; init; }
    }
}

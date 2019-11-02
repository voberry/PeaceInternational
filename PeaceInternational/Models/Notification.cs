namespace PeaceInternational.Web.Models
{
    public class Notification
    {

        public Notification()
        {

        }

        public Notification(string Type, string Message)
        {
            this.Type = Type;
            this.Message = Message;
        }

        public string Type { get; set; }
        public string Message { get; set; }
    }
}

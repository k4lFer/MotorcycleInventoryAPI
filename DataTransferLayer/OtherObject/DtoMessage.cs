namespace DataTransferLayer.OtherObject
{
    public class DtoMessage
    {
        public string type { get; set; }
        public List<string> ListMessage { get; set; }

        public DtoMessage()
        {
            ListMessage = new List<string>();
        }

        public bool ExistsMessage()
        {
            return ListMessage.Count > 0;
        }

        public void AddMessage(string message)
        {
            ListMessage.Add(message);
        }

        public void Success()
        {
            type = "success";
        }

        public void Warning()
        {
            type = "warning";
        }

        public void Error()
        {
            type = "error";
        }
        
        public void Conflict()
        {
            type = "conflict";
        }

        public void Exception()
        {
            type = "exception";
        }
    }
}

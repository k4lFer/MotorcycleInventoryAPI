namespace DataTransferLayer.OtherObject
{
    public class DtoAppSettings
    {
        public static string connetionStringsMySql { get; set; }
        public string originAudience { get; set; }
        public string originIssuer { get; set; }
        public string accessJwtSecret { get; set; }
        public string refreshJwtSecret { get; set; }
        public string originRequest { get; set; }
    }
}

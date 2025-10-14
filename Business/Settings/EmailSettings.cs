namespace Business.Settings
{
    public class EmailSettings
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; } = true;
    }
}
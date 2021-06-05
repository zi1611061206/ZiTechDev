namespace ZiTechDev.Api.Engines.Email
{
    public interface IEmailServerConfiguration
    {
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        string PopServer { get; set; }
        int PopPort { get; set; }
        string PopUsername { get; set; }
        string PopPassword { get; set; }

        string ImapServer { get; set; }
        int ImapPort { get; set; }
        string ImapUsername { get; set; }
        string ImapPassword { get; set; }
    }
}

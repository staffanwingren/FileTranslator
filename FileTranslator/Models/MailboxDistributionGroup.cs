namespace FileTranslator.Models
{
    public class MailboxDistributionGroup
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string PrimarySmtpAddress { get; set; }
        public string WindowsEmailAddress { get; set; }
        public string EmailAddresses { get; set; }
        public string HiddenFromAddressListsEnabled { get; set; }    
    }
}

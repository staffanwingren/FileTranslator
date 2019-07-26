using System.Dynamic;

namespace FileTranslator.Models
{
    public class MailboxUser
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string WindowsEmailAddress { get; set; }
        public string PrimarySmtpAddress { get; set; }
        public string EmailAddresses { get; set; }
        public string GrantSendOnBehalfTo { get; set; }
        public string HiddenFromAddressListsEnabled { get; set; }
        public string MailTip { get; set; }
        public string LitigationHoldEnabled { get; set; }
        public string RetentionHoldEnabled { get; set; }
        public string ArchiveName { get; set; }
        public string RecipientTypeDetails { get; set; }
    }
}
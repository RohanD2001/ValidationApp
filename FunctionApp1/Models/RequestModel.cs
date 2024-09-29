using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFunctionApp
{
    public class Header
    {
        [Required]
        public DateTime Timestamp { get; set; }

        [Required, StringLength(50)]
        public string TransactionType { get; set; }

        [Required, StringLength(100)]
        public string Source { get; set; }
    }

    public class ParentAccount
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class RequestModel
    {
        [Required]
        public Header Header { get; set; }

        [Required]
        public ParentAccount ParentAccount { get; set; }

        [Required]
        public string AccountId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PartyId must be a positive number")]
        public int PartyId { get; set; }

        [Required]
        public string ContactId { get; set; }

        [Required]
        public string ExternalId { get; set; }

        [Required]
        public string RecordTypeId { get; set; }

        [Required]
        public string B2bUserId { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Salutation { get; set; }

        [Range(0, int.MaxValue)]
        public int PreferredLanguage { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string EndMarketISO { get; set; }

        [Required]
        public string MailingState { get; set; }

        [Required]
        public string MailingStreet { get; set; }

        [Required]
        public string MailingCity { get; set; }

        [Required]
        public string MailingCountry { get; set; }

        [Required]
        public string MailingPostalCode { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string PortalUserStatus { get; set; }

        [Required]
        public string B2bPortalUserStatus { get; set; }

        [Required]
        public string Gender { get; set; }

        public DateTime? Birthdate { get; set; }
    }

}

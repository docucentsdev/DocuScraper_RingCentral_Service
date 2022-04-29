using System.ComponentModel.DataAnnotations;

namespace DocuScraper.Services.RingCentral.Models
{
    public class SmsRequestDTO
    {
        [Required(ErrorMessage = "Recipient number is required.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone should be a 10 digit valida phone number.")]
        public string RecipientNumber { get; set; }

        [Required(ErrorMessage = "Text message is required.")]
        public string Message { get; set; }
    }
}

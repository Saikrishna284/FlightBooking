using System.ComponentModel.DataAnnotations;


namespace FlightBooking.Dto
{
    public class PaymentDto
    {
       [Required(ErrorMessage = "Payment Method is required.")]
       [RegularExpression("Net Banking|UPI|Credit Card|Debit Card", ErrorMessage = "Payment method must be either 'UPI', 'Credit Card', 'Debit Card', or 'Net Banking'.")]
        public required string PaymentMethod { get; set; }
         

       [Required(ErrorMessage = "PaymentStatus is required.")]
       [RegularExpression("Paid|Refund|Pending", ErrorMessage = "Payment Status must be either 'Paid', 'Refund', or 'Pending'.")]
        public required string PaymentStatus { get; set; }
    }

    public class GetPaymentDto
    {
         public int PaymentId { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }

       
    }
}
namespace Presentation.Models.ViewModels.Admin
{
    using Domain.Models;
    using System.ComponentModel.DataAnnotations;

    public class AdminListTicketDetailsViewModel
    {
        public Guid Id { get; set; }

        public List<Ticket> ticketList { get; set; }

        public string Owner { get; set; }
        [Display(Name = "Passport image:")]
        public string PassportImage { get; set; }

        [Display(Name = "Paid for ticket:")]
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }
    }
}

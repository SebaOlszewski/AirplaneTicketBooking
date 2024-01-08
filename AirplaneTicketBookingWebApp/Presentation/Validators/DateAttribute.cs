using System.ComponentModel.DataAnnotations;

namespace Presentation.Validators
{
    public class DateAttribute :ValidationAttribute
    {
        //check if the data input is aftet 2020.01.01

        private readonly DateTime _minDate = new DateTime(2020, 1, 1);

        public override bool IsValid(object value)
        {
            if (value is DateTime departureDate)
            {
                return departureDate >= _minDate;
            }

            return false; // Non-DateTime types are considered invalid
        }
    }
}

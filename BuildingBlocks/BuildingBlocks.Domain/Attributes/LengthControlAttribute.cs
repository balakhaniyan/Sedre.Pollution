using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain.Models;

namespace BuildingBlocks.Domain.Attributes
{
    public class LengthControlAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public LengthControlAttribute(int min = int.MinValue, int max = int.MaxValue,
            string errorMessage = Error.LengthControl)
        {
            _min = min;
            _max = max;
            ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            var myString = value.ToString();
            return myString.Length > _min && myString.Length < _max;
        }
    }
}
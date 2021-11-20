using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class No2Dto : BaseIndicatorDto
    {
        [Required] public double No2 { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class O3Dto : BaseIndicatorDto
    {
        [Required] public double O3 { get; set; }
    }
}
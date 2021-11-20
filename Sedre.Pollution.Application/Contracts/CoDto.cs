using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class CoDto : BaseIndicatorDto
    {
        [Required] public double Co { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class So2Dto : BaseIndicatorDto
    {
        [Required] public double So2 { get; set; }
    }
}
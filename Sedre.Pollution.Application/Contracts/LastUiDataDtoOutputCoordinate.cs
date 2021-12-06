using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class LastUiDataDtoOutputCoordinate<T>  where  T : class
    {
        [Required] public int Date { get; set; }
        [Required] public int Time { get; set; }
        [Required] public T Indicators { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class LastUiDataDtoOutput<T>  where  T : class
    {
        [Required] public int Date { get; set; }
        [Required] public int Time { get; set; }
        [Required] public IList<T> Indicators { get; set; }
    }
}
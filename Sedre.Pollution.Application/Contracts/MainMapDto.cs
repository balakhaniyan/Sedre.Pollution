using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sedre.Pollution.Application.Contracts
{
    public class MainMapDto
    {
        [Required] public List<List<double>> Coordinates { get; set; }
        [Required] public string Color { get; set; }
    }

}
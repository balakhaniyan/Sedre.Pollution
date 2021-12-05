using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain.Implementations;

namespace BuildingBlocks.Domain.Models
{
    public class Document : TrackableEntity
    {
        [Required] public string FileName { get; set; }
        [Required] public string ContentType { get; set; }
        [Required] public string Extension { get; set; }
        [Required] public long Length { get; set; }
        [Required] public byte[] Data { get; set; }
    }
}
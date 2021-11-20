using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Application.Contracts
{
    public class DocumentDto :EntityDto<Guid>
    {
        [Required] public string FileName { get; set; }
        [Required] public string ContentType { get; set; }
        [Required] public string Extension { get; set; }
        [Required] public long Length { get; set; }
        [Required] public byte[] Data { get; set; }
    }
}
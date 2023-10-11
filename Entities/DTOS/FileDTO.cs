using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyAzureAppService.Entities.DTOS
{
    [DataContract]
    public class FileDTO
    {
        [Required]
        /// <summary>
        /// <function>Name of file without extension</function>
        /// <example>MyFile</example>
        /// </summary>
        public string FileName { get; set; }

        [Required]
        /// <summary>
        /// <function>Data of file</function>
        /// <example></example>
        /// </summary>
        public IFormFile FormFile { get; set; }
    }

    [DataContract]
    public class FileTestDTO
    {
        [Required]
        /// <summary>
        /// <function>Name of file without extension</function>
        /// <example>MyFile</example>
        /// </summary>
        public string FileName { get; set; }
    }
}


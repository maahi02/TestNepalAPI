using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestNepal.Entities
{
    public class FileAttachment
    {
        /// <summary>
        /// For multiple images access
        /// </summary>
        [Key]
        public int Id { get; set; }
        public int ArticleId { get; set;}
        public string AttachmentName { get; set; }
        /// <summary>
        /// filepath is for both filePath in Disk and the URL
        /// </summary>
        public string FilePath { get; set; }
        public string ThumbnailFilePath { get; set; }
    }
}

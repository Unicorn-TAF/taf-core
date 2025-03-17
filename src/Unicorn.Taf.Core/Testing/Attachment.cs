using System;
using System.IO;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents test attachment based on some file in file system.
    /// </summary>
    [Serializable]
    public class Attachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment"/> class.
        /// </summary>
        public Attachment() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment"/> class.
        /// </summary>
        /// <param name="name">attachment name</param>
        /// <param name="mimeType">file mime type</param>
        /// <param name="filePath">path to attachment file</param>
        public Attachment(string name, string mimeType, string filePath)
        {
            Name = name;
            MimeType = mimeType;
            FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets attachment name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets attachment mime type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets attachment file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Reads and gets bytes from attachment file.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes() =>
            File.ReadAllBytes(FilePath);
    }
}

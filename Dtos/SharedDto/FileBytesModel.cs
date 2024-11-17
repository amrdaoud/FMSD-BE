namespace FMSD_BE.Dtos.SharedDto
{
    public class FileBytesModel
    {
        public byte[]? Bytes { get; set; } = null;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}

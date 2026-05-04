namespace TestBrixCMS.DTOs
{
    public class PublishPageDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string JsonData { get; set; }
        public List<BlockDto> Blocks { get; set; } = new();
    }
    public class BlockDto
    {
        public string Type { get; set; }
        public string JsonData { get; set; }
        public int SortOrder { get; set; }
        public Guid? ParentId { get; set; }
        public string OriginalId { get; set; } // ? NUEVO
    }
   
}

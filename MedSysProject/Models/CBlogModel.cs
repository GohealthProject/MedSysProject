namespace MedSysProject.Models
{
    public class CBlogModel
    {
        public int BlogID { get; set; }
        public string Title { get; set; }

        public int ArticleClassID { get; set; }
        public string Category { get; set; }
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public byte[]? BlogImage { get; set; }
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
    }
}

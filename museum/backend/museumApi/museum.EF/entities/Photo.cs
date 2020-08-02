namespace museumApi.EF.entities
{
    public partial class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int FkChronicle { get; set; }

        public Chronicle FkChronicleNavigation { get; set; }
    }
}

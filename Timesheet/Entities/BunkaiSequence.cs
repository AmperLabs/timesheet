namespace Timesheet.Entities
{
    public class BunkaiSequence
    {
        public string Id { get; set; }
        public string KataId { get; set; }

        public List<BunkaiAction> Actions { get; set; } = new List<BunkaiAction>();

        public string? Description { get; set; }
        public List<string> VideoUrls { get; set; } = new List<string>();
    }

    public class BunkaiAction
    {
        public string Tori { get; set; }
        public string Uke { get; set; }
    }
}

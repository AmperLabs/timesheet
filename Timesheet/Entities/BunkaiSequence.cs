namespace Timesheet.Entities
{
    public class BunkaiSequence
    {
        public string Id { get; set; }
        public string KataId { get; set; }

        public string Title { get; set; }

        public List<BunkaiAction> Actions { get; set; } = new List<BunkaiAction>();

        public string? Description { get; set; }
        public List<string> VideoUrls { get; set; } = new List<string>();
    }

    public class BunkaiAction
    {
        public BunkaiActor Actor { get; set; }
        public string Action { get; set; }
    }

    public enum BunkaiActor
    {
        Tori,
        Uke
    }
}

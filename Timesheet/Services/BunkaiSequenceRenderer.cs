using Timesheet.Entities;

namespace Timesheet.Services
{
    public class BunkaiSequenceRenderer
    {
        public string RenderAsMarkdown(BunkaiSequence sequence)
        {
            if (sequence is null)
                return string.Empty;

            var md = $"# Bunkai für {sequence.KataId}\n\n";

            md += $"## Variante {sequence.Title}\n\n";

            md += "### Beschreibung\n\n";

            md += $"{sequence.Description}\n\n";

            md += "### Techniken\n\n";

            foreach (var action in sequence.Actions)
            {
                md += $"- {action.Actor.ToString()}:\n";
                md += $"  - {action.Action}\n";
            }

            md += "### Links\n\n";

            return md;
        }
    }
}

namespace Application.Groups.Requests
{
    public class EditGroupRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Visibility { get; set; }
        public required string SizePreset { get; set; } // csak upgrade engedélyezett!
    }
}

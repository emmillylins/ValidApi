namespace ValidApi.Models
{
    public class ProfileParameter
    {
        public string ProfileName { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }
}

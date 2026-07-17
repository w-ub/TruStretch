namespace TruStretch.Models
{
    public class ResolutionProfile
    {
        public string Name { get; set; } = "";
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString() => Name;
    }

    public class AppConfig
    {
        public int DefaultWidth { get; set; } = 1920;
        public int DefaultHeight { get; set; } = 1080;

        public string LastProfile { get; set; } = "";

        public System.Collections.Generic.List<ResolutionProfile> Profiles { get; set; } = new();
    }
}
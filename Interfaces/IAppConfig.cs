namespace AdobeBlockListConverter.Interfaces
{
    public interface IAppConfig
    {
        string GetBlockListUrl { get; }
        string? GetMode(string[] args);
        ConfigTemplate CurrentTemplate { get; }
        Dictionary<string, ConfigTemplate> AvailableTemplates { get; }
        void SetTemplate(string templateType);
    }

    public class ConfigTemplate
    {
        public string OutputFileHeader { get; set; }
        public string OutputLineTemplate { get; set; }
        public string OutputFileCommand { get; set; }
        public string OutputFileNameTemplate { get; set; }
    }
}
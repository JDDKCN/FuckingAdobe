namespace AdobeBlockListConverter.Interfaces
{
    public interface IApplication
    {
        Task RunAsync(string[] args);
    }

    public interface IFileService
    {
        bool FileExists(string path);
        Task<string> ReadFileAsync(string path);
        Task WriteToFileAsync(string path, string content);
        Task ProcessInputFileAsync(string inputPath, string outputPath, bool isWebSource, string webContent = null);
    }

    public interface INetworkService
    {
        Task<string> DownloadFileContentAsync(string url);
    }

    public interface IDataProcessor
    {
        string ProcessLine(string line);
        Task<string> ProcessDataAsync(string inputData);
    }

    public interface IUserInterface
    {
        string GetUserInput(string prompt, string defaultValue = null);
        void WelcomeMessage();
        void DisplayMessage(string message);
        void DisplayError(string message);
        void DisplaySuccess(string message);
        string GetInputFilePath(string[] args);
        string GetOutputFilePath(string[] args, string defaultPath);
    }

    public interface IAppConfig
    {
        string GetBlockListUrl { get; }
        string OutputFileTemplate { get; }
        string OutputLineTemplate { get; }
        string OutputFileHeader { get; }
        string OutputFileCommand { get; }
    }

}

using AdobeBlockListConverter.Interfaces;

namespace AdobeBlockListConverter.Services
{
    public class FileService(IDataProcessor dataProcessor, IAppConfig config) : IFileService
    {
        private readonly IDataProcessor _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
        private readonly IAppConfig _config = config ?? throw new ArgumentNullException(nameof(config));

        public bool FileExists(string path)
        {
            return !string.IsNullOrEmpty(path) && File.Exists(path);
        }

        public async Task<string> ReadFileAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("文件路径不能为空", nameof(path));

            if (!FileExists(path))
                throw new FileNotFoundException("找不到指定的文件", path);

            return await File.ReadAllTextAsync(path);
        }

        public async Task WriteToFileAsync(string path, string content)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("输出文件路径不能为空", nameof(path));

            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);

            await File.WriteAllTextAsync(path, content);
        }

        public async Task ProcessInputFileAsync(string inputPath, string outputPath, bool isWebSource, string webContent = null)
        {
            string processedContent = _config.OutputFileHeader;

            if (isWebSource && !string.IsNullOrEmpty(webContent))
            {
                processedContent += await _dataProcessor.ProcessDataAsync(webContent);
            }
            else
            {
                string fileContent = await ReadFileAsync(inputPath);
                processedContent += await _dataProcessor.ProcessDataAsync(fileContent);
            }

            processedContent += _config.OutputFileCommand;

            await WriteToFileAsync(outputPath, processedContent);
        }
    }
}

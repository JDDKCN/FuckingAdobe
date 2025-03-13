using AdobeBlockListConverter.Interfaces;

namespace AdobeBlockListConverter.Applications
{
    public class Application(
        IUserInterface ui,
        IFileService fileService,
        INetworkService networkService,
        IAppConfig config) : IApplication
    {
        private readonly IUserInterface _ui = ui ?? throw new ArgumentNullException(nameof(ui));
        private readonly IFileService _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        private readonly INetworkService _networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
        private readonly IAppConfig _config = config ?? throw new ArgumentNullException(nameof(config));

        public async Task RunAsync(string[] args)
        {
            try
            {
                _ui.WelcomeMessage();

                var templates = config.AvailableTemplates;

                Console.WriteLine("可用的模板类型:");
                foreach (var template in templates.Keys)
                {
                    Console.WriteLine($" - {template}");
                }

                Console.Write("请选择要使用的模板类型: ");
                string? templateType = config.GetMode(args) ?? Console.ReadLine();

                while (true)
                {
                    if (!string.IsNullOrWhiteSpace(templateType) && templates.ContainsKey(templateType.ToLower()))
                    {
                        config.SetTemplate(templateType);
                        break;
                    }

                    Console.WriteLine($"无效的模板类型，请重新输入：");
                    templateType = Console.ReadLine();
                }

                string inputFilePath = _ui.GetInputFilePath(args);
                string inputContent = null;
                bool isWebSource = false;

                while (true)
                {
                    if (inputFilePath == "web")
                    {
                        _ui.DisplayMessage("正在从网络获取数据...");
                        inputContent = await _networkService.DownloadFileContentAsync(_config.GetBlockListUrl);

                        if (string.IsNullOrEmpty(inputContent))
                        {
                            _ui.DisplayError("无法从网络获取数据，请检查网络连接或尝试手动下载文件。");
                            inputFilePath = _ui.GetUserInput("请输入本地源文件路径（或输入web重试）：");
                            continue;
                        }

                        _ui.DisplaySuccess("网络数据获取成功！");
                        isWebSource = true;
                        break;
                    }
                    else if (_fileService.FileExists(inputFilePath))
                    {
                        break;
                    }
                    else
                    {
                        _ui.DisplayError("指定文件不存在！");
                        inputFilePath = _ui.GetUserInput("请输入本地源文件路径（或输入web自动从网络获取）：");
                    }
                }

                string outputFilePath = _ui.GetOutputFilePath(args, _config.CurrentTemplate.OutputFileNameTemplate);
                await _fileService.ProcessInputFileAsync(inputFilePath, outputFilePath, isWebSource, inputContent);

                _ui.DisplaySuccess("\r\n处理完成！");
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"\r\n处理过程中发生错误：{ex.Message}");
                throw;
            }
        }
    }
}

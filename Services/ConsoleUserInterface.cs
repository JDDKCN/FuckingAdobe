using AdobeBlockListConverter.Interfaces;

namespace AdobeBlockListConverter.Services
{
    public class ConsoleUserInterface(IAppConfig config) : IUserInterface
    {
        private readonly IAppConfig _config = config ?? throw new ArgumentNullException(nameof(config));

        public string GetUserInput(string prompt, string defaultValue = null)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }

        public void WelcomeMessage()
        {
            Console.Title = "KCN-Server Adobe全家桶屏蔽域名表转换程序 -V1.1.0";
            Console.WriteLine("\r\nKCN-Server Adobe全家桶屏蔽域名表转换程序 -V1.1.0\r\nFucking Adobe!通过配置Clash使你的Adobe全家桶不再弹窗。\r\n");
            Console.WriteLine("Usage: AdobeBlockListConverter mode<转换模式，根据json配置自定义> input<本地源文件路径，web为联网自动获取> output<输出文件路径，auto为创建在程序运行根目录下> [-q]<静默结束>\r\n");
            Console.WriteLine($"请前往 {_config.GetBlockListUrl} 查看屏蔽域名列表。");
            Console.WriteLine("配置步骤[仅适用于Clash For Windows]: \r\n1. 打开导出的预处理配置文本，复制文本。\r\n2. 打开Clash程序，点击左边栏配置项。\r\n3. 右键你正在使用的订阅(绿色)，唤出二级菜单，点击配置文件预处理。\r\n5. 在编辑器里粘贴，把导出配置的url处改成你的订阅地址，然后保存。\r\n6. 回到主页，打开TUN模式，配置完成。");
            Console.WriteLine("记得把导出配置的url处改成你的订阅地址再保存！要否则无法使用！\r\n");
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public string GetInputFilePath(string[] args)
        {
            string inputFilePath;

            if (args.Length > 1 && args[1] != null)
            {
                inputFilePath = args[1];
                DisplayMessage($"输入文件路径: {inputFilePath}");
            }
            else
            {
                inputFilePath = GetUserInput("请输入源文件路径（输入web自动从网络获取）：");
            }

            return inputFilePath;
        }

        public string GetOutputFilePath(string[] args, string defaultPath)
        {
            string outputFilePath;

            if (args.Length > 2 && args[2] != null)
            {
                outputFilePath = args[2];
                DisplayMessage($"输出文件路径: {outputFilePath}");
            }
            else
            {
                outputFilePath = GetUserInput("请输入输出文件路径(输入auto自动创建在程序根目录)：");
            }

            if (string.IsNullOrEmpty(outputFilePath) || outputFilePath.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                outputFilePath = defaultPath;
                DisplayMessage($"文件自动创建于 {outputFilePath}");
            }

            return outputFilePath;
        }
    }
}

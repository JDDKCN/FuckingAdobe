using AdobeBlockListConverter.Interfaces;
using System.Text.Json;

namespace AdobeBlockListConverter.Configs
{
    public class AppConfig : IAppConfig
    {
        private readonly Dictionary<string, ConfigTemplate> _templates;
        private readonly string _configFilePath;
        private ConfigTemplate _currentTemplate;
        public string GetBlockListUrl => "https://a.dove.isdumb.one/list.txt";
        public Dictionary<string, ConfigTemplate> AvailableTemplates => _templates;
        public ConfigTemplate CurrentTemplate => _currentTemplate;

        public AppConfig()
        {
            _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            if (!File.Exists(_configFilePath))
            {
                Console.WriteLine("配置文件不存在: {0}，创建默认配置。", _configFilePath);
                CreateDefaultTemplates();
            }

            _templates = LoadConfigTemplates();

            if (_templates?.Count > 0)
                _currentTemplate = _templates.Values.First();
        }

        private void CreateDefaultTemplates()
        {
            string templateJson = @"
{
  ""cfw"": {
    ""outputFileHeader"": ""# By KCN-Server.AdobeBlockListConverter \nparsers:\n  - url: 你的订阅地址Url\n    yaml:\n      prepend-proxy-groups:\n        - name: Fucking-Adobe\n          type: select\n          proxies:\n            - REJECT\n      \n      prepend-rules:\n"",
    ""outputLineTemplate"": ""        - DOMAIN-SUFFIX,{0},Fucking-Adobe"",
    ""outputFileCommand"": ""\n      commands:\n        - proxy-groups.Fucking-Adobe.proxies.0=REJECT\n"",
    ""outputFileNameTemplate"": ""KCNServer-AdobeBlockList-CFW-{0}.txt""
  },
  ""verge"": {
    ""outputFileHeader"": ""# By KCN-Server.AdobeBlockListConverter \n# Clash Verge Merge 格式\n\nprepend-proxy-groups:\n  - name: Fucking-Adobe\n    type: select\n    proxies:\n      - REJECT\n\nprepend-rules:\n"",
    ""outputLineTemplate"": ""  - DOMAIN-SUFFIX,{0},Fucking-Adobe"",
    ""outputFileCommand"": """",
    ""outputFileNameTemplate"": ""KCNServer-AdobeBlockList-Verge-{0}.txt""
  }
}
";
            File.WriteAllText(_configFilePath, templateJson);
        }

        private Dictionary<string, ConfigTemplate> LoadConfigTemplates()
        {
            try
            {
                string json = File.ReadAllText(_configFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var templates = JsonSerializer.Deserialize<Dictionary<string, ConfigTemplate>>(json, options);

                Console.WriteLine("已成功加载 {0} 个配置模板", templates.Count);
                return templates;
            }
            catch (Exception ex)
            {
                Console.WriteLine("加载配置模板时出错: {0}", ex.Message);
                return new Dictionary<string, ConfigTemplate>();
            }
        }

        public void SetTemplate(string templateType)
        {
            if (string.IsNullOrWhiteSpace(templateType))
                throw new ArgumentNullException(nameof(templateType));

            if (!_templates.TryGetValue(templateType.ToLower(), out var template))
                throw new ArgumentException($"未找到模板类型: {templateType}");

            template.OutputFileNameTemplate =
                Path.Combine(Environment.CurrentDirectory,
                string.Format(_currentTemplate.OutputFileNameTemplate, Guid.NewGuid()));
            _currentTemplate = template;

            Console.WriteLine("已切换到模板: {0}", templateType);
        }

        public string? GetMode(string[] args)
        {
            try
            {
                if(args.Length > 0)
                    return args[0];
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("出错了: {0}", ex.Message);
                return null;
            }
        }
    }
}
using AdobeBlockListConverter.Interfaces;

namespace AdobeBlockListConverter.Configs
{
    public class AppConfig : IAppConfig
    {
        public string GetBlockListUrl => "https://a.dove.isdumb.one/list.txt";

        public string OutputFileTemplate => Path.Combine(Environment.CurrentDirectory, $"KCNServer-AdobeBlockList-{Guid.NewGuid()}.txt");

        public string OutputLineTemplate => "        - DOMAIN-SUFFIX,{0},Fucking-Adobe";

        public string OutputFileHeader => @"# By KCN-Server.AdobeBlockListConverter 
parsers:
  - url: 你的订阅地址Url
    yaml:
      prepend-proxy-groups:
        - name: Fucking-Adobe
          type: select
          proxies:
            - REJECT
      
      prepend-rules:
";

        public string OutputFileCommand => @"
      commands:
        - proxy-groups.Fucking-Adobe.proxies.0=REJECT
";

    }
}

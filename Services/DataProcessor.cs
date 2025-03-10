using AdobeBlockListConverter.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AdobeBlockListConverter.Services
{
    public class DataProcessor(IAppConfig config) : IDataProcessor
    {
        private readonly IAppConfig _config = config ?? throw new ArgumentNullException(nameof(config));
        private readonly Regex _ipPattern = new Regex(@"^0\.0\.0\.0\s+(.+)$", RegexOptions.Compiled);

        public string ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                return null;

            string domain = line.Trim();

            Match match = _ipPattern.Match(domain);
            if (match.Success)
                domain = match.Groups[1].Value.Trim();

            return string.Format(_config.OutputLineTemplate, domain);
        }

        public async Task<string> ProcessDataAsync(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return string.Empty;

            StringBuilder result = new StringBuilder();

            using (StringReader reader = new StringReader(inputData))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    string processedLine = ProcessLine(line);
                    if (!string.IsNullOrEmpty(processedLine))
                    {
                        result.AppendLine(processedLine);
                    }
                }
            }

            return result.ToString();
        }
    }
}

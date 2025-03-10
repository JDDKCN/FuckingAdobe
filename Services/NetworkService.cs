using AdobeBlockListConverter.Interfaces;

namespace AdobeBlockListConverter.Services
{
    public class NetworkService(IUserInterface ui) : INetworkService
    {
        private readonly IUserInterface _ui = ui ?? throw new ArgumentNullException(nameof(ui));

        public async Task<string> DownloadFileContentAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL不能为空", nameof(url));

            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);

            try
            {
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException ex)
            {
                _ui.DisplayError($"下载失败 (HTTP错误): {ex.Message}");
                return string.Empty;
            }
            catch (TaskCanceledException)
            {
                _ui.DisplayError("下载超时，请检查网络连接");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _ui.DisplayError($"下载失败: {ex.Message}");
                return string.Empty;
            }
        }
    }
}

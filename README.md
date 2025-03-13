
<div align="center"><strong>

# FuckingAdobe - KCNServer.AdobeBlockListConverter

</strong></div>
 > 杜绝Adobe非正版弹窗 - 自动拉取Adobe反盗版弹窗域名列表并生成适用于Clash的预处理屏蔽配置文件。
>
> 今天下午我用着用着Ps突然给我弹了一个反盗版弹窗，然后软件就用不了了。频繁重装很讨厌，于是花了一会时间摸了这个小项目。
>
> 使用本软件生成的配置文件可以有效消灭Adobe的非正版弹窗。具体教程请看程序内说明。**记得配合 [AdobeGenp破解补丁](https://github.com/wangzhenjjcn/AdobeGenp) 使用喵！**

## 配置模板说明

从`v1.1`版本起，本程序使用 `config.json` 文件存储不同客户端的配置模板。您可以根据需要修改此文件以自定义配置模板。

### 配置文件结构

```json
{
  "模板名称": {
    "outputFileHeader": "配置文件头部内容",
    "outputLineTemplate": "每个域名的行模板，使用 {0} 作为域名占位符",
    "outputFileCommand": "配置文件尾部内容",
    "outputFileNameTemplate": "文件名模板，使用 {0} 作为唯一ID占位符"
  }
}
```

### 预设模板

1. `cfw` - 适用于 Clash For Windows 的配置模板
2. `verge` - 适用于 Clash Verge 的配置模板

### 如何添加新模板

1. 打开 `config.json` 文件
2. 按照上述结构添加新的模板
3. 保存文件
4. 重新启动程序

## ⚠️ 免责声明
本项目仅供研究交流用，禁止用于商业及非法用途。使用本项目造成的事故与损失，与作者无关。本项目完全免费，如果您是花钱买的，说明您被骗了。请尽快退款，以减少您的损失。
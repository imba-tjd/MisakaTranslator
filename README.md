# MisakaTranslator 御坂翻译器

本项目是 https://github.com/hanmin0822/MisakaTranslator 的fork。

功能增加：

* .NET6
* x64
* 异步Hook翻译
* HandyControl 3.x，Microsoft.Data.Sqlite
* 原版TextractorCLI
* 运行不要求管理员权限（但可能导致Hook失败或者无法截图）

功能缺失：

* TTS
* Mecab
* TesseractOCR
* J北京等32位库
* 默认关闭人工翻译且未测试是否可用
* 全屏时原来能显示的现在也许不能显示
* Python脚本去重
* 不显示托盘

没有解决：

* 反复打开设置界面会导致内存不断增长
* 最小翻译间隔（防抖）
* Process.Start时Core的UseShellExecute默认为False，导致运行URL失败。点击申请API按钮会用到，懒得改
* OCR和DPI有关的问题。我不用OCR

构建（需要装SDK）：

```cmd
dotnet publish -c Release -r win10-x64 --self-contained=false -p:PublishReadyToRun=true -p:DebugType=none -p:WarningLevel=0
```

结果在`MisakaTranslator\MisakaTranslator-WPF\bin\Release\net6.0-windows\win10-x64\publish`中。如果运行一开始就报错，异常为XAMLParseException，需要删除obj和bin重新构建。

支持用VS Code调试，launch的cwd改为WPF.dll所在目录，program改为那个dll，即可。但不能和VS混用。

你也可以下载我release的包，但不保证为最新的；运行必须装好.NET Desktop Runtime（SDK中已包含）。

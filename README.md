
# 智能体系统

* **插件未开源**

## 作者信息

Copyright FirePlume, All Rights Reserved.

Email: fireplume@126.com<br>
GitHub: [FirePlume126](https://www.github.com/FirePlume126)<br>
Bilibili: [火羽FP](https://space.bilibili.com/395084718)<br>
YouTube: [FirePlume126](https://www.youtube.com/@FirePlume126)

**[返回目录](https://www.github.com/FirePlume126/FP_Readme#Directory)**

<a name="fpagentsystem"></a>
## FPAgentSystem

智能体系统框架，实现游戏逻辑与大模型的无缝交互。

* **此模块的主要类**

|类名|描述|
|:-:|:-:|
|FPAgentCore|智能体核心，负责`HTTP`通信、消息解析、状态管理、调用工具注册表|
|FPAgentConfigBase|智能体配置基类，负责配置管理智能体的模型、工具等参数|
|FPAgentContext|智能体上下文，负责管理智能体的对话历史|
|FPAgentToolBase|智能体工具基类，负责定义工具，变量如果开启"生成时公开"，AI会读取这些变量作为工具的映射参数|
|FPAgentToolManager|智能体工具管理器，负责注册和管理工具|
|FPAgentGuardrail|智能体安全栅栏(暂时不考虑)，负责定义和执行安全规则|
|FPAgentTelemetry|智能体遥测系统(暂时不考虑)，负责收集和分析智能体的性能数据|
|FPAgentStreamParser|智能体流解析器(暂时不考虑)，负责解析流式响应|

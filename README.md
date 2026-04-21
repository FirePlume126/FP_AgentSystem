
# 智能体系统

* **插件未开源**
* **Demo 中包含的插件没有开源，但支持添加到其他项目中使用，并支持打包**
* **插件还在开发中...，文档不完整**

## 作者信息

Copyright FirePlume, All Rights Reserved.

Email: fireplume@126.com<br>
GitHub: [FirePlume126](https://www.github.com/FirePlume126)<br>
Bilibili: [火羽FP](https://space.bilibili.com/395084718)<br>
YouTube: [FirePlume126](https://www.youtube.com/@FirePlume126)

**[返回目录](https://www.github.com/FirePlume126/FP_Readme#Directory)**

<a name="fpagentsystem"></a>
## FPAgentSystem

智能体系统框架

---

**智能体目录**

- [使用指南](#fpagentsystem_quickstart)：快速使用本插件
- [FPAgentSystem](#fpagentsystem_fpagentsystem)：此插件的运行时模块
	- [智能体核心](#fpagentsystem_core)：智能体的生命周期管理与请求调度中心
	- [智能体配置](#fpagentsystem_config)：配置智能体的模型、API、超时等参数，支持派生类扩展
		- [自定义配置](#fpagentsystem_config_custom)：手动填写请求地址、API密钥与模型
		- [平台配置](#fpagentsystem_config_platform)：使用选平台模型，项目设置中配置平台与模型
	- [智能体上下文](#fpagentsystem_context)：管理对话历史与上下文裁剪
	- [智能体工具](#fpagentsystem_tool)：管理工具的定义、注册与执行
		- [智能体工具基类](#fpagentsystem_toolbase)：本地工具基类，支持蓝图派生与C++派生
		- [智能体工具表](#fpagentsystem_tooltable)：通过数据表批量配置工具
		- [智能体工具管理器](#fpagentsystem_toolmanager)：统一管理所有工具来源
	- [智能体任务](#fpagentsystem_task)：蓝图中使用的异步任务节点
	- [可选扩展能力](#fpagentsystem_extensions)
		- [智能体技能](#fpagentsystem_skill)：技能流程管理系统
		- [智能体MCP](#fpagentsystem_mcp)：MCP远程工具集成
- [项目设置](#fpagentsystem_projectsettings)：此插件的项目设置
- [函数库](#fpagentsystem_functionlibrary)：此插件的函数库
- [FPAgentSystemEditor](#fpagentsystem_fpagentsystemeditor)：此插件的编辑器模块
---

<a name="fpagentsystem_quickstart"></a>
### 使用指南

**等开发完成，再完成使用指南**

1、蓝图中搜索`Agent`或`Wait Agent Response`，即可创建[智能体任务](#fpagentsystem_task)的异步节点，等待智能体的响应。<br>

2、在`ConfigClass`添加智能体配置类，选择`Custom`使用[自定义配置](#fpagentsystem_config_custom)；<br>
选择`Platform`使用[平台配置](#fpagentsystem_config_platform)，[平台配置](#fpagentsystem_config_platform)需要在项目设置中配置平台、模型和API密钥。

3、在添加`ConfigClass`后可以在节点上对其的参数进行设置，在节点后面除了可以获取[智能体任务](#fpagentsystem_task)的指针`Async Task`，还可以获取[智能体配置](#fpagentsystem_config)的指针`ConfigObject`。<br>
`ConfigObject`的类型和`ConfigClass`相同，在智能体运行时，可以通过`ConfigObject`动态设置[智能体配置](#fpagentsystem_config)的参数。

4、在内容浏览器右键菜单的`Agent System`分类下可以创建`AgentTool`和`AgentToolTable`，分别是[智能体工具](#fpagentsystem_tool)和[智能体工具表](#fpagentsystem_tooltable)。<br>
通过[智能体工具表](#fpagentsystem_tooltable)批量配置工具，把[智能体工具表](#fpagentsystem_tooltable)添加给[智能体任务](#fpagentsystem_task)异步节点的`ToolTable`引脚，智能体就可以使用这些工具了。


等待添加所有Utils中的工具函数的使用示例

---

<a name="fpagentsystem_fpagentsystem"></a>
### FPAgentSystem

智能体系统，管理智能体的配置、上下文、工具与生命周期。支持多模型、多工具来源，并通过扩展模块支持技能流程与MCP远程工具。

* **此模块的主要类**

智能体系统的核心类

|类名|描述|
|:-:|:-:|
|FPAgentCore|[智能体核心](#fpagentsystem_core)，智能体的生命周期管理与请求调度中心|
|FPAgentConfigBase|智能体配置基类|
|FPAgentContext|智能体上下文|
|FPAgentStreamParser|智能体流式解析器|
|FPAgentToolManager|智能体工具管理器|
|FPAgentToolBase|智能体工具基类|

智能体系统的扩展类

|类名|描述|
|:-:|:-:|
|FPAgentWaitResponseAsyncAction|智能体任务|
|FPAgentConfig_Custom|自定义智能体配置，仅支持 OpenAI 兼容格式的平台配置|
|FPAgentConfig_Platform|平台智能体配置，仅支持 OpenAI 兼容格式的平台配置|
|FPAgentSkillManager|智能体技能管理器，管理技能流程|
|FPAgentTool_Skill|智能体技能管理工具，用于切换智能体的技能与参考文档|
|FPAgentMCPClient|智能体模型上下文协议客户端，管理与MCP服务器的连接|
|FPAgentTool_MCP|智能体模型上下文协议管理工具，用于调用 MCP 服务器上的远程工具|

<a name="fpagentsystem_task"></a>
#### 智能体任务

蓝图中使用的异步任务节点，以下未此节点提供的函数

```c++
// 销毁智能体
UFUNCTION(BlueprintCallable, Category = "FPAgent")
void DestroyAgent();

// 发送用户消息
// @param InMessage 用户输入的消息内容
UFUNCTION(BlueprintCallable, Category = "FPAgent")
void SendUserMessage(const FString& InMessage);

// 获取智能体配置
UFUNCTION(BlueprintPure, Category = "FPAgent")
UFPAgentConfigBase* GetAgentConfig() const;

// 获取智能体状态
// @return 智能体当前的状态
UFUNCTION(BlueprintPure, Category = "FPAgent")
EFPAgentState GetAgentState() const;

// 添加上下文消息，仅添加上下文，不会执行任何额外逻辑
// @param NewMessages 上下文消息列表
UFUNCTION(BlueprintCallable, Category = "FPAgent|Context")
void AddContextMessages(const TArray<FFPAgentChatMessage>& NewMessages);

// 根据角色移除上下文
// @param InRole 要移除的聊天角色
UFUNCTION(BlueprintCallable, Category = "FPAgent|Context")
void RemoveContextByRole(const EFPAgentChatRole InRole);

// 清除上下文
// @param bKeepSystem 是否保留系统角色的上下文
UFUNCTION(BlueprintCallable, Category = "FPAgent|Context")
void ClearContext(bool bKeepSystem = true);

// 获取上下文消息列表
// @return 当前智能体的上下文消息列表
UFUNCTION(BlueprintPure, Category = "FPAgent|Context")
TArray<FFPAgentChatMessage> GetContextMessages() const;

// 强制终止正在执行的工具
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void ForceTerminateTool();

// 添加工具，同名工具会覆盖已有工具
// @param InToolName 工具名称，作为唯一标识符传递给LLM
// @param InToolClass 工具类，运行时系统会通过此类实例化并执行工具逻辑
// @param InDescription 工具的功能描述，将作为工具说明提供给大语言模型
// @param bProtected 是否受保护，受保护的工具无法通过ClearTools(false)移除，例如系统内部的FPAgentTool_Skill工具
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void AddTool(const FString& InToolName, TSoftClassPtr<UFPAgentToolBase> InToolClass, const FString& InDescription, bool bProtected = false);

// 移除工具
// @param InToolName 工具名称
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void RemoveTool(const FString& InToolName);

// 清除所有工具
// @param bIncludeProtected 是否同时清除受保护的工具，为false时保留受保护的工具
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void ClearTools(bool bIncludeProtected = false);

// 添加工具表
// @param InToolTable 工具数据表
// @param bProtected 是否受保护，受保护的工具表无法通过ClearToolTables(false)移除
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void AddToolTable(UPARAM(meta = (AgentToolTable = "true")) TSoftObjectPtr<UDataTable> InToolTable, bool bProtected = false);

// 移除工具表
// @param InToolTable 工具数据表
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void RemoveToolTable(UPARAM(meta = (AgentToolTable = "true")) TSoftObjectPtr<UDataTable> InToolTable);

// 清除所有工具表
// @param bIncludeProtected 是否同时清除受保护的工具表，为false时保留受保护的工具表
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool")
void ClearToolTables(bool bIncludeProtected = false);

// 获取所有工具描述映射，此函数返回的是初始化后，给LLM使用的实际工具，包含单独工具、工具表和MCP工具
UFUNCTION(BlueprintPure, Category = "FPAgent|Tool")
TMap<FString, FString> GetToolDescriptionMap() const;

// 创建技能管理器，技能管理器使智能体能够加载、激活和管理技能及其参考文档
// 创建时会自动将FPAgentTool_Skill注册到工具管理器中
// @param InRootPath 包含技能子目录的根目录路径，每个子目录必须包含带有SKILL.md文件
UFUNCTION(BlueprintCallable, Category = "FPAgent|Skill")
UFPAgentSkillManager* CreateSkillManager(const FString& InRootPath);

// 销毁技能管理器
UFUNCTION(BlueprintCallable, Category = "FPAgent|Skill")
void DestroySkillManager();

// 获取智能体技能管理器
UFUNCTION(BlueprintPure, Category = "FPAgent|Skill")
UFPAgentSkillManager* GetSkillManager() const;

// 添加MCP客户端，客户端名称重复时会覆盖原有客户端
// @param InMCPConfig MCP客户端配置
UFUNCTION(BlueprintCallable, Category = "FPAgent|MCP")
UFPAgentMCPClient* AddMCPClient(const FFPAgentMCPConfig& InMCPConfig);

// 移除MCP客户端
// @param InClientName MCP客户端名称
UFUNCTION(BlueprintCallable, Category = "FPAgent|MCP")
void RemoveMCPClient(const FName& InClientName);

// 清除所有MCP客户端
UFUNCTION(BlueprintCallable, Category = "FPAgent|MCP")
void ClearMCPClients();
```

<a name="fpagentsystem_projectsettings"></a>
### 项目设置

![FPAgentSystem_Settings](https://github.com/FirePlume126/FP_FPAgentSystem/blob/5.8/Images/FPAgentSystem_Settings.png)

```c++
// 平台配置映射，仅用于 OpenAI 兼容格式的平台
UPROPERTY(config, EditAnywhere, Category = "Config")
TMap<FName, FFPAgentPlatformConfig> PlatformConfigs;
```
以下为相关枚举结构体定义：
```c++
// 智能体平台模型
USTRUCT(BlueprintType)
struct FFPAgentPlatformModel
{
	GENERATED_BODY()

public:

	FFPAgentPlatformModel() = default;
	FFPAgentPlatformModel(const FString& InDisplayName, const FName& InName) : DisplayName(InDisplayName), Name(InName) {}

	// 模型显示名称
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "FPAgent")
	FString DisplayName = "";

	// 模型名称
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "FPAgent")
	FName Name = NAME_None;
};

// 智能体平台配置
USTRUCT(BlueprintType)
struct FFPAgentPlatformConfig
{
	GENERATED_BODY()

public:

	// 完整请求地址
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "FPAgent")
	FString FullUrl = "";

	// API密钥
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "FPAgent")
	FString ApiKey = "";

	// 模型列表
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "FPAgent")
	TArray<FFPAgentPlatformModel> Models;

	// 添加模型
	void AddModel(const TCHAR* InDisplayName, const TCHAR* InName)
	{
		Models.Add(FFPAgentPlatformModel(InDisplayName, InName));
	}
};
```

<a name="fpagentsystem_functionlibrary"></a>
### 函数库

函数库`UFPAgentFunctionLibrary`

```c++
// 创建数据字段
// @param InKey 字段名称
// @param InValue 字段值，函数内部会将其转换为JSON字符串格式
UFUNCTION(BlueprintPure, CustomThunk, meta = (CustomStructureParam = "InValue"), DisplayName = "Make Data Field", Category = "FPAgent|Tool|Json")
static FFPAgentDataField K2_MakeDataField(const FString& InKey, const int32& InValue);
DECLARE_FUNCTION(execK2_MakeDataField);

// 创建JSON字符串
// @param InFields 数据字段数组
// @return JSON格式的字符串
UFUNCTION(BlueprintPure, Category = "FPAgent|Tool|Json")
static FString MakeJsonString(const TArray<FFPAgentDataField>& InFields);

// 从JSON字符串中提取数据字段
// @param InJsonString JSON字符串
// @param InKey 字段名称
// @param OutValue 数据的值
// @return 是否成功提取并转换数据
UFUNCTION(BlueprintPure, CustomThunk, meta = (CustomStructureParam = "OutValue"), DisplayName = "Get Data Field", Category = "FPAgent|Tool|Json")
static bool K2_GetDataField(const FString& InJsonString, const FString& InKey, int32& OutValue);
DECLARE_FUNCTION(execK2_GetDataField);

// 将数据表转换为JSON字符串
// @param InDataTable 数据表对象
// @param InRowNames 只包含这些行，为空表示所有行
// @param InColumns 只保留这些字段，为空表示所有字段
UFUNCTION(BlueprintPure, meta = (AutoCreateRefTerm = "InRowNames, InColumns"), Category = "FPAgent|Tool|Json")
static FString ConvertDataTableToJsonString(UDataTable* InDataTable, const TArray<FName>& InRowNames, const TArray<FName>& InColumns);

// 将纹理转换为Base64字符串
// @param InTexture 纹理对象
// @return Base64编码的字符串
UFUNCTION(BlueprintPure, Category = "FPAgent|Tool|Image")
static FString TextureToBase64(UTexture2D* InTexture);

// 将Base64字符串转换为纹理
// @param InBase64 Base64编码的字符串
// @return 纹理对象，如果转换失败则返回nullptr
UFUNCTION(BlueprintPure, Category = "FPAgent|Tool|Image")
static UTexture2D* Base64ToTexture(const FString& InBase64);

// 将Base64图片进行缩放
// @param InBase64 Base64编码的图片字符串
// @param InSizeScale 缩放比例，在(0, 1)之间取值
// @return 缩放后的Base64编码的图片字符串
UFUNCTION(BlueprintPure, Category = "FPAgent|Tool|Image")
static FString ResizeBase64Image(const FString& InBase64, float InSizeScale = 1.0f);

// 将纹理保存为PNG文件
// @param InTexture 纹理对象
// @param InFullPath PNG文件的完整路径
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool|Image")
static bool SaveTextureToPngFile(UTexture2D* InTexture, const FString& InFullPath);

// 从PNG文件加载纹理
// @param InFullPath PNG文件的完整路径
// @return 纹理对象，如果加载失败则返回nullptr
UFUNCTION(BlueprintCallable, Category = "FPAgent|Tool|Image")
static UTexture2D* LoadTextureFromPngFile(const FString& InFullPath);

// 获取所有平台配置名称
UFUNCTION(BlueprintPure, Category = "FPAgent|Config")
static TArray<FName> GetAllPlatformNames();

// 添加平台配置，会保存到配置文件
// @param InPlatformName 平台名称
// @param InConfig 平台配置数据
UFUNCTION(BlueprintCallable, Category = "FPAgent|Config")
static void AddPlatformConfig(const FName& InPlatformName, const FFPAgentPlatformConfig& InConfig);

// 移除平台配置，会保存到配置文件
// @param InPlatformName 平台名称
UFUNCTION(BlueprintCallable, Category = "FPAgent|Config")
static void RemovePlatformConfig(const FName& InPlatformName);

// 查找平台配置
static const FFPAgentPlatformConfig* FindPlatformConfig(const FName& InPlatformName);

// 查找平台配置
// @param InPlatformName 平台名称
// @param OutConfig 输出平台配置数据
UFUNCTION(BlueprintPure, DisplayName = "FindPlatformConfig", Category = "FPAgent|Config")
static bool K2_FindPlatformConfig(const FName& InPlatformName, FFPAgentPlatformConfig& OutConfig);

// 获取平台所有模型显示名称
// @param InPlatformName 平台名称
UFUNCTION(BlueprintPure, Category = "FPAgent|Config")
static TArray<FString> GetPlatformModelDisplayNames(const FName& InPlatformName);

// 通过模型显示名称获取模型名称
// @param InPlatformName 平台名称
// @param InDisplayName 模型显示名称
UFUNCTION(BlueprintPure, Category = "FPAgent|Config")
static FName GetPlatformModelName(const FName& InPlatformName, const FString& InDisplayName);

// 获取当前模型的显示名称
// @param InPlatformName 平台名称
// @param InModelName 模型名称
UFUNCTION(BlueprintCallable, Category = "FPAgent|Config")
static FString GetPlatformModelDisplayName(const FName& InPlatformName, const FName& InModelName);
```

<a name="fpagentsystem_fpagentsystemeditor"></a>
### FPAgentSystemEditor

智能体系统编辑器模块

* **此模块的主要类**

|类名|描述|
|:-:|:-:|
|FPAgentNode_WaitAgentResponse|等待智能体响应节点|
|FPAgentToolDataTableFactory|智能体工具数据表工厂|
|FPAgentToolBlueprintFactory|智能体工具蓝图工厂|
|FFPAgentToolAssetTypeActions|智能体工具资产类型操作：注册内容浏览器右键菜单的智能体工具蓝图资产|
|FFPAgentToolVariableCustomization|自定义AgentTool蓝图变量的编辑器逻辑|
|UFPAgentToolBlueprint|智能体工具蓝图|
|FPAgentGraphPinFactory|智能体图表引脚工厂|
|SFPAgentToolTableGraphPin|智能体工具表引脚|
|SFPAgentModelGraphPin|智能体模型引脚|
|SFPAgentPlatformGraphPin|智能体平台引脚|

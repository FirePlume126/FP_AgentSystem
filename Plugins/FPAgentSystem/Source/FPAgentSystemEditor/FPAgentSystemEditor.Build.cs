// Copyright FirePlume, All Rights Reserved. Email: fireplume@126.com

using UnrealBuildTool;

public class FPAgentSystemEditor : ModuleRules
{
	public FPAgentSystemEditor(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

		bUsePrecompiled = true;
		PrecompileForTargets = PrecompileTargetsType.None;

		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
			}
			);

		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
				"InputCore",
				"Slate",
				"SlateCore",
				"Kismet",
				"UnrealEd",
				"AssetTools",
				"GraphEditor",
				"BlueprintGraph",
				"KismetCompiler",
				"PropertyEditor",
				"FPAgentSystem",
			}
			);

	}
}

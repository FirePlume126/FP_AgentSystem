// Copyright FirePlume, All Rights Reserved. Email: fireplume@126.com

using UnrealBuildTool;

public class FPAgentSystem : ModuleRules
{
	public FPAgentSystem(ReadOnlyTargetRules Target) : base(Target)
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
				"DeveloperSettings",
				"CoreUObject",
				"Engine",
				"Slate",
				"SlateCore",
				"Json",
				"JsonUtilities",
				"HTTP",
				"ImageWrapper",
			}
			);

	}
}

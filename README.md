# SimpleRoslynHelper

> [!IMPORTANT]
>  This repo was moved to [ArchiToolkit.RoslynHelper](https://github.com/ArchiDog1998/ArchiToolkit/tree/main/src/libraries/ArchiToolkit.RoslynHelper)!
> 



## Usage
``` xml
	<ItemGroup>
		<PackageReference Include="SimpleRoslynHelper" Version="0.9.0" />
	</ItemGroup>
	<PropertyGroup>
		<GetTargetPathDependsOn>$(GetTargetPathDependsOn);GetDependencyTargetPaths</GetTargetPathDependsOn>
	</PropertyGroup>

	<Target Name="GetDependencyTargetPaths" AfterTargets="ResolvePackageDependenciesForBuild">
		<ItemGroup>
			<TargetPathWithTargetPlatformMoniker Include="@(ResolvedCompileFileDefinitions)" IncludeRuntimeDependency="false" />
		</ItemGroup>
	</Target>
```

# SimpleRoslynHelper

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

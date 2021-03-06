# Roslynator <img align="left" width="48px" height="48px" src="http://pihrt.net/images/Roslynator.ico">

* A collection of 500+ analyzers, refactorings and fixes for C#, powered by [Roslyn](http://github.com/dotnet/roslyn).
* [List of analyzers](src/Analyzers/README.md)
* [List of refactorings](src/Refactorings/README.md)
* [List of code fixes for CS diagnostics](src/CodeFixes/README.md)
* [Release notes](ChangeLog.md)

### Donation

Although Roslynator products are free of charge, any [donation](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=BX85UA346VTN6) is welcome and supports further development.

## Extensions for Visual Studio 2017

### Roslynator 2017

* [Roslynator 2017](http://marketplace.visualstudio.com/items?itemName=josefpihrt.Roslynator2017) contains all features - analyzers, refactorings and code fixes for CS diagnostics.

### Roslynator Refactorings 2017

* [Roslynator Refactorings 2017](http://marketplace.visualstudio.com/items?itemName=josefpihrt.RoslynatorRefactorings2017) contains all features except analyzers, it is a subset of Roslynator 2017.
* Use this extension in combination with package [Roslynator.Analyzers](http://www.nuget.org/packages/Roslynator.Analyzers/) or if you are not interested in analyzers at all.

## NuGet Packages

### Roslynator.Analyzers

* Package [Roslynator.Analyzers](http://www.nuget.org/packages/Roslynator.Analyzers/) contains only analyzers.
* Use this package if you want integrate analyzers into you build process.

### Roslynator.CodeFixes

* Package [Roslynator.CodeFixes](http://www.nuget.org/packages/Roslynator.CodeFixes/) contains only code fixes for CS diagnostics.
* Use this package if you want to distribute these code fixes to your team members.

### Roslynator.CSharp

* Package [Roslynator.CSharp](http://www.nuget.org/packages/Roslynator.CSharp/) is a must-have for Roslyn-based development.
* It is built on top of Roslyn API (namely [Microsoft.CodeAnalysis.CSharp](http://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp/)).
* For more information, please see API [overview](src/CSharp/README.md).

### Roslynator.CSharp.Workspaces

* Package [Roslynator.CSharp.Workspaces](http://www.nuget.org/packages/Roslynator.CSharp.Workspaces/) is a must-have for Roslyn-based development.
* It is built on top of Roslyn API (namely [Microsoft.CodeAnalysis.CSharp.Workspaces](http://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp.Workspaces/)).
* For more information, please see API [overview](src/CSharp.Workspaces/README.md).

## Roslynator for VS Code

Currently VS Code does not support distribution of Roslyn-based tools in an extension.
Also it does not support analyzers at all.
Please read the [tutorial](docs/RoslynatorForVisualStudioCode.md) how to install refactorings and code fixes for CS diagnostics.

## Documentation

* [Analyzers vs. Refactorings](docs/AnalyzersVsRefactorings.md)
* [How to Configure Analyzers](docs/HowToConfigureAnalyzers.md)
* [How to Configure Refactorings](docs/HowToConfigureRefactorings.md)

# Blazor.UI.Localizer

Compiled localization library for [Telerik UI for Blazor](https://www.telerik.com/blazor-ui) message keys. Ships satellite assemblies for community-contributed cultures.

## Install

```bash
dotnet add package Blazor.UI.Localizer
```

## Usage

```csharp
using Telerik.Blazor.UI;

// Uses CultureInfo.CurrentUICulture
var text = TextLocalizer.GetText("Grid_Create");
```

Set the UI culture before resolving texts (for example in middleware, `CultureInfo.DefaultThreadCurrentUICulture`, or Blazor localization).

If a key is missing for the current culture (and its parents), `GetText` returns `null`.

## Supported cultures

Satellite resources are included for: `cs-CZ`, `de-DE`, `el-GR`, `fr-FR`, `hr-HR`, `it-IT`, `pl-PL`, `ru-RU`, `sk-SK`, `sl-SI`.

## License

Apache-2.0

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

If a key is missing for the current culture (and its parents), `GetText` returns the key itself.

## Supported cultures

Satellite resources are included for: `cs-CZ`, `da-DK`, `de-DE`, `fa-IR`, `fr-FR`, `hr-HR`, `it-IT`, `nl-BE`, `pt-BR`, `ru-RU`, `sl-SI`, `zh-CN`.

## License

Apache-2.0

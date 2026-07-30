using System.Globalization;
using Telerik.Blazor.UI;

namespace Blazor.UI.Localizer.Tests;

public sealed class TextLocalizerTests : IDisposable
{
	private readonly CultureInfo _originalUiCulture = CultureInfo.CurrentUICulture;

	public TextLocalizerTests()
	{
		TextLocalizer.ClearCache();
	}

	public void Dispose()
	{
		CultureInfo.CurrentUICulture = _originalUiCulture;
		TextLocalizer.ClearCache();
	}

	[Theory]
	[InlineData("cs-CZ", "Aggregate_Average", "Průměr")]
	[InlineData("de-DE", "Aggregate_Average", "Durchschnitt")]
	[InlineData("fr-FR", "Aggregate_Average", "Moyenne")]
	[InlineData("da-DK", "Aggregate_Average", "Gennemsnit")]
	[InlineData("it-IT", "Aggregate_Average", "Media")]
	[InlineData("nl-BE", "Aggregate_Average", "Gemiddeld")]
	[InlineData("pt-BR", "Aggregate_Average", "Média")]
	[InlineData("hr-HR", "Aggregate_Average", "Prosjek")]
	[InlineData("zh-CN", "Aggregate_Average", "平均")]
	[InlineData("fa-IR", "Aggregate_Average", "میانگین")]
	[InlineData("sl-SI", "Aggregate_Average", "Povprečje")]
	[InlineData("el-GR", "Aggregate_Average", "Μέσος όρος")]
	[InlineData("pl-PL", "Aggregate_Average", "Średnia")]
	[InlineData("sk-SK", "Aggregate_Average", "Priemer")]
	public void GetText_ReturnsLocalizedValue_ForCurrentUICulture(string culture, string key, string expected)
	{
		CultureInfo.CurrentUICulture = new CultureInfo(culture);

		var actual = TextLocalizer.GetText(key);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetText_FallsBackToKey_WhenCultureHasNoResources()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("en-US");

		var actual = TextLocalizer.GetText("Aggregate_Average");

		Assert.Equal("Aggregate_Average", actual);
	}

	[Fact]
	public void GetText_FallsBackToKey_WhenKeyIsMissing()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("cs-CZ");

		var actual = TextLocalizer.GetText("Key_That_Does_Not_Exist");

		Assert.Equal("Key_That_Does_Not_Exist", actual);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void GetText_Throws_WhenKeyIsNullOrEmpty(string? key)
	{
		CultureInfo.CurrentUICulture = new CultureInfo("cs-CZ");

		Assert.ThrowsAny<ArgumentException>(() => TextLocalizer.GetText(key!));
	}

	[Fact]
	public void GetText_ReturnsCachedValue_AfterCultureChangeAndRestore()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("cs-CZ");
		var first = TextLocalizer.GetText("Aggregate_Average");

		CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
		_ = TextLocalizer.GetText("Aggregate_Average");

		CultureInfo.CurrentUICulture = new CultureInfo("cs-CZ");
		var second = TextLocalizer.GetText("Aggregate_Average");

		Assert.Equal("Průměr", first);
		Assert.Equal(first, second);
	}

	[Fact]
	public void ClearCache_DoesNotBreakSubsequentLookups()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
		_ = TextLocalizer.GetText("Aggregate_Average");

		TextLocalizer.ClearCache();

		var actual = TextLocalizer.GetText("Aggregate_Average");

		Assert.Equal("Durchschnitt", actual);
	}
}

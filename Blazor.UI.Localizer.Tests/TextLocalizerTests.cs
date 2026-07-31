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
	[InlineData("cs", "Aggregate_Average", "Průměr")]
	[InlineData("cs", "Filter_And", "A")]
	[InlineData("de-DE", "Aggregate_Average", "Durchschnitt")]
	[InlineData("de", "Aggregate_Average", "Durchschnitt")]
	[InlineData("fr-FR", "Aggregate_Average", "Moyenne")]
	[InlineData("it-IT", "Aggregate_Average", "Media")]
	[InlineData("hr-HR", "Aggregate_Average", "Prosjek")]
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
	public void GetText_ReturnsNull_WhenCultureHasNoResources()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("en-US");

		var actual = TextLocalizer.GetText("Aggregate_Average");

		Assert.Null(actual);
	}

	[Fact]
	public void GetText_ReturnsNull_WhenNeutralCultureHasNoResources()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("en");

		var actual = TextLocalizer.GetText("Aggregate_Average");

		Assert.Null(actual);
	}

	[Fact]
	public void GetText_ReturnsNull_WhenKeyIsMissing()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("cs-CZ");

		var actual = TextLocalizer.GetText("Key_That_Does_Not_Exist");

		Assert.Null(actual);
	}

	[Fact]
	public void GetText_ReturnsNull_WhenKeyIsMissing_ForNeutralCulture()
	{
		CultureInfo.CurrentUICulture = new CultureInfo("cs");

		var actual = TextLocalizer.GetText("Key_That_Does_Not_Exist");

		Assert.Null(actual);
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

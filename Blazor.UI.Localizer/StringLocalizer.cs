namespace Telerik.Blazor.UI
{
	public static class TextLocalizer
	{
		public static string GetText(string key) => Localization.GetText(key) ?? key;
	}
}
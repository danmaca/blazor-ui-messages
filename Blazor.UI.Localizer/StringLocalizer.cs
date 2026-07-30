using System.Collections.Concurrent;
using System.Globalization;
using System.Resources;

namespace Telerik.Blazor.UI
{
	public static class TextLocalizer
	{
		private const string ResourceBaseName = "Telerik.Blazor.UI.Messages.TelerikMessages";

		private static readonly ResourceManager ResourceManager =
			new(ResourceBaseName, typeof(TextLocalizer).Assembly);

		// "{culture}|{key}" → resolved string (or key as fallback)
		private static readonly ConcurrentDictionary<string, string> Cache = new(StringComparer.Ordinal);

		public static string GetText(string key)
		{
			ArgumentException.ThrowIfNullOrEmpty(key);

			var culture = CultureInfo.CurrentUICulture;
			var cacheKey = culture.Name + "|" + key;

			return Cache.GetOrAdd(cacheKey, static (_, state) =>
			{
				return Resolve(state.Key, state.Culture) ?? state.Key;
			}, (Key: key, Culture: culture));
		}

		public static void ClearCache() => Cache.Clear();

		// Walk culture → parents with tryParents: false so missing neutral .resx does not throw.
		private static string? Resolve(string key, CultureInfo culture)
		{
			for (var current = culture;
			     current != null && !Equals(current, CultureInfo.InvariantCulture);
			     current = current.Parent)
			{
				var set = ResourceManager.GetResourceSet(current, createIfNotExists: true, tryParents: false);
				var value = set?.GetString(key);
				if (value is not null)
					return value;
			}

			return null;
		}
	}
}

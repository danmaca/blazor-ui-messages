using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Telerik.Blazor.UI
{
	public static class TextLocalizer
	{
		private const string ResourceBaseName = "Telerik.Blazor.UI.Messages.TelerikMessages";

		private static readonly Assembly ResourceAssembly = typeof(TextLocalizer).Assembly;

		// Culture-specific .resources are embedded in the main DLL (no satellites).
		private static readonly ConcurrentDictionary<string, ResourceSet?> ResourceSets = new(StringComparer.OrdinalIgnoreCase);

		// "{culture}|{key}" → resolved string (null when not found)
		private static readonly ConcurrentDictionary<string, string?> Cache = new(StringComparer.Ordinal);

		public static string? GetText(string key)
		{
			ArgumentException.ThrowIfNullOrEmpty(key);

			var culture = CultureInfo.CurrentUICulture;
			var cacheKey = culture.Name + "|" + key;

			return Cache.GetOrAdd(cacheKey, static (_, state) =>
			{
				return Resolve(state.Key, state.Culture);
			}, (Key: key, Culture: culture));
		}

		public static void ClearCache()
		{
			Cache.Clear();
			ResourceSets.Clear();
		}

		// Walk culture → parents. Neutral cultures (e.g. "cs") also try CreateSpecificCulture (e.g. "cs-CZ").
		private static string? Resolve(string key, CultureInfo culture)
		{
			var value = TryGet(key, culture);
			if (value is not null)
				return value;

			if (culture.IsNeutralCulture)
			{
				var specific = CultureInfo.CreateSpecificCulture(culture.Name);
				if (!Equals(specific, culture) && !Equals(specific, CultureInfo.InvariantCulture))
				{
					value = TryGet(key, specific);
					if (value is not null)
						return value;
				}
			}

			return null;
		}

		private static string? TryGet(string key, CultureInfo culture)
		{
			for (var current = culture;
			     current != null && !Equals(current, CultureInfo.InvariantCulture);
			     current = current.Parent)
			{
				var set = GetResourceSet(current);
				var value = set?.GetString(key);
				if (value is not null)
					return value;
			}

			return null;
		}

		private static ResourceSet? GetResourceSet(CultureInfo culture)
		{
			if (string.IsNullOrEmpty(culture.Name))
				return null;

			return ResourceSets.GetOrAdd(culture.Name, static name =>
			{
				var resourceName = ResourceBaseName + "." + name + ".resources";
				var stream = ResourceAssembly.GetManifestResourceStream(resourceName);
				return stream is null ? null : new ResourceSet(stream);
			});
		}
	}
}

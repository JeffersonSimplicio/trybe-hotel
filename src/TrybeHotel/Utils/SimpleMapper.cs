using System.Reflection;

namespace TrybeHotel.Utils;

public static class SimpleMapper {
    public static TTarget Map<TSource, TTarget>(TSource source) where TTarget : new() {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var target = new TTarget();
        var sourceProperties = typeof(TSource)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProperties = typeof(TTarget)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProperty in sourceProperties) {
            var targetProperty = targetProperties.FirstOrDefault(
                p => p.Name.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase) &&
                     p.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)
            );

            if (targetProperty != null && targetProperty.CanWrite) {
                var value = sourceProperty.GetValue(source);
                targetProperty.SetValue(target, value);
            }
        }

        return target;
    }
}

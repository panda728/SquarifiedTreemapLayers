using System.Reflection;

namespace SquarifiedTreeMapForge.Helpers;

internal static class Cache<T>
{
    public static readonly PropCache[] Properties =
    [
        .. typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(x => x.GetIndexParameters().Length == 0)
            .Select((p, i) => new PropCache(p, i))
            .OrderBy(p => p.Index)
    ];
}

public sealed class PropCache(PropertyInfo p, int index)
{
    private readonly IAccessor _accessor = GetAccessor(p);

    public string Name { get; init; } = p.Name;
    public Type PropertyType { get; init; } = p.PropertyType;
    public int Index { get; init; } = index;

    public object? GetValue(object target) => _accessor.GetValue(target);

    internal interface IAccessor
    {
        object? GetValue(object target);
    }

    internal static IAccessor GetAccessor(PropertyInfo property)
    {
        var getterDelegateType = typeof(Func<,>).MakeGenericType(property.DeclaringType!, property.PropertyType);
        var getMethod = property.GetGetMethod();
        return (IAccessor)Activator.CreateInstance(
            typeof(Accessor<,>).MakeGenericType(property.DeclaringType!, property.PropertyType),
            getMethod == null ? null : Delegate.CreateDelegate(getterDelegateType, getMethod))!;
    }

    internal sealed class Accessor<TTarget, TProperty>(Func<TTarget, TProperty>? getter) : IAccessor
    {
        private readonly Func<TTarget, TProperty>? Getter = getter;

        public object? GetValue(object target)
            => Getter == null || typeof(TProperty).IsGenericType ? null : Getter((TTarget)target);
    }
}

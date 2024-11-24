using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Tracer2D
{
    public static class JsonHelpers
    {
        public delegate bool JsonPropertyParser<TResult>(JsonElement element, out TResult result);

        public static string GetString(this JsonElement el, string name)
        {
            return el.GetProperty(name, JsonValueKind.String, (JsonElement element, out string i) => { i = element.GetString(); return true; });
        }

        public static byte GetByte(this JsonElement el, string name)
        {
            return el.GetProperty(name, JsonValueKind.Number, (JsonElement element, out byte i) => element.TryGetByte(out i));
        }

        public static float GetFloat(this JsonElement el, string name)
        {
            return el.GetProperty(name, JsonValueKind.Number, (JsonElement element, out float i) => element.TryGetSingle(out i));
        }

        public static int GetInt(this JsonElement el, string name)
        {
            return el.GetProperty(name, JsonValueKind.Number, (JsonElement element, out int i) => element.TryGetInt32(out i));
        }

        public static T GetEnum<T>(this JsonElement el, string name) where T : unmanaged, Enum
        {
            var typeofT = typeof(T);

            if (el.TryGetProperty(name, out var prop))
                if (prop.ValueKind == JsonValueKind.Number)
                    if (prop.TryGetInt32(out int value))
                        return Unsafe.As<int, T>(ref value);
                    else if (Enum.TryParse<T>(prop.GetString(), out var e))
                        return e;

            throw new InvalidOperationException($"Property {name} cannot be casted to {typeofT.Name}");
        }

        public static JsonElement GetObject(this JsonElement el, string name)
        {
            return el.GetProperty<JsonElement>(name, JsonValueKind.Object, (JsonElement element, out JsonElement i) => { i = element; return true; });
        }

        public static JsonElement GetArray(this JsonElement el, string name)
        {
            return el.GetProperty<JsonElement>(name, JsonValueKind.Array, (JsonElement element, out JsonElement i) => { i = element; return true; });
        }

        public static T GetProperty<T>(this JsonElement el, string name, JsonValueKind kind, JsonPropertyParser<T> parser)
        {
            if (el.TryGetProperty(name, out var prop))
                if (prop.ValueKind == kind)
                    if (parser(prop, out var @value))
                        return value;

            throw new InvalidOperationException($"Property {name} is not a {typeof(T).Name}");
        }
    }
}

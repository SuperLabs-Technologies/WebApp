using System.Diagnostics;
using System.Linq;

namespace WebApp
{
    [DebuggerDisplay("{ToDebuggerDisplay(),nq}", Type = "{ToDebuggerType(),nq}")]
    [DebuggerTypeProxy(typeof(JValueDebugView))]
    partial struct JSONReader
    {
        private const int EllipsisCount = 64;

        private string ToDebuggerType()
        {
            switch (Type)
            {
                case TypeCode.Null: return "JSONReader.Null";
                case TypeCode.Boolean: return "JSONReader.Boolean";
                case TypeCode.Number: return "JSONReader.Number";
                case TypeCode.String: return "JSONReader.String";
                case TypeCode.Array: return "JSONReader.Array";
                case TypeCode.Object: return "JSONReader.Object";
                default: return "JSONReader.Null";
            }
        }

        private string ToDebuggerDisplay()
        {
            switch (Type)
            {
                case TypeCode.Array:
                case TypeCode.Object:
                    var serialized = Serialize(0);
                    if (serialized.Length > EllipsisCount)
                        serialized = serialized.Substring(0, EllipsisCount - 3) + "...";
                    var elementCount = GetElementCount();
                    return string.Format("{0} ({1} {2})", serialized, elementCount.ToString(), elementCount != 1 ? "items" : "item");
                default:
                    return ToString();
            }
        }

        [DebuggerDisplay("{valueString,nq}", Type = "{valueTypeString,nq}")]
        private struct ArrayElement
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public readonly JSONReader value;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly string valueString;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly string valueTypeString;

            public ArrayElement(JSONReader value)
            {
                this.value = value;
                this.valueString = value.ToDebuggerDisplay();
                this.valueTypeString = value.ToDebuggerType();
            }
        }

        [DebuggerDisplay("{valueString,nq}", Name = "[{key,nq}]", Type = "{valueTypeString,nq}")]
        private struct ObjectMember
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly string key;
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public readonly JSONReader value;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly string valueString;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly string valueTypeString;

            public ObjectMember(string key, JSONReader value)
            {
                this.key = key;
                this.value = value;
                this.valueString = value.ToDebuggerDisplay();
                this.valueTypeString = value.ToDebuggerType();
            }
        }

        private sealed class JValueDebugView
        {
            private readonly JSONReader value;

            public JValueDebugView(JSONReader value)
            {
                this.value = value;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object Items
            {
                get
                {
                    switch (value.Type)
                    {
                        case TypeCode.Null:
                        case TypeCode.Boolean:
                        case TypeCode.Number:
                        case TypeCode.String:
                            return null;
                        case TypeCode.Array:
                            return value.Array().Select(it => new ArrayElement(it)).ToArray();
                        case TypeCode.Object:
                            return value.Object().Select(it => new ObjectMember(it.Key.ToString(), it.Value)).ToArray();
                        default:
                            return null;
                    }
                }
            }
        }
    }
}

namespace NetBike.Xml.TypeResolvers
{
    using System;
    using System.Reflection;

    public class XmlTypeResolver : IXmlTypeResolver
    {
        public Func<AssemblyName, Assembly> AssemblyResolver { get; set; }

        public Func<Assembly, string, bool, Type> TypeResolver { get; set; }

        public virtual string GetTypeName(Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }

            return valueType.FullName;
        }

        public Type ResolveTypeName(Type rootType, string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            Type actualType;

            if (!string.IsNullOrEmpty(typeName))
            {
                if (rootType != null && rootType.FullName == typeName)
                {
                    actualType = rootType;
                }
                else
                {
                    actualType = this.ResolveTypeName(typeName);

                    if (rootType != null && !rootType.IsAssignableFrom(actualType))
                    {
                        throw new XmlTypeResolveException(
                            $"Resolved type \"{actualType}\" is not assignable from \"{rootType}\"");
                    }
                }
            }
            else
            {
                actualType = rootType;
            }

            return actualType;
        }

        protected Type ResolveTypeName(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException(nameof(typeName));
            }

            Type type = null;

            if (this.AssemblyResolver != null || this.TypeResolver != null)
            {
                type = Type.GetType(typeName, this.AssemblyResolver, this.TypeResolver);
            }
            else if (typeName.IndexOf(',') > 0)
            {
                type = Type.GetType(typeName, false);
            }
            else
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = Type.GetType(typeName + ", " + assembly.GetName());

                    if (type != null)
                    {
                        break;
                    }
                }
            }

            if (type == null)
            {
                throw new XmlTypeResolveException($"Error at resolve type \"{typeName}\".");
            }

            return type;
        }
    }
}
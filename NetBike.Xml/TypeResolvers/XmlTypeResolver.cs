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
                throw new ArgumentNullException("valueType");
            }

            return valueType.FullName;
        }

        public Type ResolveTypeName(Type rootType, string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
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
                        throw new XmlTypeResolveException(string.Format("Resolved type \"{0}\" is not assignable from \"{1}\"", actualType, rootType));
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
                throw new ArgumentNullException("typeName");
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
                throw new XmlTypeResolveException(string.Format("Error at resolve type \"{0}\".", typeName));
            }

            return type;
        }
    }
}
using System;
using System.Reflection;

namespace LazyRedpaw.FigmaToUnity
{
    public static class TypeHelper
    {
        public static Type FindTypeByName(string typeName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (typeName == types[j].Name) return types[j];
                }
            }
            return null;
        }
    }
}
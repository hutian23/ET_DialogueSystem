using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Timeline
{
    public static partial class ReflectUtility
    {
        private static Dictionary<Type, List<Type>> s_CachedSelfAndBaseTypesMap = new();
        private static Dictionary<Type, FieldInfo[]> s_CachedTypeFieldInfoMap = new();
        private static Dictionary<Type, PropertyInfo[]> s_CachedTypePropertyInfoMap = new();
        private static Dictionary<Type, Dictionary<Type, Attribute[]>> s_CachedTypeAttributesMap = new();

        private static Dictionary<Type, MethodInfo[]> s_CachedTypeMethodInfoMap = new();
        private static Dictionary<Type, Dictionary<FieldInfo, Dictionary<Type, Attribute[]>>> s_CachedTypeFieldAttributesMap = new();

        public static IEnumerable<FieldInfo> GetAllFields(this object target, Func<FieldInfo, bool> predictate = null)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts");
                yield break;
            }

            List<Type> types = GetSelfAndBaseTypes(target);
            for (int i = types.Count - 1; i >= 0; i--)
            {
                if (!s_CachedTypeFieldInfoMap.ContainsKey(types[i]))
                {
                    //仅查找此特定类型中声明的成员，不包含继承
                    s_CachedTypeFieldInfoMap.Add(types[i],
                        types[i].GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public |
                            BindingFlags.DeclaredOnly));
                }

                IEnumerable<FieldInfo> fieldInfos = s_CachedTypeFieldInfoMap[types[i]];
                if (predictate != null)
                {
                    fieldInfos = fieldInfos.Where(predictate);
                }

                foreach (var fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this object target, Func<PropertyInfo, bool> predicate = null)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts");
                yield break;
            }

            List<Type> types = GetSelfAndBaseTypes(target);
            for (int i = types.Count - 1; i >= 0; i--)
            {
                if (!s_CachedTypePropertyInfoMap.ContainsKey(types[i]))
                {
                    s_CachedTypePropertyInfoMap.Add(types[i],
                        types[i].GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public |
                            BindingFlags.DeclaredOnly));
                }

                IEnumerable<PropertyInfo> propertyInfos = s_CachedTypePropertyInfoMap[types[i]];
                if (predicate != null)
                {
                    propertyInfos = propertyInfos.Where(predicate);
                }

                foreach (var propertyInfo in propertyInfos)
                {
                    yield return propertyInfo;
                }
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(this object target, Func<MethodInfo, bool> predicate = null)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts");
                yield break;
            }

            List<Type> types = GetSelfAndBaseTypes(target);
            for (int i = types.Count - 1; i >= 0; i--)
            {
                if (!s_CachedTypeMethodInfoMap.ContainsKey(types[i]))
                {
                    s_CachedTypeMethodInfoMap.Add(types[i],
                        types[i].GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public |
                            BindingFlags.DeclaredOnly));
                }

                IEnumerable<MethodInfo> methodInfos = s_CachedTypeMethodInfoMap[types[i]];
                if (predicate != null)
                {
                    methodInfos = methodInfos.Where(predicate);
                }

                foreach (var methodInfo in methodInfos)
                {
                    yield return methodInfo;
                }
            }
        }

        public static FieldInfo GetField(this object target, string fieldName)
        {
            //进行byte级别比较， 优化 比 String.Compare快
            return GetAllFields(target, f => f.Name.Equals(fieldName, StringComparison.Ordinal)).FirstOrDefault();
        }

        public static PropertyInfo GetProperty(this object target, string propertyName)
        {
            return GetAllProperties(target, p => p.Name.Equals(propertyName, StringComparison.Ordinal)).FirstOrDefault();
        }

        public static MethodInfo GetMethod(this object target, string methodName)
        {
            return GetAllMethods(target, m => m.Name.Equals(methodName, StringComparison.Ordinal)).FirstOrDefault();
        }

        /// <summary>
        /// 将目标类和其所有基类添加到缓存中
        /// </summary>
        private static List<Type> GetSelfAndBaseTypes(object target)
        {
            Type targetType = target.GetType();
            return GetSelfAndBaseTypes(targetType);
        }

        private static List<Type> GetSelfAndBaseTypes(Type targetType)
        {
            if (!s_CachedSelfAndBaseTypesMap.ContainsKey(targetType))
            {
                List<Type> types = new List<Type>() { targetType };
                while (types.Last().BaseType != null)
                {
                    types.Add(types.Last().BaseType);
                }

                // 建立 目标类 和 其所有基类的映射
                s_CachedSelfAndBaseTypesMap.Add(targetType, types);
            }

            return s_CachedSelfAndBaseTypesMap[targetType];
        }

        //获取泛型类型
        public static bool IsSubClassOfRawGeneric(this Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof (type));
            if (generic == null) throw new ArgumentNullException(nameof (generic));

            while (type != null && type != typeof (object))
            {
                if (IsTheRawGenericType(type))
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
            bool IsTheRawGenericType(Type test) => generic == (test.IsGenericType? test.GetGenericTypeDefinition() : test);
        }

        /// <summary>
        /// 获取一个对象的特性
        /// </summary>
        public static T GetAttribute<T>(this object target) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(target);
            return (attributes.Length > 0)? attributes[0] : null;
        }

        /// <summary>
        /// 获取一个类的特性
        /// </summary>
        public static T GetAttribute<T>(this Type targetType) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(targetType);
            return (attributes.Length > 0)? attributes[0] : null;
        }

        public static T[] GetAttributes<T>(this object target) where T : Attribute
        {
            return GetAttributes<T>(target.GetType());
        }

        public static T[] GetAttributes<T>(this Type targetType) where T : Attribute
        {
            //缓存 类 和 其标签映射
            if (!s_CachedTypeAttributesMap.ContainsKey(targetType))
            {
                s_CachedTypeAttributesMap.Add(targetType, new Dictionary<Type, Attribute[]>());
            }

            Type attributeType = typeof (T);
            if (!s_CachedTypeAttributesMap[targetType].ContainsKey(attributeType))
            {
                s_CachedTypeAttributesMap[targetType].Add(attributeType, (T[])targetType.GetCustomAttributes(attributeType, true));
            }

            return (T[])s_CachedTypeAttributesMap[targetType][attributeType];
        }

        /// <summary>
        /// 获取字段的特性
        /// </summary>
        public static T GetFieldAttribute<T>(this object target, string fieldName) where T : Attribute
        {
            T[] attributes = GetFieldAttributes<T>(target, fieldName);

            if (attributes == null || attributes.Length == 0)
            {
                return null;
            }
            else
            {
                return attributes[0];
            }
        }

        public static T[] GetFieldAttributes<T>(this object target, string fieldName) where T : Attribute
        {
            Type targetType = target.GetType();
            if (!s_CachedTypeAttributesMap.ContainsKey(targetType))
            {
                s_CachedTypeAttributesMap.Add(targetType, new Dictionary<Type, Attribute[]>());
            }

            FieldInfo fieldInfo = target.GetField(fieldName);
            if (fieldInfo == null)
            {
                return null;
            }

            if (!s_CachedTypeFieldAttributesMap[targetType].ContainsKey(fieldInfo))
            {
                s_CachedTypeFieldAttributesMap[targetType].Add(fieldInfo, new Dictionary<Type, Attribute[]>());
            }

            Type attributeType = typeof (T);
            if (!s_CachedTypeFieldAttributesMap[targetType][fieldInfo].ContainsKey(attributeType))
            {
                s_CachedTypeFieldAttributesMap[targetType][fieldInfo].Add(attributeType, (T[])fieldInfo.GetCustomAttributes(attributeType, true));
            }

            return (T[])s_CachedTypeFieldAttributesMap[targetType][fieldInfo][attributeType];
        }
    }
}
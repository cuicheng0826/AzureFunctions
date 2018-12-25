using FileFunctions.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileFunctions.Helper
{
    public static class FormHelper
    {
        public static T Get<T>(this IFormCollection fc, string key, T defaultValue = default(T), Func<T, bool> validate = null, string errorMsg = "")
        {
            var values = fc[key];
            var value = String.Empty;
            if (values.Count >= 0)
                value = values[0];
            T result;
            switch (typeof(T).Name)
            {
                case "String":
                    try
                    {
                        if (string.IsNullOrEmpty(value))
                            result = defaultValue;
                        else
                            result = (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch(Exception ex)
                    {
                        result = defaultValue;
                    }
                    break;
                case "Guid":
                    if (string.IsNullOrEmpty(value))
                        result = defaultValue;

                    result = (T)(Guid.Parse(value) as object);
                    break;
                case "Object":
                    result = JsonConvert.DeserializeObject<dynamic>(value);
                    break;
                default:
                    {
                        if (IsNullableGuidType(typeof(T)))
                        {
                            if (string.IsNullOrEmpty(value))
                                result = defaultValue;

                            result = (T)(Guid.Parse(value) as object);
                        }
                        else if (IsNullableDateTimeType(typeof(T)))
                        {
                            if (string.IsNullOrEmpty(value))
                                result = defaultValue;

                            result = (T)(DateTime.Parse(value) as object);
                        }
                        else
                        {
                            try
                            {
                                result = (T)Convert.ChangeType(value, typeof(T));
                            }
                            catch
                            {
                                result = defaultValue;
                            }
                        }
                    }
                    break;
            }

            if (validate != null && !validate(result))
            {
                if (string.IsNullOrEmpty(errorMsg))
                    errorMsg = $"参数{key}格式错误";
                throw new FuncException(errorMsg);
            }

            return result;
        }
        private static bool IsNullableGuidType(Type theType)
        {
            return (theType.IsGenericType && theType == typeof(Nullable<Guid>));
        }
        private static bool IsNullableDateTimeType(Type theType)
        {
            return (theType.IsGenericType && theType == typeof(Nullable<DateTime>));
        }
    }
}

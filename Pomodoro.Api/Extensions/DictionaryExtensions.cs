// <copyright file="DictionaryExtensions.cs" company="PomodoroGroup_GL_BaseCamp">
// Copyright (c) PomodoroGroup_GL_BaseCamp. All rights reserved.
// </copyright>

namespace Pomodoro.Api.Extensions
{
    /// <summary>
    /// Static method extension class for the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value from the <paramref name="dict"/> associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">Type of the returned value.</typeparam>
        /// <param name="dict">The collection to get value from.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>The value associated with the specified <paramref name="key"/>, if the key is found.
        /// Otherwise, the default value for the <typeparamref name="T"/>.</returns>
        public static T GetValueOrDefault<T>(
            this IDictionary<string, object?> dict,
            string key)
            where T : struct
        {
            T value = default;
            if (dict.TryGetValue(key, out var obj))
            {
                if (obj != null)
                {
                    value = (T)obj;
                }
            }

            return value;
        }
    }
}

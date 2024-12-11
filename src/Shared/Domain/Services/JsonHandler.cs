using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Domain.Services
{
    public class JsonHandler
    {
        public static T? LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            });
        }

        public static void SaveToFile<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}

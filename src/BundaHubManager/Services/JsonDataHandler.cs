using System.IO;
using System.Text.Json;
using System.Windows;

namespace BundaHubManager.Services
{
    public class JsonDataHandler
    {
        private readonly string _dataFolderPath;

        public JsonDataHandler()
        {
            var baseDirectory = AppContext.BaseDirectory;
            var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            _dataFolderPath = Path.Combine(projectDirectory, "Data");

            // Ensure the Data directory exists
            if (!Directory.Exists(_dataFolderPath))
            {
                Directory.CreateDirectory(_dataFolderPath);
            }
        }

        public T LoadData<T>(string fileName) where T : new()
        {
            var filePath = Path.Combine(_dataFolderPath, fileName);
            if (!File.Exists(filePath))
            {
                return new T();
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json) ?? new T();
        }

        public void SaveData<T>(string fileName, T data)
        {
            var filePath = Path.Combine(_dataFolderPath, fileName);
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}

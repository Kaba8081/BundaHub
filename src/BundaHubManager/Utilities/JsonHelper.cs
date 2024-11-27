using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundaHubManager.Utilities
{
    public class JsonHelper
    {
        public static void SaveToJson<T>(IEnumerable<T> data, string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(data, JsonOptionsProvider.GetDefaultOptions());
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

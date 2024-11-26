using BundaHubManager.Services;
using BundaHubManager.Services.Interfaces;
using BundaHubManager.UI;
using BundaHubManager.UI.Interfaces;

namespace BundaHubManager
{
    class Program
    {
        private static IManager _manager;
        private static IInterface _interface;

        static void Main()
        {
            _manager = new BundaManager();
            _interface = new BasicInterface(_manager);

            _interface.Run();
        }
    }
}
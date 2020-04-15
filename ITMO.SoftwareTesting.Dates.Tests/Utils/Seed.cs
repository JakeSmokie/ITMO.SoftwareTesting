using System.IO;
using Newtonsoft.Json;

namespace ITMO.SoftwareTesting.Dates.Tests.Utils
{
    public static class Seed<T>
    {
        public static T FromFile(string file)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
        }
    }
}
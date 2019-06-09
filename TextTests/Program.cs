using System.IO;
using TextTests.Handlers;

namespace TextTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = GetFullPath("Tests\\Optimizations\\CommonSubexprOptimization\\");
            var handler = new CommonSubexprTestsHandler(path);
            handler.ProcessAllFilesInDirectory();
        }

        private static string GetFullPath(string directory)
            => Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                directory);
    }
}

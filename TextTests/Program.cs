using System.IO;
using TextTests.Handlers;

namespace TextTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathToTestsDirectory = Path.Combine(new string[] { "Tests", "Optimizations", "CommonSubexprOptimization" });
            var path = GetFullPath(pathToTestsDirectory);
            var handler = new CommonSubexprTestsHandler(path);
            handler.ProcessAllFilesInDirectory();

            var directoryToEliminationTranToTran = Path.Combine("Tests", "Optimizations", "EliminateTranToTranOpt");
            var pathToEliminationTranToTran = GetFullPath(directoryToEliminationTranToTran);
            var handlerElimination = new EliminateTranToTranTestsHandler(pathToEliminationTranToTran);
            handlerElimination.ProcessAllFilesInDirectory();

            var directoryToUnreachableCodeOpt = Path.Combine("Tests", "Optimizations", "UnreachableCodeOpt");
            var pathToUnreachableCodeOpt = GetFullPath(directoryToUnreachableCodeOpt);
            var handlerUnreachable = new UnreachableCodeHandler(pathToUnreachableCodeOpt);
            handlerUnreachable.ProcessAllFilesInDirectory();
        }

        private static string GetFullPath(string directory)
            => Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                directory);
    }
}

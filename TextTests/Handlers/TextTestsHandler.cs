using System;
using System.IO;
using SimpleLang.TACode;
using SimpleLang.Visitors;
using SimpleParser;
using SimpleScanner;

namespace TextTests.Handlers
{
    /// <summary>
    /// A class that helps process all testing files in a directory
    /// </summary>
    public abstract class TextTestsHandler
    {
        public string InputFileKey => "-in";
        public string OutputFileKey => "-out";
        
        protected string DirectoryName { get; }

        /// <summary>
        /// Throws an exception if the directory does not exist
        /// </summary>
        protected TextTestsHandler(string directoryName)
        {
            if (!Directory.Exists(directoryName))
                throw new DirectoryNotFoundException();

            DirectoryName = directoryName;
        }

        /// <summary>
        /// Handles the three-address code depending on the task being tested
        /// </summary>
        protected abstract ThreeAddressCode ProcessTAC(ThreeAddressCode tacContainer);

        /// <summary>
        /// Parses the source code of the program and converts it into a three-address code
        /// </summary>
        protected ThreeAddressCode GetTAC(string source)
        {
            var scanner = new Scanner();
            scanner.SetSource(source, 0);

            var parser = new Parser(scanner);
            if (!parser.Parse())
                throw new Exception("An error occurred while parsing the program.");

            var root = parser.root;
            var tacVisitor = new ThreeAddressCodeVisitor();
            root.Visit(tacVisitor);
            tacVisitor.Postprocess();

            return tacVisitor.TACodeContainer;
        }

        /// <summary>
        /// Processes all files in the directory depending on their type
        /// </summary>
        public void ProcessAllFilesInDirectory()
        {
            var filePaths = Directory.GetFiles(DirectoryName);

            foreach (var path in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);

                if (!fileName.EndsWith(OutputFileKey) && 
                    !fileName.EndsWith(InputFileKey))
                    continue;

                var source = File.ReadAllText(path).Replace('\t', ' ');
                var tacContainer = GetTAC(source);

                if (fileName.EndsWith(InputFileKey))
                    tacContainer = ProcessTAC(tacContainer);

                var newFileName = fileName + "-tac.txt";
                var newPath = Path.Combine(Path.GetDirectoryName(path), newFileName);
                File.WriteAllText(newPath, tacContainer.ToString());
            }
        }
    }
}

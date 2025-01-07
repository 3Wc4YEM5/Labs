using System;
using System.IO;
using System.Collections;
using System.IO.Compression;


namespace CsharpProject
{
    public class Programm
    {


        public static async Task Main()
        {






            List<string> Paths = new List<string>() { "C:\\Users\\user\\source\\repos\\SearchFiles\\bin\\Debug\\net8.0\\wllwelwelewllwe.txt" };

            foreach (string path in Paths)
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }




            string line;
            try
            {
                StreamReader sr = new StreamReader(Paths[0]); // read file

                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                }


                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reading ended.");
            }





            string sourceFile = Paths[0]; // исходный файл
            string compressedFile = Paths[0] + ".gz"; // сжатый файл

            async Task CompressAsync(string sourceFile, string compressedFile)
            {
                // поток для чтения исходного файла
                using FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate);
                // поток для записи сжатого файла
                using FileStream targetStream = File.Create(compressedFile);

                // поток архивации
                using GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
                await sourceStream.CopyToAsync(compressionStream); // копируем байты из одного потока в другой

                Console.WriteLine($"Сжатие файла {sourceFile} завершено.");
                Console.WriteLine($"Исходный размер: {sourceStream.Length}  сжатый размер: {targetStream.Length}");
            }

            await CompressAsync(sourceFile, compressedFile);



        }






        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            Console.WriteLine("File is found '{0}'.", path);
        }


    }

}






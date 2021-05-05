using System;
using System.IO;

namespace MissionMerger
{
    class main
    {
        static void Main(string[] args)
        {
            string[] clArgs = Environment.GetCommandLineArgs();
            Console.WriteLine("Hello World!");
            string baseFilename = "mission.lvl";
            string fileToMerge = "mission2.lvl";
            string newFile = "MERGED.lvl";

            //string ucfbSearch = "ucfb";
            
            //StreamWriter sw = new StreamWriter(baseFilename);
            //StreamReader sr = new StreamReader(fileToMerge);

            BinaryReader br = new BinaryReader(File.Open(fileToMerge,FileMode.Open));
            BinaryReader br2 = new BinaryReader(File.Open(baseFilename, FileMode.Open));

            //read file that needs merged, place in buffer, store size of file.
            br.BaseStream.Position = 8;
            int lengthOfAppendedFile = (int)(br.BaseStream.Length - br.BaseStream.Position);
            Console.WriteLine(String.Concat("Length of file: ",lengthOfAppendedFile));
            byte[] buffer = br.ReadBytes(lengthOfAppendedFile);

            br.Close();

            //read old file, get size
            br2.BaseStream.Position = 8;
            int lengthOfOriginalFile = (int)(br2.BaseStream.Length - br2.BaseStream.Position);
            Int32 totalLengthOfFile = (Int32)lengthOfAppendedFile + (Int32)lengthOfOriginalFile;

            //read old file AND fix ucfb header
            br2.BaseStream.Position = 0;
            byte[] fileCreator = br2.ReadBytes((int)br2.BaseStream.Length);
            Console.WriteLine(String.Concat("Combined Length = ",totalLengthOfFile));
            Buffer.BlockCopy(BitConverter.GetBytes(totalLengthOfFile), 0x00, fileCreator, 0x04, 4);

            br2.Close();

            var fs = new FileStream(newFile, FileMode.Create);
            //BinaryWriter bw = new BinaryWriter(File.Open(baseFilename,FileMode.Open, FileAccess.ReadWrite));
            //bw.BaseStream.Position = (int)bw.BaseStream.Length;
            //bw.Write(buffer);
            // bw.Seek(0x04, 0);
            // bw.Write(totalLengthOfFile);
            fs.Write(fileCreator);
            fs.Write(buffer);

            Console.WriteLine("File Written");

        }
    }
}

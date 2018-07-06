using System;

namespace TelenorDataServer.Show
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new TelenorService();
            Console.WriteLine("*********************");
            Console.WriteLine("Archive files:");
            svc.ShowArchiveFileNames();
            Console.WriteLine("*********************");
            Console.WriteLine();

            Console.WriteLine("*********************");
            Console.WriteLine("Sftp files:");
            svc.ShowSftpFiles();
            Console.WriteLine("*********************");
            Console.WriteLine();
        }
    }
}

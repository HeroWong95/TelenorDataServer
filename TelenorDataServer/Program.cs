using System;

namespace TelenorDataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new TelenorService();
            try
            {
                svc.DeleteDataAsync().GetAwaiter().GetResult();
                svc.SyncFiles();
                svc.ExtractFiles();
                svc.SaveDataAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Logger.Log("Exception:" + e.Message);
                Logger.Log(e.ToString());
            }
            Logger.WriteLogAsync().GetAwaiter().GetResult();
            Console.WriteLine("Finish");
        }
    }
}

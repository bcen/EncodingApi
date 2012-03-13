using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EncodingApi;

namespace EncodingApi.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EncodingServiceClient client = new EncodingServiceClient("id", "key");

            try
            {
                var list = client.GetMediaList();
                foreach (var m in list)
                {
                    Console.WriteLine(m.MediaFile.AbsoluteUri);
                }
            }
            catch (EncodingServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var list = client.GetMediaList();
                foreach (var m in list)
                {
                    Console.WriteLine(m.MediaFile.AbsoluteUri);
                }
            }
            catch (EncodingServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

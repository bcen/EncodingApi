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
                foreach (var m in client.GetMediaList())
                {
                    Console.WriteLine(m.MediaId);
                }
            }
            catch (EncodingServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }

            client.GetMediaListAsync((mediaList) =>
            {
                foreach (var m in mediaList)
                {
                    Console.WriteLine(m.MediaId);
                }
            }, (errors) =>
            {
                foreach (string msg in errors)
                {
                    Console.WriteLine(msg);
                }
            });

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

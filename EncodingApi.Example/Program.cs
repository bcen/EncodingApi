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
            EncodingServiceClient request = new EncodingServiceClient("id", "key");

            try
            {
                var list = request.GetMediaList();
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

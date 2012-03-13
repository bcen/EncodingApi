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
                    Console.WriteLine(m.MediaId);
                    Console.WriteLine(m.MediaFile.AbsoluteUri);
                }
            }
            catch (EncodingServiceException ex)
            {
                // Gets the error message.
                Console.WriteLine(ex.Message);

                // Or a collection of errors.
                foreach (string message in ex.Data["errors"] as ICollection<string>)
                {
                    Console.WriteLine(message);
                }
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

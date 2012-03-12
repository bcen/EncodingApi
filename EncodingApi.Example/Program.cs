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
            EncodingWebRequest request = new EncodingWebRequest("id", "key");

            try
            {
                var list = request.GetMediaList();
                foreach (var m in list)
                {
                    Console.WriteLine(m.MediaFile.AbsoluteUri);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

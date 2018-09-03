using System;
using Newtonsoft.Json;
using Pokora.IA;

namespace Pokora.IaVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DrawIa(Ias.Pce30);
                DrawIa(Ias.Pce31);
                DrawIa(Ias.Pce32);
                DrawIa(Ias.Pce33);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void DrawIa(string json)
        {
            var generator = new IaImageGenerator();
            var tableResult = JsonConvert.DeserializeObject<TableResult>(json);

            generator.GenerateIaImage(tableResult);
            Console.WriteLine("done");
        }
    }
}

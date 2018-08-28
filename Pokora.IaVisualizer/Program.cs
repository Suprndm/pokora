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
                DrawIa("{\"Name\":\"MediumShit\",\"WinRate\":29.099999999999998,\"EllipticAreas\":{\"Fold\":{\"Angle\":5.9690260418206069,\"U\":0.153,\"V\":0.528,\"R\":0.31,\"A\":0.922,\"B\":0.14},\"Check\":{\"Angle\":6.1261056745000966,\"U\":0.768,\"V\":0.949,\"R\":0.316,\"A\":0.032,\"B\":0.738},\"Call\":{\"Angle\":1.2042771838760873,\"U\":0.427,\"V\":0.096,\"R\":0.063,\"A\":0.727,\"B\":0.693},\"Bet\":{\"Angle\":0.62831853071795862,\"U\":0.299,\"V\":0.288,\"R\":0.199,\"A\":0.644,\"B\":0.444},\"Raise\":{\"Angle\":0.68067840827778847,\"U\":0.594,\"V\":0.7,\"R\":0.225,\"A\":0.348,\"B\":0.461},\"AllIn\":{\"Angle\":3.5779249665883754,\"U\":0.367,\"V\":0.896,\"R\":0.167,\"A\":0.248,\"B\":0.807}}}");
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

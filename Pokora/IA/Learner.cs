using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;
using Newtonsoft.Json;
using Pokora.GameMechanisms;

namespace Pokora.IA
{
    public class Learner
    {
        private static readonly Lazy<Learner> Lazy =
            new Lazy<Learner>(() => new Learner());

        public static Learner Instance => Lazy.Value;

        private Learner()
        {
            GenerateNewElipticAreas();
            _tableResults = new ConcurrentBag<TableResult>();
        }

        private ConcurrentBag<TableResult> _tableResults;


        public TableResult GetJsonResult(string json)
        {
            var tableResult = JsonConvert.DeserializeObject<TableResult>(json);
            return tableResult;
        }
        public IDictionary<PlayerState, EllipticArea> GetJsonAreas(string json)
        {

            var tableResult = JsonConvert.DeserializeObject<TableResult>(json);
            return tableResult.EllipticAreas;
        }
        public IDictionary<PlayerState, EllipticArea> GetGoodAreasv2()
        {
            var ellipticAreas = new Dictionary<PlayerState, EllipticArea>
            {
                {PlayerState.Fold, new EllipticArea(0.008,0.178,0.489) },
                {PlayerState.Check , new EllipticArea(0.746,0.100,0.464) },
                {PlayerState.Call ,new EllipticArea(0.422,0.934,0.01)},
                {PlayerState.Bet ,new EllipticArea(0.581,0.978,0.38)},
                {PlayerState.Raise, new EllipticArea(0.09,0.174,0.230)},
                {PlayerState.AllIn,new EllipticArea(0.674,0.951,0.161) },
            };

            return ellipticAreas;
        }

        public IDictionary<PlayerState, EllipticArea> GetGoodAreas()
        {
            var ellipticAreas = new Dictionary<PlayerState, EllipticArea>
             {
                 {PlayerState.Fold, new EllipticArea(0.621,0.975,0.346) },
                 {PlayerState.Check , new EllipticArea(0.29,0.015,0.01) },
                 {PlayerState.Call ,new EllipticArea(0.956,0.2,0.250)},
                 {PlayerState.Bet ,new EllipticArea(0.639,0.998,0.2)},
                 {PlayerState.Raise, new EllipticArea(0.287,0.129,0.450)},
                 {PlayerState.AllIn,new EllipticArea(0.020,0.185,0.448) },
             };

            return ellipticAreas;
        }

        public bool AreAreaOk(IDictionary<PlayerState, EllipticArea> areas)
        {
            return true;
        }

        public IDictionary<PlayerState, EllipticArea> GenerateNewElipticAreas()
        {
            var ellipticAreas = new Dictionary<PlayerState, EllipticArea>
            {
                {PlayerState.Fold, GenerateRandomElipticArea() },
                {PlayerState.Check , GenerateRandomElipticArea() },
                {PlayerState.Call , GenerateRandomElipticArea() },
                {PlayerState.Bet , GenerateRandomElipticArea() },
                {PlayerState.Raise, GenerateRandomElipticArea() },
                {PlayerState.AllIn, GenerateRandomElipticArea() },
            };

            //ellipticAreas = new Dictionary<PlayerState, EllipticArea>
            //{
            //    {PlayerState.Fold, new EllipticArea(0.621,0.975,0.346) },
            //    {PlayerState.Check , new EllipticArea(0.29,0.015,0.01) },
            //    {PlayerState.Call ,new EllipticArea(0.956,0.2,0.250)},
            //    {PlayerState.Bet ,new EllipticArea(0.639,0.998,0.2)},
            //    {PlayerState.Raise, new EllipticArea(0.287,0.129,0.450)},
            //    {PlayerState.AllIn,new EllipticArea(0.020,0.185,0.448) },
            //};

            //_variableSet = new Dictionary<PlayerState, double>
            //{
            //    {PlayerState.Fold ,0.18},
            //    {PlayerState.Check ,0.344},
            //    {PlayerState.Call ,0.605 },
            //    {PlayerState.Bet ,0.499},
            //    {PlayerState.Raise ,0.86},
            //    {PlayerState.AllIn ,0.183},
            //};

            return ellipticAreas;
        }

        private EllipticArea GenerateRandomElipticArea()
        {
            return new EllipticArea()
            {
                A = StaticRandom.Rand(1000) / 1000d,
                B = StaticRandom.Rand(1000) / 1000d,
                R = StaticRandom.Rand(500) / 1000d,
                U = StaticRandom.Rand(1000) / 1000d,
                V = StaticRandom.Rand(1000) / 1000d,
                Angle = StaticRandom.Rand(365)* Math.PI/180,
            };
        }

        public void SaveTableResults(IDictionary<PlayerState, EllipticArea> areas, double winRate)
        {
            _tableResults.Add(new TableResult() { EllipticAreas = areas, WinRate = winRate });
        }

        public void DumpResults(int iteration)
        {
            try
            {
                var logPath = System.IO.Path.GetTempFileName() + iteration + ".csv";
                var logFile = System.IO.File.Create(logPath);
                var logWriter = new System.IO.StreamWriter(logFile);

                var json = JsonConvert.SerializeObject(_tableResults.OrderByDescending(t => t.WinRate).First());

                var csvWriter = new CsvWriter(logWriter);
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.WriteRecords(_tableResults.Select(r => new
                {
                    WinRate = r.WinRate,
                    Fold_A = r.EllipticAreas[PlayerState.Fold].A,
                    Fold_B = r.EllipticAreas[PlayerState.Fold].B,
                    Fold_R = r.EllipticAreas[PlayerState.Fold].R,
                    Fold_U = r.EllipticAreas[PlayerState.Fold].U,
                    Fold_V = r.EllipticAreas[PlayerState.Fold].V,
                    Fold_Angle = r.EllipticAreas[PlayerState.Fold].Angle,

                    Check_A = r.EllipticAreas[PlayerState.Check].A,
                    Check_B = r.EllipticAreas[PlayerState.Check].B,
                    Check_R = r.EllipticAreas[PlayerState.Check].R,
                    Check_U = r.EllipticAreas[PlayerState.Check].U,
                    Check_V = r.EllipticAreas[PlayerState.Check].V,
                    Check_Angle = r.EllipticAreas[PlayerState.Check].Angle,

                    Call_A = r.EllipticAreas[PlayerState.Call].A,
                    Call_B = r.EllipticAreas[PlayerState.Call].B,
                    Call_R = r.EllipticAreas[PlayerState.Call].R,
                    Call_U = r.EllipticAreas[PlayerState.Call].U,
                    Call_V = r.EllipticAreas[PlayerState.Call].V,
                    Call_Angle = r.EllipticAreas[PlayerState.Call].Angle,


                    Bet_A = r.EllipticAreas[PlayerState.Bet].A,
                    Bet_B = r.EllipticAreas[PlayerState.Bet].B,
                    Bet_R = r.EllipticAreas[PlayerState.Bet].R,
                    Bet_U = r.EllipticAreas[PlayerState.Bet].U,
                    Bet_V = r.EllipticAreas[PlayerState.Bet].V,
                    Bet_Angle = r.EllipticAreas[PlayerState.Bet].Angle,

                    Raise_A = r.EllipticAreas[PlayerState.Raise].A,
                    Raise_B = r.EllipticAreas[PlayerState.Raise].B,
                    Raise_R = r.EllipticAreas[PlayerState.Raise].R,
                    Raise_U = r.EllipticAreas[PlayerState.Raise].U,
                    Raise_V = r.EllipticAreas[PlayerState.Raise].V,
                    Raise_Angle = r.EllipticAreas[PlayerState.Raise].Angle,

                    Allin_A = r.EllipticAreas[PlayerState.AllIn].A,
                    Allin_B = r.EllipticAreas[PlayerState.AllIn].B,
                    Allin_R = r.EllipticAreas[PlayerState.AllIn].R,
                    Allin_U = r.EllipticAreas[PlayerState.AllIn].U,
                    Allin_V = r.EllipticAreas[PlayerState.AllIn].V,
                    AllIn_Angle = r.EllipticAreas[PlayerState.AllIn].Angle,
                    Json = JsonConvert.SerializeObject(r),
                }));

                logWriter.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("unable to dump data : " + e);
            }
        }
    }
}

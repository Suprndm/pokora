using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;
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

        private IDictionary<PlayerState, EllipticArea> _ellipticAreas;
        private ConcurrentBag<TableResult> _tableResults;

        public IDictionary<PlayerState, EllipticArea> GetElipticAreas()
        {
            return _ellipticAreas;
        }

        public void GenerateNewElipticAreas()
        {
            //_ellipticAreas = new Dictionary<PlayerState, EllipticArea>
            //{
            //    {PlayerState.Fold, GenerateRandomElipticArea() },
            //    {PlayerState.Check , GenerateRandomElipticArea() },
            //    {PlayerState.Call , GenerateRandomElipticArea() },
            //    {PlayerState.Bet , GenerateRandomElipticArea() },
            //    {PlayerState.Raise, GenerateRandomElipticArea() },
            //    {PlayerState.AllIn, GenerateRandomElipticArea() },
            //};

            _ellipticAreas = new Dictionary<PlayerState, EllipticArea>
            {
                {PlayerState.Fold, new EllipticArea(0.149,0.199,0.31) },
                {PlayerState.Check , new EllipticArea(0.301,0.495,0.166) },
                {PlayerState.Call ,new EllipticArea(0.4,0.42,0.47)},
                {PlayerState.Bet ,new EllipticArea(0.7,0.78,0.031)},
                {PlayerState.Raise, new EllipticArea(0.245,0.618,0.389)},
                {PlayerState.AllIn,new EllipticArea(0.300,0.334,0.1) },
            };

            //_variableSet = new Dictionary<PlayerState, double>
            //{
            //    {PlayerState.Fold ,0.18},
            //    {PlayerState.Check ,0.344},
            //    {PlayerState.Call ,0.605 },
            //    {PlayerState.Bet ,0.499},
            //    {PlayerState.Raise ,0.86},
            //    {PlayerState.AllIn ,0.183},
            //};
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
            };
        }

        public void SaveTableResults(double winRate)
        {
            _tableResults.Add(new TableResult() { EllipticAreas = _ellipticAreas, WinRate = winRate });
        }

        public void DumpResults(int iteration)
        {
            var logPath = System.IO.Path.GetTempFileName() + iteration + ".csv";
            var logFile = System.IO.File.Create(logPath);
            var logWriter = new System.IO.StreamWriter(logFile);

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

                Check_A = r.EllipticAreas[PlayerState.Check].A,
                Check_B = r.EllipticAreas[PlayerState.Check].B,
                Check_R = r.EllipticAreas[PlayerState.Check].R,
                Check_U = r.EllipticAreas[PlayerState.Check].U,
                Check_V = r.EllipticAreas[PlayerState.Check].V,

                Call_A = r.EllipticAreas[PlayerState.Call].A,
                Call_B = r.EllipticAreas[PlayerState.Call].B,
                Call_R = r.EllipticAreas[PlayerState.Call].R,
                Call_U = r.EllipticAreas[PlayerState.Call].U,
                Call_V = r.EllipticAreas[PlayerState.Call].V,


                Bet_A = r.EllipticAreas[PlayerState.Bet].A,
                Bet_B = r.EllipticAreas[PlayerState.Bet].B,
                Bet_R = r.EllipticAreas[PlayerState.Bet].R,
                Bet_U = r.EllipticAreas[PlayerState.Bet].U,
                Bet_V = r.EllipticAreas[PlayerState.Bet].V,

                Raise_A = r.EllipticAreas[PlayerState.Raise].A,
                Raise_B = r.EllipticAreas[PlayerState.Raise].B,
                Raise_R = r.EllipticAreas[PlayerState.Raise].R,
                Raise_U = r.EllipticAreas[PlayerState.Raise].U,
                Raise_V = r.EllipticAreas[PlayerState.Raise].V,

                Allin_A = r.EllipticAreas[PlayerState.AllIn].A,
                Allin_B = r.EllipticAreas[PlayerState.AllIn].B,
                Allin_R = r.EllipticAreas[PlayerState.AllIn].R,
                Allin_U = r.EllipticAreas[PlayerState.AllIn].U,
                Allin_V = r.EllipticAreas[PlayerState.AllIn].V,
            }));

            logWriter.Dispose();
        }
    }
}

using System;
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
            GenerateNewVariableSet();
            _tableResults = new List<TableResult>();
        }

        private IDictionary<PlayerState, double> _variableSet;
        private IList<TableResult> _tableResults;

        public IDictionary<PlayerState, double> GetVariableSet()
        {
            return _variableSet;
        }

        public void GenerateNewVariableSet()
        {
            _variableSet = new Dictionary<PlayerState, double>
            {
                {PlayerState.Fold ,Randomizer.Instance.Random.Next(1000)/1000d },
                {PlayerState.Check ,Randomizer.Instance.Random.Next(1000)/1000d},
                {PlayerState.Call ,Randomizer.Instance.Random.Next(1000)/1000d },
                {PlayerState.Bet ,Randomizer.Instance.Random.Next(1000)/1000d},
                {PlayerState.Raise ,Randomizer.Instance.Random.Next(1000)/1000d },
                {PlayerState.AllIn ,Randomizer.Instance.Random.Next(1000)/1000d },
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

        public void SaveTableResults(double winRate)
        {
            _tableResults.Add(new TableResult() {VariableSet = _variableSet, WinRate = winRate});
        }

        public void DumpResults(int iteration)
        {
            var logPath = System.IO.Path.GetTempFileName() + iteration+".csv";
            var logFile = System.IO.File.Create(logPath);
            var logWriter = new System.IO.StreamWriter(logFile);

            var csvWriter = new CsvWriter(logWriter);
            csvWriter.Configuration.Delimiter = ";";
            csvWriter.WriteRecords(_tableResults.Select(r=> new
            {
                WinRate= r.WinRate,
                Fold = r.VariableSet[PlayerState.Fold],
                Check = r.VariableSet[PlayerState.Check],
                Call = r.VariableSet[PlayerState.Call],
                Bet = r.VariableSet[PlayerState.Bet],
                Raise = r.VariableSet[PlayerState.Raise],
                AllIn = r.VariableSet[PlayerState.AllIn],
            }));

            logWriter.Dispose();
        }
    }
}

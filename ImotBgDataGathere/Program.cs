using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ImotBgDataGathere
{
    class Program
    {
        static void Main(string[] args)
        {

            //Source: https://www.imot.bg/
            var properties = new ImotBgDataGatherer().GatherData(1, 1000).GetAwaiter().GetResult();
            
            using var csvWriter = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-raw-data-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(properties);
            
            File.WriteAllText(
               $"imot.bg-raw-data-{DateTime.Now:yyyy-MM-dd}.json",
               JsonConvert.SerializeObject(properties));
           
       //     *24228 records in imot.bg - raw - data - 2019 - 07 - 06.csv
       //    *
       //    *WHERE[Price] > 0 AND[Year] > 0 AND[Floor] > 0 AND[TotalFloors] > 0
       //*
       //*16083 records in imot.bg - 2019 - 07 - 06.csv


        }
    }
}

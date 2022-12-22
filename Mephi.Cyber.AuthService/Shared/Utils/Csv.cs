using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Mephi.Cyber.AuthService.Core.Models;
namespace Mephi.Cyber.AuthService.Core.Utils
{

    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CSVClass
    {
        public static void TestCsv(List<User> records)
        {

            using (var writer = new StreamWriter("./test.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}

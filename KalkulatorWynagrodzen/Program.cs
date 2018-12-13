﻿using System;
using System.Collections.Generic;
using FileTranslator;
using SalaryCalculator;
using SalaryCalculator.SalaryReport;

namespace Parsers
{
    public class Program
    {
        static void Main()
        {
            while (true)
            {
                try
                {
                    System.Console.WriteLine("PROGRAM DO OBLICZANIA WYNAGRODZENIA ZA NADGODZINY (DODATEK 50%)\n");
                    var monthsWorkingHoursConfiguration
                        = JsonFileConverter.ConvertFromFile<Dictionary<int, int>>("MonthConfig.json");
                    var monthsWorkingHours = new MonthsWorkingHours(monthsWorkingHoursConfiguration);
                    var factors = new Factors
                    {
                        WorkedHours = UserInputConsoleReader.ReadWorkedHours(),
                        HourlyFee = UserInputConsoleReader.ReadHourlyFee(),
                        WorkedMonth = UserInputConsoleReader.ReadMonth()
                    };
                    var monthSalaryReport = new MonthSalaryReport(monthsWorkingHours, factors);
                    var textReport = new SimpleTextReportBuilder()
                        .BuildMontlhyReport(monthSalaryReport);
                    System.Console.WriteLine(textReport);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(
                        $"Wystąpił błąd: {ex.Message}{Environment.NewLine}");
                    System.Console.WriteLine("----------------------------------------------------------------");
                    Main();
                }
            }
        }
    }
}


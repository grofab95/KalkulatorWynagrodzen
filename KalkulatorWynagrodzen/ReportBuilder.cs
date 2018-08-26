﻿using System;
using System.Collections.Generic;
using System.Text;


namespace KalkulatorWynagrodzen
{
    public class ReportBuilder
    {
        private readonly Dictionary<int, int> MonthsWorkingHours;
        public ReportBuilder()
        {
            MonthsWorkingHours = new ConfigurationFileInterpreter<Dictionary<int, int>>("MonthConfig.json")
                .InterpretConfiguration();
        }
        public string BuildIncomeMonthlyReport(
           double workedHours,
           int workedMonth,
           double hourlyFee,
           double overHoursAmount,
           double overHoursGrossIncome,
           double overHoursNetIncome,
           double totalGrossIncome,
           double totalNetIncome)
        {
            Month GetMonthName = new Month();
            var reportBuilder = new StringBuilder();
            reportBuilder
                .AppendLine(Environment.NewLine)
                .AppendLine($"PODSUMOWANIE dla miesiąca {GetMonthName.MonthName(workedMonth)} :")
                .AppendLine("------------------------------------------------------------------")
                .AppendLine(
                    $"Wymiar czasu pracy w tym miesiącu: {MonthsWorkingHours[workedMonth]}h, Przepracowano: {workedHours}h")
                .AppendLine($"Stawka godzinowa (brutto): {hourlyFee}zł {Environment.NewLine}")
                .AppendLine($"Liczba nadgodzin: {overHoursAmount}, wynagrodzenie brutto: {overHoursGrossIncome}zł")
                .AppendLine($"Liczba nadgodzin: {overHoursAmount}, wynagrodzenie netto: {overHoursNetIncome}zł")
                .AppendLine($"Wynagrodzenie całkowite brutto (podstawa + nadgodziny): {totalGrossIncome}zł")
                .AppendLine($"Wynagrodzenie całkowite netto (podstawa + nadgodziny): {totalNetIncome}zł")
                .AppendLine("------------------------------------------------------------------")
                .AppendLine(Environment.NewLine);
            return reportBuilder.ToString();
        }
    }
}

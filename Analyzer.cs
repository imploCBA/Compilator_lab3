using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wpfCopilator
{
    internal class Analyzer
    {
        public static DataTable CheckStrings(string text)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Ошибка", typeof(string));
            table.Columns.Add("Место", typeof(string));



            string[] patterns =
            {
            @"\b\w+\b",             //0
            @"^\s*=\s*",
            @"^\{\s*",
            @"^\s*\b\d+\b\s*",         //3
            @"^\s*'\w+'\s*",
            @"^:\s*",               //5
            @"^'.*?'\s*",
            @"^,\s*",               //7
            @"^\}\s*",
            @"^;\s*"
            };

            Match match;
            Match match2;

            int state = 0;
            int currentIndex = 0;


            while (currentIndex < text.Length)
            {
                char c = text[currentIndex];

                switch (state)
                {
                    case 0: // Состояние 0: Имя переменной
                        match = Regex.Match(text, patterns[0]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length-1;
                            state = 1;
                        }
                        else
                        {
                            AddRowToTable(table, "0", 0, currentIndex);
                        }
                        break;
                    case 1: // Состояние 1: Знак "="
                        match = Regex.Match(text.Substring(currentIndex), patterns[1]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 2;
                        }
                        else
                        {
                            AddRowToTable(table, "1", 0, currentIndex);
                        }
                        break;

                    case 2: // Состояние 2: Открывающая фигурная скобка
                        match = Regex.Match(text.Substring(currentIndex), patterns[2]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 3;
                        }
                        else
                        {
                            AddRowToTable(table, "2", 0, currentIndex);
                        }
                        break;

                    case 3: // Состояние 3: Числовой ключ или Строковый ключ
                        match = Regex.Match(text.Substring(currentIndex), patterns[3]);
                        match2 = Regex.Match(text.Substring(currentIndex), patterns[4]);
                        if (match.Success || match2.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 4;
                        }
                        else
                        {
                            AddRowToTable(table, "3", 0, currentIndex);
                        }
                        break;

                    case 4: // Состояние 4: Двоеточие
                        match = Regex.Match(text.Substring(currentIndex), patterns[5]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 5;
                        }
                        else
                        {
                            AddRowToTable(table, "4", 0, currentIndex);
                        }
                        break;
                    case 5: // Состояние 5: Значение
                        match = Regex.Match(text.Substring(currentIndex), patterns[6]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 6;
                        }
                        else
                        {
                            AddRowToTable(table, "5", 0, currentIndex);
                        }
                        break;
                    case 6: // Состояние 6: Закрывающая фигурная скобка или запятая
                        match = Regex.Match(text.Substring(currentIndex), patterns[8]);
                        match2 = Regex.Match(text.Substring(currentIndex), patterns[7]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 7;
                        }else if (match2.Success)
                        {
                            currentIndex += match.Value.Length;
                            state = 3;
                        }
                        else
                        {
                            AddRowToTable(table, "6", 0, currentIndex);
                        }
                        break;
                    case 7: // Состояние 7: Точка с запятой
                        match = Regex.Match(text.Substring(currentIndex), patterns[9]);
                        if (match.Success)
                        {
                            currentIndex += match.Value.Length - 1;
                            state = 8;
                        }
                        else
                        {
                            AddRowToTable(table, "7", 0, currentIndex);
                        }
                        break;
                    case 8:
                        //MessageBox.Show("Завершился без ошибок.");
                        return getSuccessTable();
                        //AddRowToTable(table, "Завершился без ошибок.", 0, currentIndex);
                        //return table;
                }

                currentIndex++;
            }

            return table;
        }
        private static DataTable getSuccessTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Успешность", typeof(string));
            DataRow row = table.NewRow();
            row["Успешность"] = "Код завершился без ошибок";
            table.Rows.Add(row);
            return table;
        }
        private static void AddRowToTable(DataTable table, string content, int indexStart, int indexEnd)
        {
            DataRow row = table.NewRow();
            row["Ошибка"] = content;
            row["Место"] = $"с {indexStart} по {indexEnd} символ";
            table.Rows.Add(row);
        }
    }
}

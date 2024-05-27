using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;

namespace wpfCopilator
{
    internal class Lexer
    {
        public static DataTable Analyze(string input)
        {
            // Создаем DataTable с нужной структурой
            DataTable table = new DataTable();
            table.Columns.Add("Код", typeof(string));
            table.Columns.Add("Тип", typeof(string));
            table.Columns.Add("Содержание", typeof(string));
            table.Columns.Add("Место положения", typeof(string));

            // Определяем регулярные выражения для разных типов токенов
            var patterns = new (string pattern, string type, string code)[]
            {
            (@"\bdict\b", "ключевое слово", "20"),
            (@"\bfor\b", "ключевое слово", "21"),
            (@"\bin\b", "ключевое слово", "22"),
            (@"\n", "конец строки", "30"),
            (@"'[^']*'", "строка", "40"),
            (@"=", "оператор присваивания", "10"),
            (":", "двоеточие", "11"),
            (",", "запятая", "12"),
            (@"{", "левая фигурная скобка", "13"),
            (@"}", "правая фигурная скобка", "14"),
            (@"\[", "левая квадратная скобка", "15"),
            (@"\]", "правая квадратная скобка", "16"),
            (@"\(", "левая круглая скобка", "17"),
            (@"\)", "правая круглая скобка", "18"),
            (@"\d+", "целое без знака", "1"),
            (@"\s+", "разделитель", "99"), // Игнорируем пробелы
            };

            int position = 0;

            // Функция для добавления строки в таблицу
            void AddRow(string code, string type, string content, int start, int end)
            {
                if (content == "\n")
                {
                    content = "\\n";
                }
                table.Rows.Add(code, type, content, $"С {start + 1} по {end + 1} символ");
            }

            // Проход по всей строке
            while (position < input.Length)
            {
                bool matched = false;

                foreach (var (pattern, type, code) in patterns)
                {
                    var match = Regex.Match(input.Substring(position), pattern);

                    if (match.Success && match.Index == 0)
                    {
                        if (code != "99") // Игнорируем пробелы
                        {
                            AddRow(code, type, match.Value, position, position + match.Length - 1);
                        }
                        position += match.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    position++;
                }
            }

            return table;
        }
    }
}

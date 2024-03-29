﻿using NARFUClassLib.Extensions;
using NARFUClassLib.Structs;
using NARFUClassLib.Tools;

namespace RaspisanieNARFU
{
    public class Program
    {
        private const string url = @"https://ruz.narfu.ru/?timetable&group=16626";

        private static readonly Dictionary<ConsoleKey, string> menu_keys = new()
        {
            { ConsoleKey.Escape, "выйти из программы" },
            { ConsoleKey.A, "вывести расписание недели" },
            { ConsoleKey.T, "вывести расписание на сегодня" },
            { ConsoleKey.N, "вывести расписание на завтра" },
            { ConsoleKey.W, "выбрать неделю (1-6)" },
            { ConsoleKey.F, "выбрать формат вывода расписания"},
            { ConsoleKey.R, "заново загрузить список лекций с сайта"}
        };
        private static string week = "1";
        private static bool short_format = true;
        private static void LoadAllLectures(string week)
        {
            Dictionary<string, Auditorium> filters = new Filters(Directory.GetCurrentDirectory() + @"\filters.xml").GetFilters();
            LectureExtensions.lectures = LectureExtensions.GetLecturesList(new Uri(url), week).FilterLectures(filters);
        }
        private static void Show_Menu()
        {
            Console.WriteLine("===========МЕНЮ ВЫБОРА===========");
            for (int i = 0; i < menu_keys.Count; i++)
            {
                KeyValuePair<ConsoleKey, string> pair = menu_keys.ElementAt(i);
                Console.WriteLine($"{i}) Нажмите клавишу {pair.Key}, чтобы {pair.Value}.");
            }
        }
        private static string? GetResultByKey()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            Console.Clear();
            switch (key.Key)
            {
                case ConsoleKey.A:
                    return LectureExtensions.lectures.GetGroupedLectureList().GetLecturesString(short_format);
                case ConsoleKey.F:
                    Console.Write("Короткий формат вывода расписания(y - да/n - нет): ");
                    short_format = Console.ReadLine().ToLower() == "y";
                    return "Формат успешно установлен.";
                case ConsoleKey.T:
                    return LectureExtensions.lectures.Where(x => DateTime.Parse(x.dayofweek).DayOfWeek == DateTime.Now.DayOfWeek).ToList()
                                         .GetGroupedLectureList()
                                         .GetLecturesString(short_format);
                case ConsoleKey.N:
                    return LectureExtensions.lectures.Where(x => DateTime.Parse(x.dayofweek).DayOfWeek == DateTime.Now.AddDays(1).DayOfWeek).ToList()
                                         .GetGroupedLectureList()
                                         .GetLecturesString(short_format);
                case ConsoleKey.W:
                    Console.Write("Введите номер недели (1-6): ");
                    week = Console.ReadLine();
                    LoadAllLectures(week);
                    return "Неделя по умолчанию успешно установлена.";
                case ConsoleKey.R:
                    LoadAllLectures(week);
                    return "Иформация с сайта успешно загружена.";
                case ConsoleKey.Escape:
                    return null;
                default:
                    return "Такого пункта не существует.";
            }
        }
        internal static void Main()
        {
            Console.WriteLine("Загрузка и парсинг данных. Пожалуйста подожди...");
            LoadAllLectures(week);
            Console.Clear();
            do
            {
                Show_Menu();
                string? result = GetResultByKey();
                if (result == null)
                    break;
                Console.WriteLine(result);
                Console.WriteLine("=======> Введите любую клавишу, чтобы вернуться в меню <=======");
                Console.ReadKey();
                Console.Clear();
            } while (true);
        }
    }
}
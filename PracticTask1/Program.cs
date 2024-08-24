using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            var vacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };

            var availableWorkingDaysOfWeekWithoutWeekends = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            List<DateTime> vacations = new List<DateTime>();
            int allVacationCount = 0;

            Random gen = new Random();

            foreach (var vacationList in vacationDictionary)
            {
                var dateList = vacationList.Value;
                int vacationCount = 28;

                while (vacationCount > 0)
                {
                    DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime end = new DateTime(DateTime.Today.Year, 12, 31);
                    int range = (end - start).Days;

                    var startDate = start.AddDays(gen.Next(range));

                    if (availableWorkingDaysOfWeekWithoutWeekends.Contains(startDate.DayOfWeek.ToString()))
                    {
                        int vacationLength = (vacationCount <= 7) ? 7 : gen.Next(7, 15);
                        var endDate = startDate.AddDays(vacationLength);

                        bool canCreateVacation = true;

                        if (vacations.Any(element => element >= startDate && element < endDate))
                        {
                            canCreateVacation = false;
                        }

                        if (canCreateVacation)
                        {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {
                                vacations.Add(dt);
                                dateList.Add(dt);
                            }
                            allVacationCount++;
                            vacationCount -= vacationLength;
                        }
                    }
                }
            }

            foreach (var vacationList in vacationDictionary)
            {
                var setDateList = vacationList.Value;
                Console.WriteLine($"Дни отпуска {vacationList.Key} : ");
                if (setDateList.Count > 0)
                {
                    foreach (var date in setDateList)
                    {
                        Console.WriteLine(date);
                    }
                }
                else
                {
                    Console.WriteLine("Нет назначенных отпусков.");
                }
            }

            Console.ReadKey();
        }
    }
}
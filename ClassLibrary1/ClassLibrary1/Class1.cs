namespace ClassLibrary1
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            List<TimeSpan> busyPeriods = new List<TimeSpan>();
            for (int i = 0; i < startTimes.Length; i++)
            {
                busyPeriods.Add(startTimes[i]);
                busyPeriods.Add(startTimes[i].Add(TimeSpan.FromMinutes(durations[i])));
            }

            List<string> availablePeriods = new List<string>();
            TimeSpan currentStart = beginWorkingTime;

            // Сортируем занятые интервалы для корректной обработки
            busyPeriods.Sort();

            for (int i = 0; i < busyPeriods.Count; i += 2)
            {
                TimeSpan busyStart = busyPeriods[i];
                TimeSpan busyEnd = busyPeriods[i + 1];

                // Проверяем свободное время до начала занятого интервала
                while (currentStart + TimeSpan.FromMinutes(consultationTime) <= busyStart)
                {
                    availablePeriods.Add($"{currentStart:hh\\:mm}-{currentStart.Add(TimeSpan.FromMinutes(consultationTime)):hh\\:mm}");
                    currentStart = currentStart.Add(TimeSpan.FromMinutes(30)); // Увеличиваем на 30 минут
                }

                // Перемещаем текущее время на конец занятого интервала
                currentStart = busyEnd > currentStart ? busyEnd : currentStart;
            }

            // Проверяем свободное время после последнего занятого интервала до конца рабочего дня
            while (currentStart + TimeSpan.FromMinutes(consultationTime) <= endWorkingTime)
            {
                availablePeriods.Add($"{currentStart:hh\\:mm}-{currentStart.Add(TimeSpan.FromMinutes(consultationTime)):hh\\:mm}");
                currentStart = currentStart.Add(TimeSpan.FromMinutes(30)); // Увеличиваем на 30 минут
            }

            return availablePeriods.ToArray();
        }

    }
}


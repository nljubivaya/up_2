using ClassLibrary1;
namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private Calculations _calculations;

        [TestInitialize]
        public void Setup()
        {
            _calculations = new Calculations();
        }

        // ѕровер€ет ситуацию, когда все врем€ свободно
        [TestMethod]
        public void Test_All_Free_Periods()
        {
            TimeSpan[] startTimes = { }; // Ќет зан€тых периодов
            int[] durations = { }; // Ќет длительностей
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00",
                "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00",
                "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00",
                "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00"
            }, result);
        }

        // ѕровер€ет доступные интервалы при частично зан€тых периодах
        [TestMethod]
        public void Test_Partial_Busy_Periods()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0) };
            int[] durations = { 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(12, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "09:30-10:00", "10:30-11:00", "11:00-11:30", "11:30-12:00"
            }, result);
        }

        // ѕровер€ет доступные интервалы при нескольких консультаци€х
        [TestMethod]
        public void Test_Multiple_Consultations()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = { 30, 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "09:30-10:00", "10:30-11:00", "11:30-12:00", "12:00-12:30",
                "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30",
                "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30",
                "16:30-17:00"
            }, result);
        }

        // ѕровер€ет, что доступные интервалы пусты, если весь день зан€т
        [TestMethod]
        public void Test_Fully_Busy_Day()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 480 }; // 8 часов
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] { }, result); // ќжидаем пустой список
        }

        // ѕровер€ет доступные интервалы при отсутствии зан€тых периодов
        [TestMethod]
        public void Test_No_Busy_Periods()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00",
                "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00",
                "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00",
                "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00"
            }, result);
        }

        // ѕровер€ет доступные интервалы, когда консультаци€ начинаетс€ в начале рабочего дн€
        [TestMethod]
        public void Test_ConsultationTime_At_Start_Of_Workday()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00",
                "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00",
                "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00",
                "16:00-16:30", "16:30-17:00"
            }, result);
        }

        // ѕровер€ет доступные интервалы, когда консультаци€ совпадает с зан€тыми периодами
        [TestMethod]
        public void Test_ConsultationTime_Exactly_Busy_Periods()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = { 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "09:00-09:30", "09:30-10:00", "10:30-11:00", "11:30-12:00",
                "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00",
                "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00",
                "16:00-16:30", "16:30-17:00"
            }, result);
        }

        // ѕровер€ет, что врем€ консультации больше, чем свободные интервалы
        [TestMethod]
        public void Test_ConsultationTime_Larger_Than_Free_Slots()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = { 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 120;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(8, result.Length);
        }

        // ѕровер€ет доступные интервалы при множественных свободных промежутках
        [TestMethod]
        public void Test_ConsultationTime_Available_At_Multiple_Intervals()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(14, 0, 0) };
            int[] durations = { 30, 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {

                 "09:00-09:30", "09:30-10:00", "10:30-11:00", "11:00-11:30",
                "11:30-12:00", "12:30-13:00", "13:00-13:30", "13:30-14:00",
                "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30",
                "16:30-17:00"
            }, result);
        }

        // ѕровер€ет доступные интервалы при единственном зан€том периоде в начале рабочего дн€
        [TestMethod]
        public void Test_Single_Busy_Period_At_Start()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] {
                "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00",
                "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00",
                "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00",
                "16:00-16:30", "16:30-17:00"
            }, result);
        }


        [TestMethod]
        public void AvailablePeriods_NoAppointments_CorrectlyIdentifiesAllSlots()
        {
            // Ќет зан€тых интервалов
            TimeSpan[] startTimes = { };
            int[] durations = { };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(12, 0, 0);
            int consultationTime = 30;

            List<string> expected = new List<string>();
            for (TimeSpan time = beginWorkingTime; time < endWorkingTime; time = time.Add(TimeSpan.FromMinutes(30)))
            {
                expected.Add($"{time:hh\\:mm}-{time.Add(TimeSpan.FromMinutes(consultationTime)):hh\\:mm}");
            }

            string[] actual = new Calculations().AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(expected.ToArray(), actual);
        }
        [TestMethod]
        public void AvailablePeriods_OverlappingAppointments_DoesNotReturnOverlappingSlots()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0), new TimeSpan(9, 30, 0) };
            int[] durations = { 45, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(10, 0, 0);
            int consultationTime = 30;

            string[] actual = new Calculations().AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            // ѕровер€ем, что функци€ не вернула интервал 9:45-10:15, который перекрываетс€ с зан€тыми
            Assert.IsFalse(actual.Contains("09:45-10:15"));
        }


        [TestMethod]
        public void AvailablePeriods_NoFreeSlots_ReturnsEmptyArray()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 600 }; // 10 часов
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(10, 0, 0);
            int consultationTime = 30;

            string[] actual = new Calculations().AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(0, actual.Length); // ƒолжен вернуть пустой массив
        }


        [TestMethod]
        public void AvailablePeriods_IncorrectConsultationTime_DoesNotReturnInvalidSlots()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(10, 0, 0);
            int consultationTime = 75; // «апрос на 75 минут, а слоты только по 30

            string[] actual = new Calculations().AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreNotEqual(1, actual.Length); // Ќе должно быть ни одного интервала
        }

        [TestMethod]
        public void AvailablePeriods_EdgeCase_DoesNotReturnIncorrectSlots()
        {
            // раевой случай: начало рабочего дн€ совпадает с началом встречи
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            int[] durations = { 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(12, 0, 0);
            int consultationTime = 30;

            string[] actual = new Calculations().AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
            Assert.IsFalse(actual.Contains("09:00-09:30")); // Ќе должно быть интервала 9:00-9:30
        }

    }
}


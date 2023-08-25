using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DPUtils.Systems.DateTime
{
    public class TimeManager : MonoBehaviour
    {
        [Header("Time Settings")]
        [SerializeField]
        private int weekLength = 7;
        [SerializeField]
        private int daysInMonth = 28;
        [SerializeField]
        private int monthsInYear = 4;
        [SerializeField]
        private int hoursInDay = 24;
        [SerializeField]
        private int weeksInSeason = 4;
        
        [Header("Date & Time Settings")] [Range(1, 28)]
        public int dateInMonth;
        [Range(1, 4)] public int season;
        [Range(1, 99)] public int year;
        [Range(0, 24)] public int hour;
        [Range(0, 60)] public int minutes;

        private DateTime DateTime;

        [Header("Time Speed Settings")] public int TickMinutesIncrease = 10;
        public float TimeBetweenTicks = 1f;
        private float currentTimeBetweenTicks = 0;

        public static UnityAction<DateTime> OnDateTimeChanged;

        private void Awake()
        {
            DateTime = new DateTime(dateInMonth, season, year, hour, minutes, weekLength, daysInMonth, monthsInYear, hoursInDay, weeksInSeason);
        }

        private void Start()
        {
            OnDateTimeChanged?.Invoke(DateTime);
        }

        private void Update()
        {
            currentTimeBetweenTicks += Time.deltaTime;

            if (currentTimeBetweenTicks >= TimeBetweenTicks)
            {
                currentTimeBetweenTicks = 0;
                Tick();
            }
        }

        void Tick()
        {
            AdvanceTime();
        }

        void AdvanceTime()
        {
            DateTime.AdvanceMinutes(TickMinutesIncrease);
            OnDateTimeChanged?.Invoke(DateTime);
        }
    }
    
    [System.Serializable]
    public struct DateTime
    {
        #region Fields

        private Days day;
        private int date;
        private int year;
        
        private int hour;
        private int minutes;
        
        private Season season;

        private int totalNumDays;
        private int totalNumWeeks;
        
        private int weekLength;
        private int daysInMonth;
        private int monthsInYear;
        private int hoursInDay;
        private int weeksInSeason;

        #endregion
        
        #region Properties
        
        public Days Day => day;
        public int Date => date;
        public int Year => year;
        public int Hour => hour;
        public int Minutes => minutes;
        public Season Season => season;
        public int TotalNumDays => totalNumDays;
        public int TotalNumWeeks => totalNumWeeks;
        public int CurrentWeek => totalNumWeeks % 16 == 0 ? 16 : totalNumWeeks % 16;

        #endregion
        
        #region Constructors
        
        public DateTime(int date, int season, int year, int hour, int minutes, int weekLength, int daysInMonth, int monthsInYear, int hoursInDay, int weeksInSeason)
        {
            this.day = (Days)(date % weekLength);
            if(day == 0) day = (Days)7;
            this.date = date;
            this.year = year;
            this.hour = hour;
            this.minutes = minutes;
            this.season = (Season)season;
            totalNumDays = date + (daysInMonth * (int)this.season);
            totalNumDays = totalNumDays + (monthsInYear * (year - 1));
            totalNumWeeks = 1 + totalNumDays / weekLength;
            
            this.weekLength = weekLength;
            this.daysInMonth = daysInMonth;
            this.monthsInYear = monthsInYear;
            this.hoursInDay = hoursInDay;
            this.weeksInSeason = weeksInSeason;
        }

        #endregion
        
        #region TimeAdvancement

        public void AdvanceMinutes(int secondsToAdvance)
        {
            if(minutes + secondsToAdvance >= 60)
            {
                AdvanceHours((minutes + secondsToAdvance) / 60);
                minutes = (minutes + secondsToAdvance) % 60;
            }
            else
            {
                minutes += secondsToAdvance;
            }
        }
        
        public void AdvanceHours(int hoursToAdvance)
        {
            if(hour + hoursToAdvance >= hoursInDay)
            {
                AdvanceDays((hour + hoursToAdvance) / hoursInDay);
                hour = (hour + hoursToAdvance) % hoursInDay;
            }
            else
            {
                hour += hoursToAdvance;
            }
        }
        
        public void AdvanceDays(int daysToAdvance)
        {
            if(date + daysToAdvance >= weekLength)
            {
                AdvanceWeeks((date + daysToAdvance) / weekLength);
                date = (date + daysToAdvance) % weekLength;
            }
            else
            {
                date += daysToAdvance;
            }
            
            day = (Days)(date % weekLength);
            if(day == 0) day = (Days)weekLength;
            
        }
        
        public void AdvanceWeeks(int weeksToAdvance)
        {
            totalNumWeeks += weeksToAdvance;
            
            if(totalNumWeeks % weeksInSeason == 0)
            {
                AdvanceSeason();
            }
        }
        
        public void AdvanceSeason()
        {
           if(Season == Season.Winter)
           {
               season = Season.Spring;
               AdvanceYear();
           }
           else
           {
               season++;
           }
        }
        
        public void AdvanceYear()
        {
            year++;
        }
        
        #endregion
        
        #region BooleanChecks
        
        public bool IsDay(Days dayToCheck)
        {
            return day == dayToCheck;
        }
        
        public bool IsDate(int dateToCheck)
        {
            return date == dateToCheck;
        }
        
        public bool IsMonth(int monthToCheck)
        {
            return season == (Season)monthToCheck;
        }

        public bool IsYear(int yearToCheck)
        {
            return year == yearToCheck;
        }
        
        public bool IsHour(int hourToCheck)
        {
            return hour == hourToCheck;
        }
        
        public bool IsMinutes(int minutesToCheck)
        {
            return minutes == minutesToCheck;
        }
        
        public bool IsSeason(Season seasonToCheck)
        {
            return season == seasonToCheck;
        }
        
        public bool IsNight()
        {
            return hour >= 20 || hour <= 5;
        }
        
        public bool IsMorning()
        {
            return hour >= 6 && hour <= 11;
        }
        
        public bool IsAfternoon()
        {
            return hour >= 12 && hour <= 16;
        }
        
        public bool IsWeekend()
        {
            return day == Days.Sat || day == Days.Sun;
        }
        
        public bool IsWeekday()
        {
            return day != Days.Sat && day != Days.Sun;
        }
        
        public bool IsWeek(int weekToCheck)
        {
            return CurrentWeek == weekToCheck;
        }

        #endregion
        
        #region KeyDates

        public DateTime NewYearsDate(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(1, 1, year, 6, 0, weekLength, daysInMonth, monthsInYear, hoursInDay, weeksInSeason);
        }
        
        #endregion

        #region StartOfSeason
        
        public DateTime StartOfSpring(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(1, 1, year, 6, 0, weekLength, daysInMonth, monthsInYear, hoursInDay, weeksInSeason);
        }
        
        public DateTime StartOfSummer(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(1, 2, year, 6, 0, weekLength, daysInMonth, monthsInYear, hoursInDay, weeksInSeason);
        }
        
        public DateTime StartOfAutumn(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(1, 3, year, 6, 0, weekLength, daysInMonth, monthsInYear, hoursInDay, weeksInSeason);
        }
        
        public DateTime StartOfWinter(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(1, 4, year, 6, 0, weekLength, daysInMonth, monthsInYear, hoursInDay, weeksInSeason);
        }

        #endregion
        
        #region ToString
        
        public override string ToString()
        {
            return $"{day} {date} {season} {year} {hour}:{minutes}";
        }
        
        public string DateToString()
        {
            return $"{day} {date + 1}";
        }

        public string TimeToString()
        {
            if (hour == 24)
                hour = 0;
            string hourString = hour.ToString();
            string minutesString = minutes.ToString();
            if (hour < 10)
                hourString = "0" + hourString;
            if(minutes < 10)
                minutesString = "0" + minutesString;
            return $"{hourString}:{minutesString}";
        }
        
        #endregion
        
    }
    
    [System.Serializable]
    public enum Season
    {
        Spring = 1,
        Summer = 2,
        Autumn = 3,
        Winter = 4
    }
    [System.Serializable]
    public enum Days
    {
        NULL = 0,
        Mon = 1,
        Tue = 2,
        Wed = 3,
        Thu = 4,
        Fri = 5,
        Sat = 6,
        Sun = 7
    }

}
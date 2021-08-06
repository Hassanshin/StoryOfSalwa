using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public TimeDiff lastOpen;
    public TimeDiff daily;
    public TimeDiff weekly;

    public override void Initialization()
    {
        base.Initialization();

        PlayerData data = DataManager.Instance.playerData;

        lastOpen.TimeCheck(data.TimeSinceLastOpened);
        if (lastOpen.IsNewTime)
        {
            Debug.Log("IsNew Time");
            data.SetTimeSinceLastOpened(DateTime.Now);
        }

        daily.TimeCheck(data.LoginDaily);
        if (daily.IsNewDay)
        {
            Debug.Log("IsNew Day");
            data.SetTimeDaily(DateTime.Now);
        }

        weekly.TimeCheck(data.LoginWeekly);
        if (weekly.IsNewWeek)
        {
            Debug.Log("IsNew Week");
            data.SetTimeWeekly(DateTime.Now);
        }

        SaveManager.Instance.Save();
    }
}

[System.Serializable]
public class TimeDiff
{
    private TimeSpan _timeDifference = new TimeSpan();
    public TimeSpan TimeDifference => _timeDifference;

    public bool IsNewTime;
    public bool IsNewDay;
    public bool IsNewWeek;

    public void TimeCheck(DateTime dateTime)
    {
        _timeDifference = SubtractFromNow(dateTime);

        IsNewTime = _timeDifference.TotalSeconds > 0;
        IsNewDay = _timeDifference.TotalDays > 1;
        IsNewWeek = _timeDifference.TotalDays > 7;
    }

    private TimeSpan SubtractFromNow(DateTime dateTime)
    {
        DateTime tempDate = new DateTime(
            dateTime.Year, dateTime.Month, dateTime.Day,
            DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);

        return DateTime.Now.Subtract(tempDate);
    }
}
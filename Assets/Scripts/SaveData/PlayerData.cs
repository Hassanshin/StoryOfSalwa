using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string PlayerName;

    public string timeSinceLastOpened;
    public string loginDaily;
    public string loginWeekly;

    public DateTime TimeSinceLastOpened
    {
        get
        {
            if (string.IsNullOrEmpty(timeSinceLastOpened))
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.Parse(timeSinceLastOpened);
            }
        }
    }

    public DateTime LoginDaily
    {
        get
        {
            if (string.IsNullOrEmpty(loginDaily))
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.Parse(loginDaily);
            }
        }
    }

    public DateTime LoginWeekly
    {
        get
        {
            if (string.IsNullOrEmpty(loginWeekly))
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.Parse(loginWeekly);
            }
        }
    }

    public void SetTimeSinceLastOpened(DateTime _date)
    {
        timeSinceLastOpened = _date.ToString();
    }

    public void SetTimeDaily(DateTime _date)
    {
        loginDaily = _date.ToString();
    }

    public void SetTimeWeekly(DateTime _date)
    {
        loginWeekly = _date.ToString();
    }
}
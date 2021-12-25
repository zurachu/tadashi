using System;
using UnityEngine;

public struct ScoreParameter
{
    public int CompletedCount;
    public int UncompletedCount;
    public int OverAttachedPenaltyCount;
    public float WorkingTime;

    static private readonly int CompletedUnitPrice = 1000;
    static private readonly int UncompletedUnitPrice = -1000;
    static private readonly int OverAttachedPenaltyUnitPrice = -500;
    static private readonly int WorkingTimeBonusPerSecond = 100;
    static private readonly float WorkingTimeBonusZeroSeconds = 300;

    public int CompletedPrice => CompletedCount * CompletedUnitPrice;
    public int UncompletedPrice => UncompletedCount * UncompletedUnitPrice;
    public int OverAttachedPenaltyPrice => OverAttachedPenaltyCount * OverAttachedPenaltyUnitPrice;
    public int WorkingTimeBonus => Mathf.Max((int)((WorkingTimeBonusZeroSeconds - WorkingTime) * WorkingTimeBonusPerSecond), 0);
    public int Price => CompletedPrice + UncompletedPrice + OverAttachedPenaltyPrice + WorkingTimeBonus;

    public string WorkingTimeText
    {
        get
        {
            var timeSpan = new TimeSpan(0, 0, 0, 0, (int)(WorkingTime * 1000));
            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.TotalHours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
            }
            else
            {
                return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:D3}";
            }
        }
    }
}

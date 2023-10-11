using UnityEngine;

namespace Screamies.Utility
{
    public struct BoolTimer
    {
        float resetTime;

        public void Set(float time)
        {
            resetTime = Mathf.Max(resetTime, Time.time + time);
        }

        public void Set(float time, bool overwrite)
        {
            if (overwrite)
                resetTime = Time.time + time;
            else Set(time);
        }

        public void Reset()
        {
            resetTime = Time.time - 1;
        }

        public bool Value { get { return Time.time <= resetTime; } }

        public static implicit operator bool(BoolTimer bt)
        {
            return bt.Value;
        }

    }

}

using JHelpers;

namespace Signals
{
    public class TimerUpdatedSignal : ISignal
    {
        public readonly float RemainigTime;

        public TimerUpdatedSignal(float remainigTime)
        {
            RemainigTime = remainigTime;
        }
    }
}

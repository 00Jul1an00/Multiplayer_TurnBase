using JHelpers;
using Units;

namespace Signals
{
    public class ChangeTurnSignal : ISignal
    {
        public readonly Membersip ChangeToMembersip;
        public readonly int TurnCount;

        public ChangeTurnSignal(Membersip membersip, int turnCount) 
        {
            ChangeToMembersip = membersip;
            TurnCount = turnCount;
        }
    }
}

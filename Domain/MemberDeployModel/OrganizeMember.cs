using LogicRoll.Domain.Battle;

namespace LogicRoll.Domain
{
    public class OrganizeMember
    {
        public readonly int MasterID;
        public BattlerStrategy Strategy { get; private set; }

        public OrganizeMember(int masterID, BattlerStrategy strategy)
        {
            MasterID = masterID;
            Strategy = strategy;
        }

        internal void SetStrategy(BattlerStrategy strategy)
        {
            Strategy = strategy;
        }
    }
}

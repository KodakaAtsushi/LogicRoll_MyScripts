using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LogicRoll.Domain.Battle;
using LogicRoll.Domain.Entity;
using R3;

namespace LogicRoll.Domain
{
    public class StrategySelectModel
    {
        readonly IEnumerable<OrganizeMember> members;
        OrganizeMember selectedMember;
        BattlerStrategy selectedStrategy;

        public bool IsDirty { get; private set; } = false; // SaveとLoadの時にfalse

        // events
        public Observable<OrganizeMember> OnMemberSelect => onMemberSelect;
        Subject<OrganizeMember> onMemberSelect = new();
        public Observable<BattlerStrategy> OnStrategySelect => onStrategySelect;
        Subject<BattlerStrategy> onStrategySelect = new();

        public StrategySelectModel(IEnumerable<OrganizeMember> members)
        {
            foreach(var member in members)
            {
                var isDuplicated = members.GroupBy(m => m.MasterID).Any(group => group.Count() > 1);

                if(isDuplicated) throw new Exception("A member with same masterID already exists");
            }

            this.members = members;
        }

        public void SaveStrategy()
        {
            selectedMember.SetStrategy(selectedStrategy);
            onMemberSelect.OnNext(selectedMember); // 更新されたstrategyをviewに反映するために通知

            IsDirty = true;
        }

        public void SelectMember(int masterID)
        {
            selectedMember = members.First(m => m.MasterID == masterID);
            onMemberSelect.OnNext(selectedMember);

            SelectStrategy((int)selectedMember.Strategy);
        }

        public void SelectStrategy(int strategy)
        {
            selectedStrategy = (BattlerStrategy)strategy;
            onStrategySelect.OnNext(selectedStrategy);
        }

        // 配置メンバーのみ、strategyを更新
        public void Load(IEnumerable<OrganizeMemberEntity> deployedMembers)
        {
            foreach(var dM in deployedMembers)
            {
                var member = members.First(m => m.MasterID == dM.MasterID);
                member.SetStrategy(dM.Strategy);
            }

            IsDirty = false;
        }
        
        internal void ResetDirtyFlag()
        {
            IsDirty = false;
        }
    }
}
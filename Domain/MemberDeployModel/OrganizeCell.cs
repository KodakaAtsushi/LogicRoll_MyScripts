using System;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace LogicRoll.Domain
{
    public class OrganizeCell
    {
        public OrganizeMember Member;
        public readonly Vector2Int GridPos;

        public bool IsEmpty => Member == null;

        // events
        public Subject<int> OnMemberIncrement { get; private set; } = new();
        public Subject<(int, Vector2Int)> OnDeploy { get; private set; } = new();

        public OrganizeCell(Vector2Int gridPos)
        {
            GridPos = gridPos;
        }

        public void Deploy(OrganizeMember member)
        {
            Assert.IsTrue(IsEmpty);

            Member = member;
            OnMemberIncrement.OnNext(1);
            OnDeploy.OnNext((Member.MasterID, GridPos));

            Assert.IsFalse(IsEmpty);
        }

        public OrganizeMember Pick()
        {
            Assert.IsFalse(IsEmpty);

            var picked = Member;
            Member = null;
            OnMemberIncrement.OnNext(-1);

            Assert.IsTrue(IsEmpty);
            return picked;
        }

        public OrganizeMember Replace(OrganizeMember Member)
        {
            Assert.IsFalse(IsEmpty);

            var replacedMember = Pick();
            Deploy(Member);

            Assert.IsFalse(IsEmpty);
            return replacedMember;
        }

        public bool IsMatchID(int masterID)
        {
            return Member?.MasterID == masterID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using R3;

namespace LogicRoll.Domain
{
    public class OrganizeMemberPool
    {
        readonly List<OrganizeMember> members = new();
        public IEnumerable<OrganizeMember> Members => members;

        // debug用
        public int Count => members.Count();

        // event
        public Subject<int> OnReturn { get; private set; }= new();

        public OrganizeMemberPool(OrganizeMember[] members)
        {
            for(int i = 0; i < members.Length; i++)
            {
                if(Contains(members[i].MasterID)) throw new Exception("A member with the same masterID already exists");
                
                this.members.Add(members[i]);
            }
        }

        public OrganizeMember Pick(int masterID)
        {
            var member = members.First(m => m.MasterID == masterID);
            members.Remove(member);

            return member;
        }

        public void Return(OrganizeMember member)
        {
            if(Contains(member.MasterID)) throw new Exception("A member with the same masterID already exists");
            members.Add(member);
            OnReturn.OnNext(member.MasterID);
        }

        public bool Contains(int masterID)
        {
            return members.Any(m => m.MasterID == masterID);
        }
    }
}

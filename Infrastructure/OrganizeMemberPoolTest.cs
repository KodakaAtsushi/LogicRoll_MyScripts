using System;
using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;

namespace LogicRoll.Infrastructure.Organize
{
    public class OrganizeMemberPoolTest
    {
        [Test]
        public void ContainsTest()
        {
            var members = CreateMembers(1, 2, 3);
            var pool = new OrganizeMemberPool(members);

            Assert.That(pool.Contains(1));
            Assert.That(pool.Contains(2));
            Assert.That(pool.Contains(3));

            Assert.That(!pool.Contains(0));
            Assert.That(!pool.Contains(4));
        }

        [Test]
        // 一度ピックしたメンバーを再度ピックしてしまわないか
        public void DuplicatePickTest()
        {
            var member = CreateMembers(1);
            var pool = new OrganizeMemberPool(member);

            pool.Pick(1);
            Assert.That(Pick, Throws.InstanceOf<Exception>());

            void Pick()
            {
                pool.Pick(1);
            }
        }

        [Test]
        // 追加していないメンバーをピックしてしまわないか
        public void NonMemberPickTest()
        {
            var member = CreateMembers(1);
            var pool = new OrganizeMemberPool(member);

            Assert.That(NonMemberPick, Throws.InstanceOf<Exception>());

            void NonMemberPick()
            {
                pool.Pick(2);
            }
        }

        [Test]
        // retrun と pickの前後で、内部状態が正しく更新されているか
        public void RetrunTest()
        {
            var member = CreateMembers();
            var pool = new OrganizeMemberPool(member);

            Assert.That(!pool.Contains(1));
            pool.Return(new OrganizeMember(1, BattlerStrategy.Assassination));
            Assert.That(pool.Contains(1));
            pool.Pick(1);
            Assert.That(!pool.Contains(1));
        }

        [Test]
        // 同じmasterIDで初期化したとき、エラーが出るか
        public void DuplicateMasterIDTest()
        {
            var member = CreateMembers(0, 0);

            Assert.That(New, Throws.InstanceOf<Exception>());

            void New()
            {
                new OrganizeMemberPool(member);
            }
        }

        OrganizeMember[] CreateMembers(params int[] masterIDs)
        {
            var members = new OrganizeMember[masterIDs.Length];

            for(int i = 0; i < masterIDs.Length; i++)
            {
                members[i] = new OrganizeMember(masterIDs[i], BattlerStrategy.Assassination);
            }

            return members;
        }
    }
}
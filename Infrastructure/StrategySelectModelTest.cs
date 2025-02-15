using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using NUnit.Framework;
using System.Collections.Generic;
using R3;
using LogicRoll.Domain.Entity;
using System;
using UnityEditor.VersionControl;

namespace LogicRoll.Infrastructure.Organize
{
    public class StrategySelectModelTest
    {
        [Test]
        public void StrategySaveTest()
        {
            var member = new OrganizeMember(0, BattlerStrategy.Assassination);
            var model = CreateModel(member);

            model.SelectMember(0);
            model.SelectStrategy((int)BattlerStrategy.Escort);

            Assert.That(member.Strategy == BattlerStrategy.Assassination); // まだ更新されていない
            
            model.SaveStrategy();

            Assert.That(member.Strategy == BattlerStrategy.Escort);
        }

        [Test]
        public void ReselectMemberTest()
        {
            var member_0 = new OrganizeMember(0, BattlerStrategy.Assassination);
            var member_1 = new OrganizeMember(1, BattlerStrategy.Assassination);
            var model = CreateModel(member_0, member_1);

            model.SelectMember(0);
            model.SelectMember(1); // reselect
            model.SelectStrategy((int)BattlerStrategy.Escort);
            model.SaveStrategy();

            Assert.That(member_0.Strategy == BattlerStrategy.Assassination);
            Assert.That(member_1.Strategy == BattlerStrategy.Escort);
        }

        [Test]
        public void ReselectStrategyTest()
        {
            var member_0 = new OrganizeMember(0, BattlerStrategy.Assassination);
            var model = CreateModel(member_0);

            model.SelectMember(0);
            model.SelectStrategy((int)BattlerStrategy.Escort);
            model.SelectStrategy((int)BattlerStrategy.Raid); // reselect
            model.SaveStrategy();
            
            Assert.That(member_0.Strategy == BattlerStrategy.Raid);
        }

        [Test]
        // loadしたとき、配置メンバーだけstrategyが更新されるか
        public void LoadTest()
        {
            var member_0 = new OrganizeMember(0, BattlerStrategy.Assassination);
            var member_1 = new OrganizeMember(1, BattlerStrategy.Assassination);
            var model = CreateModel(member_0, member_1);

            // 第一引数の10は適当な数字
            var entity = new OrganizeMemberEntity(10, 0, BattlerStrategy.Escort);

            model.Load(new OrganizeMemberEntity[1]{entity});

            Assert.That(member_0.Strategy == BattlerStrategy.Escort); // 更新される
            Assert.That(member_1.Strategy == BattlerStrategy.Assassination); // 更新されない
        }

        [Test]
        // 同じmasterIDを持つmemberで初期化したとき、エラーが出るか
        public void DuplicateMasterIDTest()
        {
            var members = CreateMembers(0, 0);

            Assert.That(New, Throws.InstanceOf<Exception>());

            void New()
            {
                CreateModel(members);
            }
        }

        [Test]
        public void IdDirtyTest()
        {
            var model = CreateModel(CreateMembers(0));

            Assert.That(!model.IsDirty);

            model.SelectMember(0);
            Assert.That(!model.IsDirty);

            model.SelectStrategy(0);
            Assert.That(!model.IsDirty);

            model.SaveStrategy();
            Assert.That(model.IsDirty);

            var entity = new OrganizeMemberEntity(0, 0, BattlerStrategy.Assassination); // 引数に意味はない
            model.Load(new OrganizeMemberEntity[1]{entity});
            Assert.That(!model.IsDirty);
        }

        StrategySelectModel CreateModel(params OrganizeMember[] members)
        {
            var model = new StrategySelectModel(members);

            return model;
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
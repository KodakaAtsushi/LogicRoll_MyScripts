using System;
using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using LogicRoll.Domain.Entity;
using NUnit.Framework;
using UnityEngine;

namespace LogicRoll.Infrastructure.Organize
{
    public class MemberDeployModelTest
    {
        Vector2Int boardSize = new Vector2Int(9, 7);

        [Test]
        public void PickAndDeployTest()
        {
            var model = CreateDeployModel(0, 1, 2);

            model.Pick(0);
            model.Deploy(new Vector2Int(0, 0));

            model.Pick(1);
            model.Deploy(new Vector2Int(1, 1));

            model.Pick(2);
            model.Deploy(new Vector2Int(2, 2));
        }

        [Test]
        // pick する前にdeployしてしまったとき、エラーが出るか
        public void DeployBeforePickTest()
        {
            var model = CreateDeployModel(0);

            Assert.That(DeployBeforePick, Throws.InstanceOf<Exception>());

            void DeployBeforePick()
            {
                model.Deploy(new Vector2Int(0, 0));
            }
        }

        [Test]
        // ピック状態中に、さらにピックしたとき、エラーが出るか
        public void DeplicatePickTest()
        {
            var model = CreateDeployModel(0, 1);

            model.Pick(0);
            Assert.That(DeplicatePick, Throws.InstanceOf<Exception>());
            
            void DeplicatePick()
            {
                model.Pick(1);
            }
        }

        [Test]
        // 存在しないメンバーをピックしたとき、エラーが出るか
        public void DoesNotExistMemberPickTest()
        {
            var model = CreateDeployModel();

            Assert.That(DoesNotExistMemberPick, Throws.InstanceOf<Exception>());

            void DoesNotExistMemberPick()
            {
                model.Pick(0);
            }
        }

        [Test]
        // poolに返した後、再度ピック出来るようになっているか
        public void ReturnToPoolTest()
        {
            var model = CreateDeployModel(0);

            model.Pick(0);
            model.ReturnToPool();
            model.Pick(0);
        }

        [Test]
        // 一度deployした後、boardからpickしなおせるか
        public void PickAfterDeployTest()
        {
            var model = CreateDeployModel(0);

            model.Pick(0);
            model.Deploy(new Vector2Int(0, 0));
            model.Pick(0);
        }

        [Test]
        // 既に配置されているcellに再配置すると、memberが交換されるか
        public void ReplaceTest()
        {
            var model = CreateDeployModel(0, 1);

            model.Pick(0);
            model.Deploy(Vector2Int.zero);
            model.Pick(1);
            model.Deploy(Vector2Int.zero); // ここでmasterID:0がpoolに返される
        }

        [Test]
        public void IsDirtyTest()
        {
            var model = CreateDeployModel(0);

            Assert.That(!model.IsDirty);

            model.Pick(0);
            Assert.That(!model.IsDirty);

            model.Deploy(new Vector2Int(0, 0));
            Assert.That(model.IsDirty);

            model.Load(new OrganizeEntity(0));
            Assert.That(!model.IsDirty);
        }

        MemberDeployModel CreateDeployModel(params int[] masterIDs)
        {
            var members = CreateMembers(masterIDs);
            var pool = new OrganizeMemberPool(members);
            var board = new OrganizeBoard(boardSize);
            var deployModel = new MemberDeployModel(pool, board);

            return deployModel;
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

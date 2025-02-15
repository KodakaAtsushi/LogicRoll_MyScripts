using System;
using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using NUnit.Framework;
using UnityEngine;

namespace LogicRoll.Infrastructure.Organize
{
    public class OrganizeCellTest
    {
        Vector2Int gridPos = Vector2Int.zero;

        // cellのIsEmptyに関しては、cellのクラス内で正常に機能することを保証する
        // Assert.That(cell.IsEmpty)を毎回このテスト内に書いていると煩雑になるため

        [Test]
        // 追加したメンバーで正しく変数が更新されているか
        public void DeployTest()
        {
            var cell = CreateCell();
            var member = CreateMember();

            cell.Deploy(member);

            Assert.That(cell.Member == member);
        }

        [Test]
        // pickしたとき、追加したメンバーが返ってくるのか
        public void PickTest()
        {
            var cell = CreateCell();
            var member = CreateMember();

            cell.Deploy(member);
            var picked = cell.Pick();

            Assert.That(member == picked);
        }

        [Test]
        // replaceしたとき、追加したメンバーが返ってくるのか
        public void ReplaceTest()
        {
            var cell = CreateCell();
            var firstMember = CreateMember();

            cell.Deploy(firstMember);
            var scecondMember = CreateMember();
            var replacedMember = cell.Replace(scecondMember);

            Assert.That(firstMember == replacedMember);
            Assert.That(cell.Member == scecondMember);
        }

        [Test]
        // 重複して追加したときに、エラーが出るか
        public void DuplicateDeployTest()
        {
            var cell = CreateCell();
            var member = CreateMember();

            cell.Deploy(member);
            Assert.That(Deploy, Throws.InstanceOf<Exception>());

            void Deploy()
            {
                cell.Deploy(member);
            }
        }

        [Test]
        // 何も追加されていないcellからpickしたとき、エラーが出るか
        public void PickFromEmptyTest()
        {
            var cell = CreateCell();

            Assert.That(Pick, Throws.InstanceOf<Exception>());

            void Pick()
            {
                cell.Pick();
            }
        }

        [Test]
        // 何も追加されていないcellからreplaceしたとき、エラーが出るか
        public void ReplaceFromEmptyTest()
        {
            var cell = CreateCell();

            Assert.That(Replace, Throws.InstanceOf<Exception>());
            
            void Replace()
            {
                var member = CreateMember();
                cell.Replace(member);
            }
        }

        OrganizeCell CreateCell()
        {
            return new OrganizeCell(gridPos);
        }

        OrganizeMember CreateMember(int masterID = 0, BattlerStrategy strategy = BattlerStrategy.Assault)
        {
            return new OrganizeMember(masterID, strategy);
        }
    }
    
}
# 概要

現在チーム開発中の”LogicRoll”にて、私が担当したキャラクター編成シーンのソースコードになります<br>
フォルダ分けや、依存関係のルールに関してはチームメンバーに指導いただきました。<br>
また、イラスト素材に関しても別のチームメンバーに製作していただいたものになります。<br>


https://github.com/user-attachments/assets/959e09b5-0609-46e7-973c-07a9c798de1f


# 使用技術
Unity(Test Runner使用), C#(R3, UniTask含む)

# 一部ソースコード

メンバーを配置するmodelクラス

```csharp
using System;
using System.Linq;
using LogicRoll.Domain.Entity;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace LogicRoll.Domain
{
    public class MemberDeployModel : IDisposable
    {
        readonly OrganizeMemberPool pool;
        readonly OrganizeBoard board;

        readonly static Vector2Int inValidGridPos = new Vector2Int(-1, -1);

        PickedData pickedData;

        public bool IsDirty { get; private set; } = false; // SaveとLoadの時にfalse

        // public events
        public Observable<int> OnMemberCountChange => board.MemberCount;
        public Observable<(int, Vector2Int)> OnDeploy => board.OnDeploy;
        public Observable<int> OnReturn => pool.OnReturn;

        CompositeDisposable disposables = new();

        public MemberDeployModel(OrganizeMemberPool pool, OrganizeBoard board)
        {
            this.pool = pool;
            this.board = board;

            board.MemberCount.Subscribe(_ =>
            {
                IsDirty = true;
            }).AddTo(disposables);

            IsDirty = false;

            disposables.Add(board);
        }

        public void Pick(int masterID)
        {
            Assert.IsNull(pickedData);

            if(pool.Contains(masterID))
            {
                var member = pool.Pick(masterID);
                pickedData = new PickedData(member, inValidGridPos);
            }
            else if(board.Contains(masterID))
            {
                (var member, var gridPos) = board.Pick(masterID);
                pickedData = new PickedData(member, gridPos);
            }

            Assert.IsNotNull(pickedData);
        }

        public void Deploy(Vector2Int gridPos)
        {
            Assert.IsNotNull(pickedData);

            if(!board.IsValidGridPos(gridPos))
            {
                // cellの数を変えたときに出る可能性あり
                Debug.Log($"board外に配置されたbattlerがいたため、poolに戻しました id : {pickedData.member.MasterID}, gridPos : {gridPos}");
                ReturnToPool();
                return;
            }

            if (board.IsEmpty(gridPos))
            {
                board.Deploy(gridPos, pickedData.member);
            }
            else
            {
                var replacedMember = board.Replace(gridPos, pickedData.member);

                if (pickedData.gridPos == inValidGridPos)
                {
                    pool.Return(replacedMember);
                }
                else
                {
                    board.Deploy(pickedData.gridPos, replacedMember);
                }
            }

            pickedData = null;

            Assert.IsNull(pickedData);
        }

        public void ReturnToPool()
        {
            Assert.IsNotNull(pickedData);

            pool.Return(pickedData.member);

            pickedData = null;

            Assert.IsNull(pickedData);
        }

        public void Load(OrganizeEntity entity)
        {
            Clear();
            
            var members = entity.Members;
            var startPositions = entity.StartPositions;

            for(int i = 0; i < members.Count; i++)
            {
                var id = members[i].MasterID;
                var gridPos = startPositions[i].GridPos;
                Pick(id);
                Deploy(gridPos);
            }

            Debug.Log("loaded");
            IsDirty = false;
        }

        // saveされたときに呼び出される
        public void ResetDirtyFlag()
        {
            IsDirty = false;
        }

        public (OrganizeMemberEntity[], OrganizeStartPosition[]) CreateEntity()
        {
            var filledCells = board.Cells.Where(c => !c.IsEmpty).ToArray();
            var count = filledCells.Length;

            var members = new OrganizeMemberEntity[count];
            var startPositions = new OrganizeStartPosition[count];

            for(int i = 0; i < count; i++)
            {
                var cell = filledCells[i];

                var masterID = cell.Member.MasterID;
                var strategy = cell.Member.Strategy;
                var gridPos = cell.GridPos;

                members[i] = new OrganizeMemberEntity(i, masterID, strategy);
                startPositions[i] = new OrganizeStartPosition(i, gridPos);
            }

            return (members, startPositions);
        }

        void Clear()
        {
            if(pickedData != null) ReturnToPool();

            var members = board.PickAll();

            foreach(var member in members)
            {
                pool.Return(member);
            }
        }

        void LogState()
        {
            Debug.Log($"pool count : {pool.Count}");
            Debug.Log($"deploy count : {board.Count}");
            Debug.Log($"have pickedData : {pickedData != null}");
            Debug.Log("-------------------------------------");
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        class PickedData
        {
            public readonly OrganizeMember member;
            public readonly Vector2Int gridPos;

            public PickedData(OrganizeMember member, Vector2Int gridPos)
            {
                this.member = member;
                this.gridPos = gridPos;
            }
        }
    }
}
```

上記クラスのテストコード

```csharp
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
        // poolにいるメンバーを追加・配置できるか
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
            model.Pick(0); // エラーが出なければOK
        }

        [Test]
        // 一度deployした後、boardからpickしなおせるか
        public void PickAfterDeployTest()
        {
            var model = CreateDeployModel(0);

            model.Pick(0);
            model.Deploy(new Vector2Int(0, 0));
            model.Pick(0); // エラーが出なければOK
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

            model.Deploy(new Vector2Int(0, 0)); // deployされたときにisDirtyをtrueに更新
            Assert.That(model.IsDirty);

            model.Load(new OrganizeEntity(0));  // loadご、isDirtyがfalseに戻る
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

```

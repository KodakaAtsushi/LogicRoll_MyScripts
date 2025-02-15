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
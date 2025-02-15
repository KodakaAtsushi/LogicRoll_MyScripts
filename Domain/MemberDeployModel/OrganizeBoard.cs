using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace LogicRoll.Domain
{
    public class OrganizeBoard : IDisposable
    {
        readonly OrganizeCell[,] cells;
        
        // debug 用
        public IEnumerable<OrganizeCell> Cells => cells.Cast<OrganizeCell>();
        public int Count => cells.Cast<OrganizeCell>().ToArray().Count(c => !c.IsEmpty);
        
        // events
        public readonly ReactiveProperty<int> MemberCount = new();
        public Subject<(int, Vector2Int)> OnDeploy = new();
        CompositeDisposable disposables = new();

        public OrganizeBoard(Vector2Int gridSize)
        {
            cells = new OrganizeCell[gridSize.x, gridSize.y];

            for(int y = 0; y < cells.GetLength(1); y++)
            {
                for(int x = 0; x < cells.GetLength(0); x++)
                {
                    var cell = new OrganizeCell(new Vector2Int(x, y));

                    cell.OnMemberIncrement.Subscribe(increment =>
                    {
                        MemberCount.Value += increment;
                        Assert.IsTrue(MemberCount.Value >= 0);
                    }).AddTo(disposables);

                    cell.OnDeploy.Subscribe(data =>
                    {
                        OnDeploy.OnNext(data);
                    }).AddTo(disposables);

                    cells[x, y] = cell;
                }
            }
        }

        public void Deploy(Vector2Int gridPos, OrganizeMember member)
        {
            cells[gridPos.x, gridPos.y].Deploy(member);
        }

        public (OrganizeMember, Vector2Int) Pick(int masterID)
        {
            var cell = Cells.First(cell => cell.IsMatchID(masterID));
            var member = cell.Pick();

            return (member, cell.GridPos);
        }

        public IEnumerable<OrganizeMember> PickAll()
        {
            var deployedCells = Cells.Where(c => !c.IsEmpty);

            var members = new List<OrganizeMember>();

            foreach(var cell in deployedCells)
            {
                members.Add(cell.Pick());
            }

            return members;
        }

        public OrganizeMember Replace(Vector2Int gridPos, OrganizeMember member)
        {
            return cells[gridPos.x, gridPos.y].Replace(member);
        }

        public bool IsEmpty(Vector2Int gridPos)
        {
            return cells[gridPos.x, gridPos.y].IsEmpty;
        }

        public bool Contains(int masterID)
        {
            return Cells.Any(cell => cell.IsMatchID(masterID));
        }

        public bool IsValidGridPos(Vector2Int gridPos)
        {
            return 0 <= gridPos.x && gridPos.x < cells.GetLength(0)
                && 0 <= gridPos.y && gridPos.y < cells.GetLength(1);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
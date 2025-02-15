using System;
using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using NUnit.Framework;
using UnityEngine;

public class OrganizeBoardTest
{
    [Test]
    // 範囲外に配置したとき江ラガー出るか
    public void DeployTest()
    {
        var board = CreateModel(new Vector2Int(2, 2));

        board.Deploy(new Vector2Int(0, 0), CreateMember(0));
        board.Deploy(new Vector2Int(1, 1), CreateMember(0));

        Assert.That(() => Deploy(new Vector2Int(-1, -1)), Throws.InstanceOf<Exception>());
        Assert.That(() => Deploy(new Vector2Int(2, 2)), Throws.InstanceOf<Exception>());        
        
        void Deploy(Vector2Int gridPos)
        {
            board.Deploy(gridPos, CreateMember(0));
        }
    }

    [Test]
    public void IsValidGridPosTest()
    {
        var board = CreateModel(new Vector2Int(2, 2));

        Assert.That(board.IsValidGridPos(new Vector2Int(0, 0)));
        Assert.That(board.IsValidGridPos(new Vector2Int(1, 1)));
        Assert.That(!board.IsValidGridPos(new Vector2Int(-1, -1)));
        Assert.That(!board.IsValidGridPos(new Vector2Int(2, 2)));
    }

    OrganizeMember CreateMember(int masterID)
    {
        return new OrganizeMember(masterID, BattlerStrategy.Assassination);
    }

    OrganizeBoard CreateModel(Vector2Int gridPos)
    {
        var board = new OrganizeBoard(gridPos);

        return board;
    }
}

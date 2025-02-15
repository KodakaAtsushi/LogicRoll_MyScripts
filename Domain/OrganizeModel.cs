using LogicRoll.Domain.Battle;
using LogicRoll.Domain.Entity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LogicRoll.Domain.Organize
{
    /// <summary>
    /// BattlerOrganizeDataの編集用Model
    /// 変更に必要なメソッドを提供する。
    /// 変更する際のルール（制限など）を持つ。
    /// 
    /// ※メンバーIDは、削除されない限り変更されない。(詰めない）
    /// 　0,1,2の3つのIDがある場合、0を削除しても1,2のIDは変わらず、0は空きになる。
    /// </summary>
    public class OrganizeModel
    {
        public const int MAX_MEMBER_COUNT = 3;

        #region 変数
        /// <summary>
        /// 編成スロットのID
        /// </summary>
        public int slotID { get; private set; }

        /// <summary>
        /// 編成名
        /// </summary>
        public string organizeName { get; set; }

        /// <summary>
        /// 編成メンバーリスト
        /// </summary>
        List<OrganizeMemberEntity> memberList;

        /// <summary>
        /// 編成メンバーの初期位置
        /// </summary>
        List<OrganizeStartPosition> startPositions;

        public OrganizeModel(OrganizeEntity organizeEntity)
        {
            if (organizeEntity == null)
            {
                this.slotID = 0;
                this.organizeName = "New Team";
                this.memberList = new List<OrganizeMemberEntity>();
                this.startPositions = new List<OrganizeStartPosition>();
                return;
            }
            else
            {
                var entity = organizeEntity;
                this.slotID = entity.SlotID;
                this.organizeName = entity.TeamName;
                this.memberList = entity.Members.ToList();
                this.startPositions = entity.StartPositions.ToList();
            }
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 編成にメンバーを追加
        /// </summary>
        /// <param name="mstID"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public (AddMemberResult reason, int memID) AddMember(int mstID, BattlerStrategy strategy)
        {
            if (memberList.Count >= MAX_MEMBER_COUNT)
            {
                return (AddMemberResult.MAX_MEMBER_COUNT_OVER, -1);
            }

            //TODO:後でマスターIDではなく、キャラクターIDで判定するように変更
            if (memberList.Exists(mem => mem.MasterID == mstID))
            {
                return (AddMemberResult.SAME_CHARACTER_EXIST, -1);
            }

            int memID = GetEmptyMemberID();
            memberList.Add(new OrganizeMemberEntity(memID, mstID, strategy));
            return (AddMemberResult.SUCCESS, memID);
        }

        public static string GetAddMemberResultString(AddMemberResult result)
        {
            switch (result)
            {
                case AddMemberResult.SUCCESS:
                    return "Add Member Success";
                case AddMemberResult.MAX_MEMBER_COUNT_OVER:
                    return "これ以上配置できません";
                case AddMemberResult.SAME_CHARACTER_EXIST:
                    return "同じキャラクターが配置されています";
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// 配置ポジションデータをリストに追加
        /// 編成と配置が分離する可能性を考慮して分けておく
        /// </summary>
        /// <param name="memID"></param>
        /// <param name="gridPos"></param>
        public void AddDeployPosition(int memID, Vector2Int gridPos)
        {
            var posData = startPositions.Find(pos => pos.GridPos == gridPos);
            if (posData != null)
            {
                posData.memberID = memID;
            }
            else
            {
                startPositions.Add(new OrganizeStartPosition(memID, gridPos));
            }
        }

        /// <summary>
        /// 編成からメンバーを削除
        /// </summary>
        /// <param name="memID"></param>
        /// <returns></returns>
        public RemoveMemberResult RemoveMember(int memID)
        {
            var index = memberList.FindIndex(data => data.MemberID == memID);
            if (index == -1)
            {
                return RemoveMemberResult.NOT_EXIST_MEM_ID;
            }

            memberList.RemoveAt(index);
            return RemoveMemberResult.SUCCESS;
        }

        /// <summary>
        /// 編成メンバーの情報を取得
        /// </summary>
        /// <param name="memID"></param>
        /// <returns></returns>
        public OrganizeMemberEntity GetBattlerEntity(int memID)
        {
            foreach(var data in memberList)
            {
                if (data.MemberID == memID)
                {
                    return data;
                }
            }
            return null;
        }

        /// <summary>
        /// メンバーリスト取得
        /// </summary>
        /// <returns></returns>
        public List<OrganizeMemberEntity> GetMemberList()
        {
            return memberList;
        }

        /// <summary>
        /// 保存用のデータを作成
        /// </summary>
        /// <returns></returns>
        public OrganizeEntity ToEntity()
        {
            var memberData = memberList.ToArray();
            var startPositions = this.startPositions.ToArray();
            return new OrganizeEntity(slotID, organizeName, memberData, startPositions);
        }

        /// <summary>
        /// セーブスロット変更
        /// </summary>
        /// <param name="slotID"></param>
        public void ChangeSlotID(int slotID)
        {
            this.slotID = slotID;
        }

        /// <summary>
        /// チーム名変更
        /// </summary>
        /// <param name="name"></param>
        public void ChangeTeamName(string name)
        {
            this.organizeName = name;
        }

        public OrganizeMemberEntity GetMemberByPosition(Vector2Int gridPos)
        {
            var posData = startPositions.Find(pos => pos.GridPos == gridPos);
            if (posData == null)
            {
                return null;
            }

            return memberList.Find(mem => mem.MemberID == posData.memberID);
        }

        public OrganizeStartPosition GetStartPositionByMemberID(int memID)
        {
            return startPositions.Find(pos => pos.memberID == memID);
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var log = $"[Log OrganizeModel] slotID:{slotID}, organizeName:{organizeName}\n";
            foreach (var data in memberList)
            {
                var posData = startPositions.Find(pos => pos.memberID == data.MemberID);
                log += $"{data}  POS : {posData?.GridPos}\n";
            }
            return log;
        }

        /// <summary>
        /// 空きメンバーIDを取得
        /// </summary>
        /// <returns></returns>
        private int GetEmptyMemberID()
        {
            for (int i = 0; i < MAX_MEMBER_COUNT; i++)
            {
                if (memberList.Exists(data => data.MemberID == i))
                {
                    continue;
                }
                return i;
            }
            return -1;
        }
        #endregion
    }
}

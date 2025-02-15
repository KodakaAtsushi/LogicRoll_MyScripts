using System;
using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using R3;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class MemberInfoView : MonoBehaviour
    {
        // status
        [SerializeField] SkeletonGraphic battlerGraphic;
        [SerializeField] TextMeshProUGUI battlerName;
        [SerializeField] TextMeshProUGUI hp;
        [SerializeField] TextMeshProUGUI atk;
        [SerializeField] TextMeshProUGUI spd;
        [SerializeField] TextMeshProUGUI serch;

        [SerializeField] GameObject infoPanel;
        [SerializeField] GameObject nonePanel;

        // strategy button
        [SerializeField] Button strategyButton;
        [SerializeField] TextMeshProUGUI strategyName;

        // active skill
        [SerializeField] TextMeshProUGUI activeSkillName;
        [SerializeField] TextMeshProUGUI activeSkillDetails;
        
        // passive skill
        [SerializeField] TextMeshProUGUI passiveSkillName;
        [SerializeField] TextMeshProUGUI passiveSkillDetails;

        public Observable<Unit> OnClick => strategyButton.OnClickAsObservable();
        public bool isActive { get; private set; } = false;

        public void Init()
        {
            SetActive(false);
        }

        public void UpdateInfo(int masterID, string strategyName)
        {
            var data = ServiceLocator.Get<IBattlerMasterDatabase>().GetBattlerDataByID(masterID);
            
            // battler image
            var path = PathService.GetCharacterSpinePath(masterID);
            var asset = Resources.Load<SkeletonDataAsset>(path);
            battlerGraphic.skeletonDataAsset = asset;
            battlerGraphic.Initialize(true);

            // status
            // battlerName.text = data.Name; 日本語に対応次第実装
            hp.text = $"HP : {data.HP}";
            atk.text = $"ATK : {data.ATK}";
            spd.text = $"SPEED : {data.SPEED}";
            serch.text = $"SERCH : {data.ACTION_RANGE}";

            // strategy button
            this.strategyName.text = strategyName;

            // active skill
            // masterに追加され次第実装

            // passive skill
            // masterに追加され次第実装
        }

        public void SetActive(bool isActive)
        {
            infoPanel.SetActive(isActive);
            nonePanel.SetActive(!isActive);

            this.isActive = isActive;
        }
    }
}

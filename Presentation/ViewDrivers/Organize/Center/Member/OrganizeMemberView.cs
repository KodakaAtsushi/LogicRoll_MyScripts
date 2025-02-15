using LogicRoll.Application;
using LogicRoll.Domain;
using Spine.Unity;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class OrganizeBattlerView : MonoBehaviour
    {
        SkeletonGraphic backGroundBattler;
        SkeletonGraphic dragBattler;
        public Draggable Draggable { get; private set; }

        public void Init(int masterID)
        {
            backGroundBattler = transform.GetChild(0).GetComponentInChildren<SkeletonGraphic>();
            dragBattler = transform.GetChild(1).GetComponentInChildren<SkeletonGraphic>();
            Draggable = GetComponentInChildren<Draggable>();
            Draggable.Init(masterID);

            // asset取得
            var path = PathService.GetCharacterSpinePath(masterID);
            var asset = Resources.Load<SkeletonDataAsset>(path);

            backGroundBattler.skeletonDataAsset = asset;
            backGroundBattler.Initialize(true);
            backGroundBattler.color = new Color(0.5f, 0.5f, 0.5f); // 背景グレーアウト

            dragBattler.skeletonDataAsset = asset;
            dragBattler.Initialize(true);
        }
        
        public void SetDraggingImage()
        {
            // Drag中のアイコンをセット
        }

        public void SetDefaultImage()
        {
            // 通常時のアイコンをセット
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
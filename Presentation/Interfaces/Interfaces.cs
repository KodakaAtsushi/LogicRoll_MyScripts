using System;
using LogicRoll.Application;
using LogicRoll.Domain.Battle;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public interface IFilterToggle
    {
        Toggle Toggle { get; }
        Observable<bool> OnClick { get; }
    }

    public interface IFilterableByJob
    {
        BattlerJob Job { get; }

        void Show();
        void Hide();
    }

    // drag and drop, deploy
    public interface IDraggable
    {
        int MasterID { get; }
        Transform transform { get; }

        Observable<IDraggable> OnBeginDrag { get; }
        Observable<IDraggable> OnDrag { get; }
        Observable<IDraggable> OnEndDrag { get; }
    }

    public interface IDeployable
    {
        int MasterID { get; }
        void ResetParent();
    }

    public interface IDroppable
    {
        Transform transform { get; }
    }

    public interface IDropPoint
    {
        Vector2Int GridPos { get; }

        void Droped(IDroppable draggable);
    }
}
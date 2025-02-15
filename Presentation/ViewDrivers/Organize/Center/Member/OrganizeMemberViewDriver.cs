using System.Collections.Generic;
using R3;
using LogicRoll.Domain.Battle;
using LogicRoll.Domain;
using LogicRoll.Application;

namespace LogicRoll.Presentation
{
    public class OrganizeMemberViewDriver : IFilterableByJob, IOrganizeMemberViewDriver
    {
        readonly OrganizeBattlerView view;
        readonly int masterID;
        public BattlerJob Job { get; private set; }

        // events

        public Observable<int> OnSelect => view.Draggable.OnPointerDown.Select(_=>masterID);

        public OrganizeMemberViewDriver(OrganizeBattlerView view, int masterID)
        {
            view.Init(masterID);
            this.view = view;
            this.masterID = masterID;
            
            Job = ServiceLocator.Get<IBattlerMasterDatabase>().GetBattlerDataByID(masterID).Job;

            view.Draggable.OnBeginDrag.Subscribe(d => BeginDragHandler(view)).AddTo(view);
            view.Draggable.OnEndDrag.Subscribe(d => EndDragHandler(view)).AddTo(view);
        }
        
        // draggable handlers
        void BeginDragHandler(OrganizeBattlerView view)
        {
            view.SetDraggingImage();
        }

        void EndDragHandler(OrganizeBattlerView view)
        {
            view.SetDefaultImage();
        }

        void IFilterableByJob.Show()
        {
            view.SetActive(true);
        }

        void IFilterableByJob.Hide()
        {
            view.SetActive(false);
        }
    }
}
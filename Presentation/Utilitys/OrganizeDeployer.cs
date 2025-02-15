using System.Collections.Generic;
using System.Linq;
using LogicRoll.Application;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class MemberDeployer : IMemberDeployer
    {
        readonly IEnumerable<IDeployable> deployables;
        readonly IEnumerable<IDropPoint> dropPoints;

        public MemberDeployer(IEnumerable<IDeployable> deployables, IEnumerable<IDropPoint> dropPoints)
        {
            this.deployables = deployables;
            this.dropPoints = dropPoints;
        }

        void IMemberDeployer.Drop(int masterID, Vector2 gridPos)
        {
            var deployable = deployables.First(d => d.MasterID == masterID);
            var dropPoint = dropPoints.First(dP => dP.GridPos == gridPos);

            dropPoint.Droped((IDroppable)deployable);
        }

        void IMemberDeployer.Return(int masterID)
        {
            var deployable = deployables.First(d => d.MasterID == masterID);

            deployable.ResetParent();
        }
    }
}

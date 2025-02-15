using LogicRoll.Application;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class OrganizeCellFactory : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] Transform parent;

        [SerializeField] Vector2Int maxBoardSize = new Vector2Int(1050, 610);

        public (OrganizeCellViewDriver[], IDropPoint[]) Create(Vector2Int cellCount)
        {
            // cell grid 初期化
            var grid = InitGrid(cellCount);
            prefab.GetComponent<RectTransform>().sizeDelta = grid.cellSize;

            var xCount = cellCount.x;
            var yCount = cellCount.y;

            var count = xCount * yCount;

            var views = new OrganizeCellViewDriver[count];
            var dropPoints = new IDropPoint[count];

            for(int y = 0; y < yCount; y++)
            {
                for(int x = 0; x < xCount; x++)
                {
                    var instance = Instantiate(prefab, parent);

                    var view = instance.GetComponent<OrganizeCellView>();

                    var viewDriver = new OrganizeCellViewDriver(view, new Vector2Int(x, y));

                    views[y*xCount + x] = viewDriver;
                    dropPoints[y*xCount + x] = instance.GetComponent<IDropPoint>();
                }
            }

            return (views, dropPoints);
        }

        GridLayoutGroup InitGrid(Vector2Int cellCount)
        {
            var grid = parent.GetComponent<GridLayoutGroup>();
            var space = grid.spacing;

            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = cellCount.y;

            int requiredCellWidth = (int)((maxBoardSize.x - (cellCount.x-1)*space.x) / cellCount.x);
            int requiredCellHeight = (int)((maxBoardSize.y - (cellCount.y-1)*space.y) / cellCount.y);

            int cellSizeX = Mathf.Min(requiredCellWidth, (int)grid.cellSize.x);
            int cellSizeY = Mathf.Min(requiredCellHeight, (int)grid.cellSize.y);

            grid.cellSize = new Vector2(cellSizeX, cellSizeY);

            return grid;
        }
    }
}

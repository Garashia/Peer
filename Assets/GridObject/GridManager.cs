using System.Collections.Generic;


// using UnityEditor;
using UnityEngine;
// using UnityEditor;
public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_gridScale = Vector2.one;
    public Vector2 GridScale
    {
        get { return m_gridScale; }
        set { m_gridScale = value; }
    }

    [SerializeField]
    private List<GridObject> m_grids = new List<GridObject>();

    public List<GridObject> Grids
    {
        get { return m_grids; }
        set { m_grids = value; }
    }

    [SerializeField]
    private MazeTable m_mazeTableObject = null;
    public MazeTable MazeObject
    {
        get { return m_mazeTableObject; }
        set { m_mazeTableObject = value; }
    }

    public bool IsAdjacent(Vector2Int point, Vector2Int direct)
    {
        Vector2Int vector2Int = point + direct;
        foreach (GridObject obj in m_grids)
        {
            if (obj.GridPoint == vector2Int)
            {
                return true;
            }
        }
        return false;

    }

    public GridObject GetGridObject(Vector2Int point, Vector2Int direct)
    {
        Vector2Int vector2Int = point + direct;
        int count = m_grids.Count;
        for (int i = 0; i < count; ++i)
        {
            //Debug.Log(m_grids[i].GridPoint.ToString() +
            //    vector2Int.ToString() + (m_grids[i].GridPoint == vector2Int).ToString());

            if (m_grids[i].GridPoint == vector2Int)
            {
                //Debug.Log(i);
                return m_grids[i];
            }
        }
        return null;

    }

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int[] inter = new Vector2Int[4] { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        int count = m_grids.Count;
        for (int i = 0; i < count; ++i)
        {
            GridObject obj = m_grids[i];
            GridObject right = GetGridObject(obj.GridPoint, Vector2Int.right);
            GridObject left = GetGridObject(obj.GridPoint, Vector2Int.left);
            GridObject up = GetGridObject(obj.GridPoint, Vector2Int.up);
            GridObject down = GetGridObject(obj.GridPoint, Vector2Int.down);

            // Debug.Log(i);


            if (right != null && obj.A_Grid.Right == null)
            {
                Debug.Log(right);
                obj.A_Grid.Right = right;
                right.A_Grid.Left = obj;
            }
            if (left != null && obj.A_Grid.Left == null)
            {
                Debug.Log(left);

                obj.A_Grid.Left = left;
                left.A_Grid.Right = obj;
            }
            if (up != null && obj.A_Grid.Front == null)
            {
                Debug.Log(up);
                obj.A_Grid.Front = up;
                up.A_Grid.Back = obj;
            }
            if (down != null && obj.A_Grid.Back == null)
            {
                Debug.Log(down);

                obj.A_Grid.Back = down;
                down.A_Grid.Front = obj;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyedObject(GameObject obj)
    {
        int count = m_grids.Count;
        for (int i = 0; i < count; ++i)
        {
            if (m_grids[i].gameObject.GetInstanceID() == obj.GetInstanceID())
            {
                DestroyImmediate(m_grids[i].gameObject.gameObject);
                m_grids.RemoveAt(i);
                break;
            }
        }
    }
}


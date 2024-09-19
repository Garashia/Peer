using System.Collections.Generic;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;      //!< �f�v���C����Editor�X�N���v�g������ƃG���[�ɂȂ�̂� UNITY_EDITOR �Ŋ����ĂˁI
#endif // UNITY_EDITOR



#if UNITY_EDITOR
[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private GridManager obj;
    private bool isString = false;

    private void OnEnable()
    {
        // �L���ɂȂ������ɑΏۂ��m�ۂ��Ă���
        obj = target as GridManager;
    }

    public override void OnInspectorGUI()
    {
        // ��ʍ\���Ə���
        // �W���I�ȕ\�����������ꍇ�́Abase���Ăяo��
        // base.OnInspectorGUI();
        bool flag = false;
        bool flag2 = false;
        EditorGUI.BeginChangeCheck();
        {
            isString = EditorGUILayout.Toggle("DebugString", isString);
        }
        if (EditorGUI.EndChangeCheck())
        {
            flag2 = true;
        }
        EditorGUI.BeginChangeCheck();
        {
            EditorGUILayout.BeginHorizontal();
            {
                obj.GridScale = EditorGUILayout.Vector2Field("Scale:", obj.GridScale);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (EditorGUI.EndChangeCheck())
        {
            flag = true;
        }
        List<GridObject> grids = obj.Grids;
        int count = grids.Count;

        if (GUILayout.Button("Delete Grid"))
        {
            GridDestroy(grids, count);
        }

        count = grids.Count;
        if (count != 0)
        {

            for (int i = 0; i < count; ++i)
            {
                GridObject grid = grids[i];
                if (grid == null) continue;
                Transform transform = grid.transform;
                Vector2Int point = grid.GridPoint;
                if (flag2)
                    grid.GridRender = isString;
                grid.SetGridManager = obj;

                if (grid.Open = EditorGUILayout.Foldout(grid.Open, "grid" + (i + 1).ToString()))
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        transform.localPosition = EditorGUILayout.Vector3Field("position:", transform.localPosition);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        transform.localEulerAngles = EditorGUILayout.Vector3Field("rotation:", transform.localEulerAngles);
                    }
                    EditorGUILayout.EndHorizontal();
                    Vector2Int rightPoint = point + Vector2Int.right;
                    EditorGUILayout.BeginHorizontal();
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.right)))
                    {
                        if (GUILayout.Button("Right"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.right,
                                Vector3.right,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.left)))
                    {
                        if (GUILayout.Button("Left"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.left,
                                Vector3.left,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.up)))
                    {
                        if (GUILayout.Button("Front"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.up,
                                Vector3.forward,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.down)))
                    {
                        if (GUILayout.Button("Back"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.down,
                                Vector3.back,
                                transform
                                );
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("Destroy"))
                    {
                        DestroyImmediate(grid.gameObject);
                        grids.RemoveAt(i); //���X�g�̍폜
                        break;
                    }
                    if (GUILayout.Button("GridDown"))
                    {
                        grid.GridDown();
                    }
                }

                if (flag)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = obj.GridScale.x;
                    scale.z = obj.GridScale.y;
                    transform.localScale = scale;
                }

            }
            if (GUILayout.Button("GridAllDown"))
            {
                for (int i = 0; i < count; ++i)
                {
                    GridObject grid = grids[i];
                    grid.GridDown();
                }
            }

        }
        else
        {
            if (GUILayout.Button("Create"))
            {
                GameObject games = new GameObject();
                games.transform.position = Vector3.zero;
                Vector3 scale = games.transform.localScale;
                scale.x = obj.GridScale.x;
                scale.z = obj.GridScale.y;
                games.transform.localScale = scale;
                GridObject newGrid = games.AddComponent<GridObject>();
                grids.Add(newGrid);
                games.transform.parent = obj.transform;
                newGrid.GridPoint = Vector2Int.zero;
                newGrid.SetGridManager = obj;
            }
        }
        obj.MazeObject = EditorGUILayout.ObjectField("Maze", obj.MazeObject, typeof(MazeTable), true) as MazeTable;
        if (obj.MazeObject != null)
        {
            if (GUILayout.Button("SpawnMaze"))
            {
                GridDestroy(grids);
                var mazeList = obj.MazeObject.MazeList;
                if (mazeList != null)
                    foreach (Vector2Int maze in mazeList)
                    {
                        GameObject games = new GameObject();

                        Vector3 scale = games.transform.localScale;
                        scale.x = obj.GridScale.x;
                        scale.z = obj.GridScale.y;
                        games.transform.localScale = scale;
                        games.transform.localPosition =
                            new((float)(maze.x) * games.transform.localScale.x * 2.0f,
                            0.0f,
                           (float)(maze.y) * games.transform.localScale.z * 2.0f);
                        GridObject newGrid = games.AddComponent<GridObject>();
                        grids.Add(newGrid);
                        games.transform.parent = obj.transform;
                        newGrid.GridPoint = maze;
                        newGrid.SetGridManager = obj;
                    }
            }
        }

    }

    private void GridDestroy(List<GridObject> grids)
    {
        int count = grids.Count;

        if (count != 0)
        {
            foreach (GridObject grid in grids)
            {
                if (grid != null)
                    DestroyImmediate(grid.gameObject);
            }
        }
        // �������Ɏ��s����������
        grids.Clear();

    }

    private void GridDestroy(List<GridObject> grids, int count)
    {
        if (count != 0)
        {
            foreach (GridObject grid in grids)
            {
                if (grid != null)
                    DestroyImmediate(grid.gameObject);
            }
        }
        // �������Ɏ��s����������
        grids.Clear();

    }


    private void AddObject
    (
        List<GridObject> list,
        Vector2Int point,
        Vector2Int direct2,
        Vector3 direct3,
        Transform transform)
    {
        GameObject games = new GameObject();
        Vector3 position = transform.localPosition;
        Vector3 moving = new Vector3(
            direct3.x * transform.localScale.x,
            0.0f,
            direct3.z * transform.localScale.z
            );
        position += transform.rotation * (moving * 2.0f);

        games.transform.localPosition = position;
        games.transform.rotation = transform.rotation;
        GridObject newGrid = games.AddComponent<GridObject>();
        list.Add(newGrid);
        games.transform.localScale = transform.localScale;
        games.transform.parent = obj.transform;
        newGrid.GridPoint = point + direct2;
        newGrid.SetGridManager = obj;
    }

}
#endif // UNITY_EDITOR
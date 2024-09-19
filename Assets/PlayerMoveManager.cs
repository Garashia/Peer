using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    private const int ANGLE_MAX_INDEX = 4;
    private readonly float[] CAMERA_ANGLE = new float[ANGLE_MAX_INDEX]
        {0.0f, 90.0f, 180.0f, 270.0f};

    [SerializeField]
    private GridManager m_gridManager;
    private GridObject m_object;

    [SerializeField]
    private float m_higher = 1.0f;
    private Quaternion m_firstAngle;

    enum DIRECT : uint
    {
        FRONT = 0,
        RIGHT = 1,
        BACK = 2,
        LEFT = 3,
    }
    private int m_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_object = m_gridManager.Grids[0];
        transform.rotation = m_object.Rotation;
        //m_firstAngle = transform.rotation;
        transform.position = m_object.transform.position + m_object.Rotation * (Vector3.up * m_higher);
        m_index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector3 =/* transform.localEulerAngles*/Vector3.zero;

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_index++;
            m_index %= ANGLE_MAX_INDEX;
            vector3.y = CAMERA_ANGLE[m_index];
            transform.rotation = m_object.Rotation * Quaternion.AngleAxis
            (vector3.y, Vector3.up);
            //vector3.y += m_firstAngle;

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            m_index = ANGLE_MAX_INDEX + m_index - 1;
            m_index %= ANGLE_MAX_INDEX;
            vector3.y = CAMERA_ANGLE[m_index];
            transform.rotation = m_object.Rotation * Quaternion.AngleAxis
            (vector3.y, Vector3.up);

            //vector3.y += m_firstAngle;

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GridObject obj = null;
            if (m_index == (int)DIRECT.FRONT)
            {
                obj = m_object.A_Grid.Front;
            }
            else if (m_index == (int)DIRECT.BACK)
            {
                obj = m_object.A_Grid.Back;
            }
            else if (m_index == (int)DIRECT.RIGHT)
            {
                obj = m_object.A_Grid.Right;
            }
            else if (m_index == (int)DIRECT.LEFT)
            {
                obj = m_object.A_Grid.Left;
            }
            if (obj != null)
            {
                m_object = obj;
                //transform.rotation = m_object.Rotation;
                //m_firstAngle = transform.localEulerAngles.y;
                transform.position = m_object.transform.position + m_object.Rotation * (Vector3.up * m_higher);
                // vector3 = transform.localEulerAngles;
                vector3.y = CAMERA_ANGLE[m_index];
                transform.rotation = m_object.Rotation * Quaternion.AngleAxis
                (vector3.y, Vector3.up);

                //vector3.y += m_firstAngle;
            }
            //Debug.Log(obj);

        }



    }
}

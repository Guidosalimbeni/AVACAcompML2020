using UnityEngine;

public class ColliderBounds : MonoBehaviour
{
    private Collider m_Collider;
    public Vector3 m_Center { get; set; }
    public Vector3 m_Size { get; set; }

    Vector3 m_Min; // not used
    Vector3 m_Max; // not used

    private void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<Collider>();
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;
        
        //Output this data into the console
        //OutputData();
    }

    private void OutputData() // not used only for debug
    {
        //Output to the console the center and size of the Collider volume
        Debug.Log("Collider Center : " + m_Center);
        Debug.Log("Collider Size : " + m_Size);
        Debug.Log("Collider bound Minimum : " + m_Min);
        Debug.Log("Collider bound Maximum : " + m_Max);
    }

    public Vector3 GetCenterOfMass()
    {
        return m_Center;
    }

    public Vector3 GetVectorSizeOfVolumeBounds()
    {
        return m_Size;
    }
}
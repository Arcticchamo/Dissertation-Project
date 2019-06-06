using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    [SerializeField] Vector3 m_startingPos;
    [SerializeField] Vector3 m_feedingPos;
    [SerializeField] Vector3 m_waterPos;
    [SerializeField] Vector3 m_cleaningPos;
    [SerializeField] Vector3 m_interactionPos;

    bool m_inter;
    bool m_clean;
    bool m_feed;
    bool m_water;

    Transform m_transform;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_startingPos = m_transform.position;
    }

    void Update()
    {
    
    }

    public void Feeding(bool _f)
    {
        m_feed = _f;

        if (m_feed == true)
        {
            m_transform.position = m_feedingPos;
        }
        else
        {
            m_transform.position = m_startingPos;
        }
    }

    public void Watering(bool _w)
    {
        m_water = _w;

        if (m_water == true)
        {
            m_transform.position = m_waterPos;
        }
        else
        {
            m_transform.position = m_startingPos;
        }
    }

    public void Cleaning(bool _c)
    {
        m_clean = _c;

        if (m_clean == true)
        {
            m_transform.position = m_cleaningPos;
        }
        else
        {
            m_transform.position = m_startingPos;
        }
    }

    public void Interact(bool _i)
    {
        m_inter = _i;

        if (m_inter == true)
        {
            m_transform.position = m_interactionPos;
        }
        else
        {
            m_transform.position = m_startingPos;
        }
    }
}

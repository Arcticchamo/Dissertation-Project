using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public AnimatorTestScript m_animator;
    private Transform m_transform;
    private Rigidbody m_rigidbody;
    private float m_yPos;
    public float m_movSpeed;
    public float m_rotSpeed;
    

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_yPos = m_transform.position.y;

        m_rigidbody.isKinematic = false;
        m_rigidbody.detectCollisions = true;
    }

    public void MovePlayer(Vector3 _pos)
    {
        float Movement = m_movSpeed * Time.deltaTime;

		//Movement
		Vector3 movetowards = Vector3.MoveTowards(m_transform.position, _pos, Movement);
        movetowards.y = 0.0f;
		m_rigidbody.MovePosition(movetowards);
        //m_transform.position = new Vector3 (m_transform.position.x, m_yPos, m_transform.position.z);
    }

    public bool RotatePlayer(float _rotation)
    {
        float Rotation = m_rotSpeed * Time.deltaTime;

        if (System.Math.Round(m_transform.rotation.eulerAngles.y, 0) == System.Math.Round(_rotation, 0))
        {
            //if _rotation is equal to the current rotation
            return true;
        }

        if (m_transform.rotation.eulerAngles.y < _rotation)
        {
            m_animator.SetRotate(1);
            Vector3 rot = new Vector3(0.0f, Rotation, 0.0f);
            m_transform.Rotate(rot);
        }
        else if (m_transform.rotation.eulerAngles.y > _rotation)
        {
            m_animator.SetRotate(2);
            Vector3 rot = new Vector3(0.0f, -Rotation, 0.0f);
            m_transform.Rotate(rot);
        }

        if (System.Math.Round(m_transform.rotation.eulerAngles.y, 0) == System.Math.Round(_rotation, 0))
        {
            //if _rotation is equal to the current rotation
            return true;
        }
        else
        {
            return false;
        }
    }
}

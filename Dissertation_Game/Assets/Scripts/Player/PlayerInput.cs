using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    //[SerializeField]
    //private AnimatorTestScript m_animator;

    private bool m_move_To_Food, m_move_To_Water, m_move_To_Clean;
    private bool m_interacting_With_Food, m_interacting_With_Water, m_interacting_With_Clean;
    private bool m_actionHit;

    [SerializeField] 
    private PlayerMovement m_movement;

    [SerializeField] 
    private Transform m_foodObject, m_waterObject, m_cleanObject;

    void Start()
    {
        m_move_To_Food = 
        m_move_To_Clean = 
        m_move_To_Water = false;

        m_interacting_With_Clean = 
        m_interacting_With_Food = 
        m_interacting_With_Water = false;

        m_actionHit = false;

        m_movement = GetComponent<PlayerMovement>();
        m_foodObject = GameObject.Find("Food").GetComponent<Transform>();
        m_waterObject = GameObject.Find("Water").GetComponent<Transform>();
        m_cleanObject = GameObject.Find("Clean").GetComponent<Transform>();
    }

    void Update()
    { 
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "Food" && !m_interacting_With_Food)
                {
                    Debug.Log("Player Clicked Food");

                    m_move_To_Food = true;
                    m_move_To_Water = m_move_To_Clean = false;
                    m_actionHit = false;
                }
                else if (hit.collider.name == "Water" && !m_interacting_With_Water)
                {
                    Debug.Log("Player Clicked Water");

                    m_move_To_Water = true;
                    m_move_To_Food = m_move_To_Clean = false;
                    m_actionHit = false;
                }
                else if (hit.collider.name == "Clean" && !m_interacting_With_Clean)
                {
                    Debug.Log("Player Clicked Clean");

                    m_move_To_Clean = true;
                    m_move_To_Food = m_move_To_Water = false;
                    m_actionHit = false;
                }
            }
        }

        if (m_move_To_Food)
        {
            m_movement.MovePlayer(m_foodObject.position);
            //m_animator.SetWalking(true);
        }
        else if (m_move_To_Water)
        {
            m_movement.MovePlayer(m_waterObject.position);
            //m_animator.SetWalking(true);
        }
        else if (m_move_To_Clean)
        {
            m_movement.MovePlayer(m_cleanObject.position);
            //m_animator.SetWalking(true);
        }
        else if (m_actionHit)
        {
            
        }
        //else 
            //m_animator.SetWalking(false); 
    }

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.name == "Food")
        {
            m_move_To_Food = false;
            m_interacting_With_Food = true;
            m_interacting_With_Clean = m_interacting_With_Water = false;
            m_actionHit = true;
        }
        else if (_col.gameObject.name == "Water")
        {
            m_move_To_Water = false;
            m_interacting_With_Water = true;
            m_interacting_With_Food = m_interacting_With_Clean = false;
            m_actionHit = true;
        }
        else if (_col.gameObject.name == "Clean")
        {
            m_move_To_Clean = false;
            m_interacting_With_Clean = true;
            m_interacting_With_Food = m_interacting_With_Water = false;
            m_actionHit = true;
        }
    }

    void OnCollisionExit()
    {
        m_interacting_With_Food = false;
        m_interacting_With_Water = false;
        m_interacting_With_Clean = false;
    }



}

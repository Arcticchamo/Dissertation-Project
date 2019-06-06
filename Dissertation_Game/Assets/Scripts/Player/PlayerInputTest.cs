using UnityEngine;
using System.Collections;

public class PlayerInputTest : MonoBehaviour {

    public Transform[] m_nodes;
    public bool[] m_nodebool;
    public PlayerMovement m_movement;
    public PAD m_pad;
    public AnimatorTestScript m_animator;
    private Transform m_transform;

    private bool m_food, m_notFood, m_water, m_notWater, m_clean, m_notClean;
    private bool m_inter = false;

    [SerializeField] float m_deTimer = 0.0f;
    [SerializeField] float m_deCounter = 0.0f;
    [SerializeField] float m_waterTimer = 0.0f;
    [SerializeField] float m_foodTimer = 0.0f;
    [SerializeField] float m_cleanTimer = 0.0f;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_movement = GetComponent<PlayerMovement>();

        m_food = 
        m_notFood = 
        m_water = 
        m_notWater = 
        m_clean =
        m_notClean = false;

        for (int i = 0; i < m_nodebool.Length; i++)
        {
            m_nodebool[i] = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "Food")
                {
                    m_food = true;
                    m_notFood = m_water = m_notWater = m_clean = m_notClean = false;

                    for (int i = 0; i < m_nodebool.Length; i++)
                    {
                        m_nodebool[i] = false;
                    }
                }
                else if (hit.collider.name == "Water")
                {
                    m_water = true;
                    m_notFood = m_food = m_notWater = m_clean = m_notClean = false;

                    for (int i = 0; i < m_nodebool.Length; i++)
                    {
                        m_nodebool[i] = false;
                    }
                }
                else if (hit.collider.name == "Clean")
                {
                    m_clean = true;
                    m_notFood = m_water = m_notWater = m_food = m_notClean = false;

                    for (int i = 0; i < m_nodebool.Length; i++)
                    {
                        m_nodebool[i] = false;
                    }
                }
                else if (hit.collider.name == this.name)
                {
                    if (m_inter == false)
                    {
                        m_inter = true;
                        m_pad.Interactions("Interaction");
                        Debug.Log("INTER");
                        m_deCounter = 0.0f;
                        StartCoroutine(InteractionBool());
                    }

                }
            }
        }

        if (m_deCounter >= m_deTimer)
        {
            m_deCounter = 0.0f;
            Debug.Log("NO INTER");
            m_pad.NoInteractions("No Interaction");
        }

        m_deCounter += Time.deltaTime;

        if (m_food) MoveToFood();
        else if (m_notFood) MoveFromFood();
        else if (m_water) MoveToWater();
        else if (m_notWater) MoveFromWater();
        else if (m_clean) MoveToClean();      
        else if (m_notClean) MoveFromClean();
    }

    void MoveToFood()
    { 
        if (m_nodebool[0] == false)
	    {
            m_animator.SetWalking(true);
		    //moving to start
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) && 
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
		    {
			    m_nodebool[0] = true;
		    }
            else 
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
		        {
			        m_nodebool[0] = true;
		        }
            }
	    }
	    else if (m_nodebool[1] == false)
	    {
		    //Link up 
            m_animator.SetWalking(true);
            if (m_nodebool[1] == false)
	        {
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[1].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[1].position.z, 2)))
		        {
			        m_nodebool[1] = true;
		        }
                else 
                {
                    m_movement.MovePlayer(m_nodes[1].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[1].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[1].position.z, 2)))
		            {
			            m_nodebool[1] = true;
		            }
                }
	         }
        }
	    else if (m_nodebool[2] == false)
	    {
		    //food bowl
            m_animator.SetWalking(true);
            if (m_nodebool[2] == false)
            {
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[2].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[2].position.z, 2)))
                {
                    m_nodebool[2] = true;
                    m_animator.SetCurrentJob(1);
                    StartCoroutine(FoodTimer());
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[2].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[2].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[2].position.z, 2)))
                    {
                        m_nodebool[2] = true;
                        m_animator.SetCurrentJob(1);
                        StartCoroutine(FoodTimer());
                    }
                }
            }
	    }
    }

    void MoveFromFood()
    { 
        if (m_nodebool[2] == true)
	    {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[2].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[2].position.z, 2)))
		    {
			    m_nodebool[2] = false;
		    }
            else 
            {
                m_movement.MovePlayer(m_nodes[2].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[2].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[2].position.z, 2)))
                {
                    m_nodebool[2] = false;
                }
            }
	    }

	    else if (m_nodebool[1] == true)
	    {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[1].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[1].position.z, 2)))
		    {
			    m_nodebool[1] = false;
		    }
            else 
            {
                m_movement.MovePlayer(m_nodes[1].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[1].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[1].position.z, 2)))
                {
                    m_nodebool[1] = false;
                }
            }
	    }

	    else if (m_nodebool[0] == true)
	    {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = false;
                m_animator.SetWalking(false);
                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = false;
                    m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                    m_animator.SetWalking(false);
                }
            }
	    }
    }

    void MoveToWater()
    {
        if (m_nodebool[0] == false)
        {
            m_animator.SetWalking(true);
            //moving to start
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = true;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = true;
                }
            }
        }

        else if (m_nodebool[3] == false)
        {
            //Link up 
            if (m_nodebool[3] == false)
            {
                m_animator.SetWalking(true);
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                {
                    m_nodebool[3] = true;
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[3].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                    {
                        m_nodebool[3] = true;
                    }
                }
            }
        }

        else if (m_nodebool[4] == false)
        {
            //food bowl
            m_animator.SetWalking(true);
            if (m_nodebool[4] == false)
            {
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[4].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[4].position.z, 2)))
                {
                    m_nodebool[4] = true;
                    m_animator.SetCurrentJob(2);
                    StartCoroutine(WaterTimer());
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[4].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[4].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[4].position.z, 2)))
                    {
                        m_nodebool[4] = true;
                        m_animator.SetCurrentJob(2);
                        StartCoroutine(WaterTimer());
                    }
                }
            }
        }
    }

    void MoveFromWater()
    {
        if (m_nodebool[4] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[4].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[4].position.z, 2)))
            {
                m_nodebool[4] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[4].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[4].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[4].position.z, 2)))
                {
                    m_nodebool[4] = false;
                }
            }
        }

        else if (m_nodebool[3] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
            {
                m_nodebool[3] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[3].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                {
                    m_nodebool[3] = false;
                }
            }
        }

        else if (m_nodebool[0] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = false;
                m_animator.SetWalking(false);
                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = false;
                    m_animator.SetWalking(false);
                    m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                }
            }
        }
    }

    void MoveToClean()
    {
        if (m_nodebool[0] == false)
        {
            //moving to start
            m_animator.SetWalking(true);
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = true;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = true;
                }
            }
        }

        else if (m_nodebool[3] == false)
        {
            //Link up 
            m_animator.SetWalking(true);
            if (m_nodebool[3] == false)
            {
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                {
                    m_nodebool[3] = true;
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[3].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                    {
                        m_nodebool[3] = true;
                    }
                }
            }
        }

        else if (m_nodebool[5] == false)
        {
            //food bowl
            m_animator.SetWalking(true);
            if (m_nodebool[5] == false)
            {
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[5].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[5].position.z, 2)))
                {
                    m_nodebool[5] = true;
                    m_animator.SetCurrentJob(3);
                    StartCoroutine(CleanTimer());
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[5].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[5].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[5].position.z, 2)))
                    {
                        m_nodebool[5] = true;
                        m_animator.SetCurrentJob(3);
                        StartCoroutine(CleanTimer());
                    }
                }
            }
        }
    }

    void MoveFromClean()
    {
        if (m_nodebool[5] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[5].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[5].position.z, 2)))
            {
                m_nodebool[5] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[5].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[5].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[5].position.z, 2)))
                {
                    m_nodebool[5] = false;
                }
            }
        }

        else if (m_nodebool[3] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
            {
                m_nodebool[3] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[3].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                {
                    m_nodebool[3] = false;
                }
            }
        }

        else if (m_nodebool[0] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = false;
                m_animator.SetWalking(false);
                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = false;
                    m_animator.SetWalking(false);
                    m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                }
            }
        }
    }

    IEnumerator WaterTimer()
    {
        Debug.Log("Timer");
        yield return new WaitForSeconds(m_waterTimer);
        Debug.Log("Timer2");
        m_animator.SetCurrentJob(0);
        m_notWater = true;
        m_notClean = m_food = m_notFood = m_clean = m_water = false;
    }

    IEnumerator FoodTimer()
    {
        Debug.Log("Timer");
        yield return new WaitForSeconds(m_foodTimer);
        Debug.Log("Timer2");
        m_animator.SetCurrentJob(0);
        m_notFood = true;
        m_notClean = m_food = m_notWater = m_clean = m_water = false;
    }

    IEnumerator CleanTimer()
    {
        Debug.Log("Timer");
        yield return new WaitForSeconds(m_cleanTimer);
        Debug.Log("Timer2");
        m_animator.SetCurrentJob(0);
        m_notClean = true;
        m_notFood = m_food = m_notWater = m_clean = m_water = false;
    }

    IEnumerator InteractionBool()
    {
        yield return new WaitForSeconds(2);
        m_inter = false;
    }
}
/*
New movement (Array of Vec 3) + (Array of bool)
Node 0 - Starting Pos 
Node 1 - Food Bowl Line Up 
Node 2 - Food Bowl  
Node 3 - Water/Clean Line Up 
Node 4 - Water 
Node 5 - Clean


To get to the food bowl
0 - 1 - 2
Then back 
2 - 1 - 0


To get to the water 
0 - 3 - 4
Then back 
4 - 3 - 0

To get to the clean
0 - 3 - 5
Then back 
5 - 3 - 0*/
using UnityEngine;
using System.Collections;

public class PlayerInputTest2 : MonoBehaviour {

    public Transform[] m_nodes;
    public bool[] m_nodebool;

    public PlayerMovement m_movement;
    public PAD m_pad;
    public AnimatorTestScript m_animator;
    private Transform m_transform;

    private bool m_food,
                 m_notFood,
                 m_water,
                 m_notWater,
                 m_clean,
                 m_notClean;

    private bool m_inter= false;
    [SerializeField] private bool m_actionHappening;
    private bool m_rotation;
    private bool m_moveTo;

    private CameraMovement m_camMovement;

    [SerializeField] float m_deTimer = 0.0f; //Max Timer
    [SerializeField] float m_deCounter = 0.0f;

    [SerializeField] float m_waterTimer = 0.0f; //Time in each Action
    [SerializeField] float m_foodTimer  = 0.0f;
    [SerializeField] float m_cleanTimer = 0.0f;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_movement = GetComponent<PlayerMovement>();
        m_camMovement = GameObject.Find("Main Camera").GetComponent<CameraMovement>();

        m_food =
        m_water =
        m_clean =
        m_notClean =
        m_notWater =
        m_notFood = false;

        m_inter = false;
        m_actionHappening = false;
        m_rotation = false;

        for (int i = 0; i < m_nodebool.Length; i++)
        {
            m_nodebool[i] = false;
        }

        //Saved current time
        m_deCounter = m_pad.GetInteractionTimer();

        //Get the number of calls missed 
        float CallsMissed = m_pad.getTotalTimeMissed() / m_deTimer;
        int totalCallsMissed = (int)CallsMissed;
        //Get the decimal value 
        float Decimal = CallsMissed - totalCallsMissed;
        
        float TimeToAdd = m_deTimer * Decimal;

        m_deCounter += TimeToAdd;

        if (m_deCounter >= m_deTimer)
        {
            totalCallsMissed++;
            m_deCounter -= m_deTimer;
        }

        for (int i = 0; i < totalCallsMissed; i++)
        {
            m_pad.LoadInInterCheck();
        }
    }
    
    void Update()
    {
        if (m_actionHappening == false) //no action in use
        {
            if (Input.GetMouseButton(0)) //Left Click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) //Check for ray collision
                {
                    if (hit.collider.name == "Food")
                    {
                        m_actionHappening = true;
                        m_food = true;
                        m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                    }
                    else if (hit.collider.name == "Water")
                    {
                        m_actionHappening = true;
                        m_water = true;
                        m_food = m_notFood = m_notWater = m_clean = m_notClean = false;
                    }
                    else if (hit.collider.name == "Clean")
                    {
                        m_actionHappening = true;
                        m_clean = true;
                        m_food = m_notFood = m_water = m_notWater = m_notClean = false;
                    }
                    else if (hit.collider.name == this.name)
                    {
                        m_actionHappening = true;
                        m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                        StartCoroutine(InteractionBool());
                    }
                }
            }
        }

        if (m_deCounter >= m_deTimer)
        {
            m_deCounter = 0.0f;
            m_pad.NoInteractions("No Interaction");
        }

        m_deCounter += Time.deltaTime;

        if (m_food) MoveToFood();
        else if (m_notFood) MoveFromFood();
        else if (m_water) MoveToWater();
        else if (m_notWater) MoveFromWater();
        else if (m_clean) MoveToClean();
        else if (m_notClean) MoveFromClean();

        //#if UNITY_ANDROID

        //if (Input.touchCount > 0)
        //{
        //    Debug.Log("TOUCH");
        //}

        //if (m_actionHappening == false) //no action in use
        //{
        //    if (Input.touchCount > 0) 
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit)) //Check for ray collision
        //        {
        //            if (hit.collider.name == "Food")
        //            {
        //                m_actionHappening = true;
        //                m_food = true;
        //                m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
        //            }
        //            else if (hit.collider.name == "Water")
        //            {
        //                m_actionHappening = true;
        //                m_water = true;
        //                m_food = m_notFood = m_notWater = m_clean = m_notClean = false;
        //            }
        //            else if (hit.collider.name == "Clean")
        //            {
        //                m_actionHappening = true;
        //                m_clean = true;
        //                m_food = m_notFood = m_water = m_notWater = m_notClean = false;
        //            }
        //            else if (hit.collider.name == this.name)
        //            {
        //                m_actionHappening = true;
        //                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
        //            }
        //        }
        //    }
        //}

        //if (m_deCounter >= m_deTimer)
        //{
        //    m_deCounter = 0.0f;
        //    m_pad.NoInteractions("No Interaction");
        //}

        //m_deCounter += Time.deltaTime;

        //if (m_food) MoveToFood();
        //else if (m_notFood) MoveFromFood();
        //else if (m_water) MoveToWater();
        //else if (m_notWater) MoveFromWater();
        //else if (m_clean) MoveToClean();
        //else if (m_notClean) MoveFromClean();

        ////#endif
    }

    void MoveToFood()
    {
        if (m_nodebool[0] == false) //if the first position is false
        {
            m_camMovement.Feeding(true);
            m_animator.SetWalking(true); //Start walking
 
            //Check if in the correct position
            if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = true;
            }
            else
            {
                //If not in the correct position
                m_movement.MovePlayer(m_nodes[0].position); 

                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[0].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[0].position.z, 2)))
                    {
                        m_nodebool[0] = true;
                    }
            }
        }

        else if(m_nodebool[1] == false)
        {
            m_animator.SetWalking(true);

            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(295.0f);
            }
            else
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[1].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[1].position.z, 2)))
                {
                    m_nodebool[1] = true;
                    m_rotation = false;
                }
                else
                {
                    //If not in the correct position
                    m_movement.MovePlayer(m_nodes[1].position);

                    if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[1].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[1].position.z, 2)))
                    {
                        m_nodebool[1] = true;
                        m_rotation = false;
                    }
                }
            }
        }

        else if (m_nodebool[1] == true && m_moveTo == false)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(270.0f);
            }
            else if (m_rotation == true)
            {
                m_moveTo = true;
                m_rotation = false;
                m_animator.SetCurrentJob(1);
                StartCoroutine(FoodTimer());
            }
        }
    }//

    void MoveFromFood()
    {
        if (m_nodebool[1] == true)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(138.0f);
            }
            else if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[1].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[1].position.z, 2)))
                {
                    m_nodebool[1] = false;
                    m_rotation = false;
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[1].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[1].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[1].position.z, 2)))
                    {
                        m_nodebool[1] = false;
                        m_rotation = false;
                    }
                }
            }
        }

        else if (m_nodebool[0] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = false;
                }
            }
            
        }

        else if (m_nodebool[0] == false)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(180.0f);
            }

            if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                m_rotation = false;
                m_animator.SetWalking(false);
                m_actionHappening = false;
                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                m_camMovement.Feeding(false);
            }
        }
    }

    void MoveToWater()
    {
        if (m_nodebool[0] == false)
        {
            m_camMovement.Watering(true);
            m_animator.SetWalking(true);

            if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[0].position.x, 2)) &&
               (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = true;
            }
            else
            {
                //If not in the correct position
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[0].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = true;
                }
            }
        }

        else if (m_nodebool[4] == false)
        {
            m_animator.SetWalking(true);

            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(101.0f);
            }
            else
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[4].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[4].position.z, 2)))
                {
                    m_nodebool[4] = true;
                    m_rotation = false;
                }
                else
                {
                    //If not in the correct position
                    m_movement.MovePlayer(m_nodes[4].position);

                    if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[4].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[4].position.z, 2)))
                    {
                        m_nodebool[4] = true;
                        m_rotation = false;
                    }
                }
            }
        }
        else if (m_nodebool[4] == true && m_moveTo == false)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(90.0f);
            }
            else if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                m_moveTo = true;
                m_rotation = false;
                m_animator.SetCurrentJob(2);
                StartCoroutine(WaterTimer());
            }
        }
    }

    void MoveFromWater()
    {
        if (m_nodebool[4] == true)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(287.0f);
            }
            else if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[4].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[4].position.z, 2)))
                {
                    m_nodebool[4] = false;
                    m_rotation = false;
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[4].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[4].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[4].position.z, 2)))
                    {
                        m_nodebool[4] = false;
                        m_rotation = false;
                    }
                }
            }
        }

        else if (m_nodebool[0] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = false;
                }
            }
        }

        else if (m_nodebool[0] == false)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(180.0f);
            }

            if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                m_rotation = false;
                m_animator.SetWalking(false);
                m_actionHappening = false;
                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                m_camMovement.Watering(false);
            }
        }
    }//

    void MoveToClean()
    {
        if (m_nodebool[0] == false)
        {
            m_camMovement.Cleaning(true);
            m_animator.SetWalking(true);

            if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = true;
            }
            else
            {
                //If not in the correct position
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[0].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = true;
                }
            }
        }

        else if (m_nodebool[2] == false)
        {
            m_animator.SetWalking(true);

            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(63.0f);
            }
            else
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[2].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[2].position.z, 2)))
                {
                    m_nodebool[2] = true;
                    m_rotation = false;
                }
                else
                {
                    //If not in the correct position
                    m_movement.MovePlayer(m_nodes[2].position);

                    if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[2].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[2].position.z, 2)))
                    {
                        m_nodebool[2] = true;
                        m_rotation = false;
                    }
                }
            }
        }

        else if (m_nodebool[3] == false)
        {
            m_animator.SetWalking(true);

            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(36.0f);
            }
            else
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[3].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[3].position.z, 2)))
                {
                    m_nodebool[3] = true;
                    m_rotation = false;
                }
                else
                {
                    //If not in the correct position
                    m_movement.MovePlayer(m_nodes[3].position);

                    if ((System.Math.Round(m_transform.position.x, 2)) == (System.Math.Round(m_nodes[3].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2)) == (System.Math.Round(m_nodes[3].position.z, 2)))
                    {
                        m_nodebool[3] = true;
                        m_rotation = false;
                    }
                }
            }
        }

        else if (m_nodebool[3] == true && m_moveTo == false)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(216.0f);
            }
            else if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                m_moveTo = true;
                m_rotation = false;
                m_animator.SetCurrentJob(3);
                StartCoroutine(CleanTimer());
            }
        }
    }//

    void MoveFromClean()
    {
        if (m_nodebool[3] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
            {
                m_nodebool[3] = false;
                m_rotation = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[3].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[3].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[3].position.z, 2)))
                {
                    m_nodebool[3] = false;
                    m_rotation = false;
                }
            }
        }

        else if (m_nodebool[2] == true)
        { 
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(228.0f);
            }
            else if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[2].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[2].position.z, 2)))
                {
                    m_nodebool[2] = false;
                    m_rotation = false;
                }
                else
                {
                    m_movement.MovePlayer(m_nodes[2].position);

                    if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[2].position.x, 2)) &&
                        (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[2].position.z, 2)))
                    {
                        m_nodebool[2] = false;
                        m_rotation = false;
                    }
                }
            }
        }

        else if (m_nodebool[0] == true)
        {
            if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
            {
                m_nodebool[0] = false;
            }
            else
            {
                m_movement.MovePlayer(m_nodes[0].position);

                if ((System.Math.Round(m_transform.position.x, 2) == System.Math.Round(m_nodes[0].position.x, 2)) &&
                    (System.Math.Round(m_transform.position.z, 2) == System.Math.Round(m_nodes[0].position.z, 2)))
                {
                    m_nodebool[0] = false;
                }
            }
        }

        else if (m_nodebool[0] == false)
        {
            if (m_rotation == false)
            {
                m_rotation = m_movement.RotatePlayer(180.0f);
            }

            if (m_rotation == true)
            {
                m_animator.SetRotate(0);
                m_rotation = false;
                m_animator.SetWalking(false);
                m_actionHappening = false;
                m_food = m_notFood = m_water = m_notWater = m_clean = m_notClean = false;
                m_camMovement.Cleaning(false);
            }
        }
    }//

    IEnumerator WaterTimer()
    {
        Debug.Log("Timer");
        m_animator.SetRotate(0);
        yield return new WaitForSeconds(m_waterTimer);
        Debug.Log("Timer2");
        m_animator.SetCurrentJob(0);
        m_notWater = true;
        m_notClean = m_food = m_notFood = m_clean = m_water = false;
    }

    IEnumerator FoodTimer()
    {
        Debug.Log("Timer");
        m_animator.SetRotate(0);
        yield return new WaitForSeconds(m_foodTimer);
        Debug.Log("Timer2");
        m_animator.SetCurrentJob(0);
        m_notFood = true;
        m_moveTo = false;
        m_notClean = m_food = m_notWater = m_clean = m_water = false;
    }

    IEnumerator CleanTimer()
    {
        Debug.Log("Timer");
        m_animator.SetRotate(0);
        yield return new WaitForSeconds(m_cleanTimer);
        Debug.Log("Timer2");
        m_animator.SetCurrentJob(0);
        m_notClean = true;
        m_notFood = m_food = m_notWater = m_clean = m_water = false;
    }

    IEnumerator InteractionBool()
    {
        m_camMovement.Interact(true);
        m_pad.Interactions("Interaction");
        m_deCounter = 0.0f;
        yield return new WaitForSeconds(2);
        m_actionHappening = false;
        m_inter = false;
        m_camMovement.Interact(false);
    }

    public float GetInterCounter()
    {
        return m_deCounter;
    }
} //


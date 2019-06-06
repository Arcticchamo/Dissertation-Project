using UnityEngine;
using System.Collections;

public class AnimatorTestScript : MonoBehaviour {

    public PAD m_pad;
    public PlayerInputTest m_playerInput;

    private enum CURRENT_EMOTION
    {
        Exuberant,
        Dependant,
        Relaxed,
        Docile,
        Bored,
        Disdainful,
        Anxious,
        Hostile,
        PassiveHappy,
        PassiveSad,
        Neutral
    }

    public Animator m_animator;

    private int m_emotion = 0;
    //Neutral = 0   PassiveHappy = 1   PassiveSad = 2
    //Exuberant = 3   Dependant = 4   Relaxed = 5
    //Docile = 6   Bored = 7   Disdainful = 8
    //Anxious = 9   Hostile = 10

    private int m_job = 0;
    //Nothing = 0   Eating = 1
    //Drinking = 2  Cleaning = 3

    private int m_rot = 0;
    //No rotation = 0
    //Right = 1
    //Left = 2

    private bool m_moving = false;
    private bool m_action = false;
    private bool m_passive = false;



    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    { 
        
    }

    /*
     *When the hamster isnt moving - 
     *a timer should run to see when the hamster 
     *should use an anmation.
     *A passive animation will run automatically throughout the
     *scene - the breathing animation
     *When the hamster is interacted with, the current
     *emotional state will play
     *When an action is pressed. the walking animation will 
     *play until it reaches the action. Then it will play the 
     *Action animation. Once the action is complete, it will 
     *return to the walking animation
     *When back at the origin spot, return to the idle animation 
     *and continue the timer
     */

    public void SetPassive(bool _passive)
    {
        m_passive = _passive;
        m_animator.SetBool("m_passive", m_passive);
    }

    public void SetAction(bool _action)
    {
        m_action = _action;
        m_animator.SetBool("m_action", m_action);
    }

    public void SetWalking(bool _walking)
    {
        m_moving = _walking;
        m_animator.SetBool("m_moving", m_moving);
    }

    public void SetCurrentEmotion(int _emotion)
    {
        m_emotion = _emotion;
        m_animator.SetInteger("m_emotion", m_emotion);
    }

    public void SetCurrentJob(int _job)
    {
        m_job = _job;
        m_animator.SetInteger("m_job", m_job);
    }

    public void SetRotate(int _rot)
    { 
        //0 = no rotation
        //1 = right 
        //2 = left

        m_rot = _rot;
        m_animator.SetInteger("Rotate", m_rot);
    }
}

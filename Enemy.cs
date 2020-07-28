using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform m_transform;
    Animator m_ani;
    UnityEngine.AI.NavMeshAgent m_agent;
    Player m_player;
    float m_movSpeed = 2.5f;
    float m_rotSpeed = 5.0f;
    float m_timer = 2;
    int m_life = 15;




    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_ani = this.GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_agent.speed = m_movSpeed;
        m_agent.SetDestination(m_player.m_transform.position);
        
    }

    // Update is called once per frame
    void Update()

    {
        if(m_player.m_life <= 0)
        return;
        m_timer -= Time.deltaTime;
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idel",false);
            if(m_timer > 0)
            return;
        if(Vector3.Distance(m_transform.position,m_player.m_transform.position) < 1.5f)
            {
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);
            }
        else
            {
                m_timer = 1;
                m_agent.SetDestination(m_player.m_transform.position);
                m_ani.SetBool("run",true);
            }
        }

        if(stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            if(m_timer<0)
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }

            if(Vector3.Distance(m_transform.position, m_player.m_transform.position) < 1.5f)
            {
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);
            }
        }

        if(stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack") && !m_ani.IsInTransition(0))
        {
            RotateTo();
            m_ani.SetBool("attack",false);
            if(stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);
                m_timer = 2;
            }
        }
    }
       


    void RotateTo()

    {
        Vector3 targetdir = m_player.m_transform.position - m_transform.position; // calculate the direction of player
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir,m_rotSpeed * Time.deltaTime, 0.0f); // set new direction to rotate to
        m_transform.rotation = Quaternion.LookRotation(newDir); //rotate enemy
    }
}
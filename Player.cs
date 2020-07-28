using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("MyGame/Player")]
public class Player : MonoBehaviour
{
    public Transform m_transform; 
    CharacterController m_ch; //角色控制组件
    float m_moveSpeed = 3.0f; //角色移动速度
    float m_gravity = 2.0f; //重力
    public int m_life = 5; //生命值
    Transform m_camTransform; // Transform of camera
    Vector3 m_camRot;
    float m_camHeight = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
     m_transform = this.transform;
     m_ch = this.GetComponent<CharacterController>(); //Get  Character Controller   
     m_camTransform = Camera.main.transform; //Get camera compenent
     m_camTransform.position = m_transform.TransformPoint(0,m_camHeight,0);
     m_camTransform.rotation = m_transform.rotation;// set rotation derectiono is the same with player
     m_camRot = m_camTransform.eulerAngles;

     Screen.lockCursor = true; // lock the mouse
    }

    // Update is called once per frame
    void Update()
    {
    if (m_life <= 0)
        return;
    Control();
    //if hp is zero do nothing
    }

    void Control()
    {
        float xm = 0, ym = 0, zm =0; // define 3 variables to control movement
        ym -= m_gravity * Time.deltaTime; // gravity movement 
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");// Get the distance of mouse movement

        m_camRot.x -= rv;
        m_camRot.y += rh; 
        m_camTransform.eulerAngles = m_camRot; // rotate cam

        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot; // set the same direction after movement of Player 


        if(Input.GetKey(KeyCode.W))
        {
            zm += m_moveSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            zm -= m_moveSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            xm -= m_moveSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            xm += m_moveSpeed * Time.deltaTime;
        }

        m_ch.Move(m_transform.TransformDirection(new Vector3(xm,ym,zm))); //use MOve function from Character Contorller
        m_camTransform.position = m_transform.TransformPoint(0,m_camHeight,0);
    }    
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }
}

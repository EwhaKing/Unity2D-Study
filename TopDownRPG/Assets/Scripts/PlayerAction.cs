using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float h;
    float v;
    public float speed;
    bool isHorizonMove;
    Vector3 dirVec; //for Ray

    Rigidbody2D rigid;
    Animator anim;
    GameObject scanObject;
    

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {   
        //Move Value
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //Check Button Down & Up
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        //Check Horizontal Move
        if(hDown || vUp) 
            isHorizonMove=true; 
        else if(vDown || hUp)
            isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = h !=0;
        
           
        //Animation
        if(anim.GetInteger("hAxisRaw")!=h){//현 애니메이션 상태랑 인풋 h랑 차이가 생기면 
            anim.SetInteger("hAxisRaw", (int)h); //현 인풋에 맞게 업데이트 
        }
        else if(anim.GetInteger("vAxisRaw")!=v){
            anim.SetInteger("vAxisRaw",(int)v);
        }
        
        //Direction
        if(vDown && v == 1)
        {
            dirVec= Vector3.up;
        }
        else if(vDown && v == -1)
        {
            dirVec = Vector3.down;
        }
        else if(hDown && h == 1)
        {
            dirVec = Vector3.right;
        }
        else if (hDown && h == -1)
        {
            dirVec = Vector3.left;
        }

        //Scan Object
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            Debug.Log("this is : " + scanObject.name);
        }
    }

    void FixedUpdate()
    {
        //Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h,0) : new Vector2(0,v);
        rigid.velocity = moveVec * speed ;

        //Ray
        Debug.DrawRay(rigid.position, dirVec*0.7f,new Color(0,1,0));
        //위치와 방향*길이, 색깔
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f,LayerMask.GetMask("Object"));
        if(rayHit.collider != null)
        {
            //Raycast된 오브젝트를 저장하여 활용하기
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject =null;
    }
}

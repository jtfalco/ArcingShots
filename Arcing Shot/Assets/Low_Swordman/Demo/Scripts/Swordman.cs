using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Helpers;

public class Swordman : PlayerController
{

 

    private void Start()
    {

        m_CapsulleCollider  = this.transform.GetComponent<CapsuleCollider2D>();
        m_Anim = this.transform.Find("model").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();
        if (!LeftFacingNames.Contains(this.name)) LeftFacingNames.Add(this.name);

    }
           
    private void Update()
    {



        checkInput();

        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);

        }

        foreach(KeyValuePair<string, GameObjectPlus> IDWeaponAndRotation in NamesToWeaponsAndRotations)
        {
            Vector3 refAngles = IDWeaponAndRotation.Value.Obj.transform.localEulerAngles;            
            float eX = refAngles.x, requestedX = IDWeaponAndRotation.Value.RequestedRotation.x, minX = IDWeaponAndRotation.Value.MinRotation.x, maxX = IDWeaponAndRotation.Value.MaxRotation.x;
            float eY = refAngles.y, requestedY = IDWeaponAndRotation.Value.RequestedRotation.y, minY = IDWeaponAndRotation.Value.MinRotation.y, maxY = IDWeaponAndRotation.Value.MaxRotation.y;
            float eZ = refAngles.z, requestedZ = IDWeaponAndRotation.Value.RequestedRotation.z, minZ = IDWeaponAndRotation.Value.MinRotation.z, maxZ = IDWeaponAndRotation.Value.MaxRotation.z;

            if (eX <= 360f && eX > 180f) eX -= 360;
            if (eY <= 360f && eY > 180f) eY -= 360;
            if (eZ <= 360f && eZ > 180f) eZ -= 360;

            if ((eX >= maxX ) && requestedX > 0) requestedX = 0f;
            if ((eY >= maxY ) && requestedY > 0) requestedY = 0f;
            if ((eZ >= maxZ ) && requestedZ > 0) requestedZ = 0f;
            if ((eX <= minX ) && requestedX < 0) requestedX = 0f;
            if ((eY <= minY ) && requestedY < 0) requestedY = 0f;
            if ((eZ <= minZ ) && requestedZ < 0) requestedZ = 0f;

            IDWeaponAndRotation.Value.Obj.transform.Rotate(requestedX, requestedY, requestedZ );
        }


    }

    static Dictionary<string, GameObjectPlus> NamesToWeaponsAndRotations = new Dictionary<string, GameObjectPlus>();
    static List<string> LeftFacingNames = new List<string>();


    public void checkInput()
    {
        var weaponId = "Weapon-Sword";


        if (Input.GetKeyDown(KeyCode.S))  //아래 버튼 눌렀을때. 
        {

            IsSit = true;
            m_Anim.Play("Sit");


        }
        else if (Input.GetKeyUp(KeyCode.S))
        {

            m_Anim.Play("Idle");
            IsSit = false;

        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameObject weapon = GameObject.Find(weaponId);
            SetRotationVelocity(weaponId, weapon, new Vector3(0f, 0f, -1.0f), new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));            
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameObject weapon = GameObject.Find(weaponId);
            SetRotationVelocity(weaponId, weapon, new Vector3(0f, 0f, 1.0f), new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            GameObject weapon = GameObject.Find(weaponId);
            SetRotationVelocity(weaponId, weapon, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            GameObject weapon = GameObject.Find(weaponId);
            SetRotationVelocity(weaponId, weapon, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));
        }

        // sit나 die일때 애니메이션이 돌때는 다른 애니메이션이 되지 않게 한다. 
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentJumpCount < JumpCount)  // 0 , 1
                {
                    DownJump();
                }
            }

            return;
        }


        m_MoveX = Input.GetAxis("Horizontal");


   
        GroundCheckUpdate();


        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {


                m_Anim.Play("Attack");
            }
            else
            {

                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle");

                }
                else
                {

                    m_Anim.Play("Run");
                }

            }
        }


        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_Anim.Play("Die");

        }

        // 기타 이동 인풋.

        if (Input.GetKey(KeyCode.D))
        {

            if (isGrounded)  // 땅바닥에 있었을때. 
            {



                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                transform.transform.Translate(Vector2.right* m_MoveX * MoveSpeed * Time.deltaTime);



            }
            else
            {

                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));

            }




            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.A))
                Filp(false);


        }
        else if (Input.GetKey(KeyCode.A))
        {


            if (isGrounded)  // 땅바닥에 있었을때. 
            {



                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;


                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);

            }
            else
            {

                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));

            }


            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.D))
                Filp(true);


        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;


            if (currentJumpCount < JumpCount)  // 0 , 1
            {

                if (!IsSit)
                {
                    prefromJump();


                }
                else
                {
                    DownJump();

                }

            }


        }



    }

    public static void SetRotationVelocity(string id, GameObject go, Vector3 rot, Vector3 mins, Vector3 maxes)
    {
        if(NamesToWeaponsAndRotations.ContainsKey(id))
        {
            NamesToWeaponsAndRotations[id] = new GameObjectPlus(go, go.transform.localEulerAngles, rot, mins, maxes); 
        } else
        {
            NamesToWeaponsAndRotations.Add(id, new GameObjectPlus(go, go.transform.localEulerAngles, rot, mins, maxes));
        }
    }


  


    protected override void LandingEvent()
    {


        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle");

    }





}

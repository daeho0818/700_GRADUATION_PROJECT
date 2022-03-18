using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    Rigidbody2D RB;
    bool isGround;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float jumpTime;
    float jumpTimeCounter;
    bool isJumping;
    bool doubleJumpAble;
    [SerializeField] CameraFollow cam;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        JumpLogic();
        JumpHold();
        CheckGround();
    }

    void FixedUpdate()
    {
        MoveLogic();
    }

    void JumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGround && !isJumping)
        {
            RB.velocity = Vector2.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !isGround && doubleJumpAble)
        {
            RB.velocity = Vector2.up * jumpForce * 1.5f;
            jumpTimeCounter = 0f;
            isJumping = false;
            doubleJumpAble = false;
        }
    }

    void JumpHold()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (jumpTimeCounter > 0 && isJumping && RB.velocity != Vector2.zero)
            {
                //RB.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                RB.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpTimeCounter = 0f;
            isJumping = false;
        }
    }

    void CheckGround()
    {
        bool ground = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if(ground != isGround)
        {
            doubleJumpAble = true;
            isGround = ground;
        }
        else
        {
            isGround = ground;
        }
    }

    void MoveLogic()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CameraRoom"))
        {
            cam.followType = collision.GetComponent<RoomTrigger>().type;

            if(cam.followType == CameraFollowType.FollowArena && !InGameManager.Instance.GameActive)
            {
                InGameManager.Instance.GameStart();
            }
        }
    }
}

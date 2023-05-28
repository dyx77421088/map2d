using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public InputActionReference jumpReference;
    public float speed = 5f;

    private Rigidbody2D rb2d;
    private PlayerInput playerInput;
    private Vector2 moveV2;
    private int jumpCount;
    private int maxJumpCount = 3;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        jumpCount = 3;
    }


    void Update()
    {
        //this.transform.position += new Vector3(moveV2.x * speed * Time.deltaTime, 0, 0);
        Vector2 v2 = rb2d.velocity;
        v2.x = 0;
        rb2d.velocity = v2; 
        if (moveV2.x != 0)
        {
            rb2d.velocity = new Vector2(moveV2.x * speed, rb2d.velocity.y);
        }

        if (moveV2.y == 0 && jumpCount != maxJumpCount)
        {
            Debug.Log("在里面呢");
            var raycastHitAll = Physics2D.RaycastAll(this.transform.position, Vector2.down, 0.7f);
            Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, -0.7f));
            foreach (var hit in raycastHitAll)
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "floor")
                {
                    jumpCount = maxJumpCount;
                }
            }
        }
        
    }

    public void Jump(CallbackContext c)
    {
        if (c.performed && --jumpCount >= 0)
        {
            rb2d.velocity = Vector2.up * 5 ;
        }
    }

    public void Move(CallbackContext c)
    {
        moveV2 = c.ReadValue<Vector2>();
    }

    /// <summary>
    /// 切换按键
    /// </summary>
    public void RebindJumpInput()
    {
        playerInput.SwitchCurrentActionMap("PlayerReBind");
        jumpReference.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete((operation) =>
            {
                Debug.Log("成功了=>" + operation);
                operation.Dispose();
                playerInput.SwitchCurrentActionMap("PlayerNormal");
            })
            .Start();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("进来了，碰撞");
        if (collision.gameObject.tag == "floor")
        {
            
            
        }
    }
}

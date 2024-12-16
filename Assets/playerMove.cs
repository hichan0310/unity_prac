using System.Collections.Generic;
using UnityEngine;
using GameBackend;
using GameBackend.PlayerClasses;

public class PlayerController : Player
{
    public float moveForce; // 이동할 때 적용할 힘
    public float jumpForce; // 점프할 때 적용할 힘
    public float maxSpeed; // 최대 속도
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private SpriteRenderer sr;
    public Animator animator;
    private int attackStep = 0;
    private bool isGrounded; // 바닥에 닿아 있는지 확인하는 변수
    private bool isHamjung; // 함정에 닿아 있는지 확인하는 변수
    private float clickInterval = 0.3f;         // 연타로 인식할 최대 간격
    private float lastClickTime = 0f;           // 마지막 클릭 시각


    public Entity enemytest;


    private float logInterval = 0.5f; // 0.1초 간격
    private float timeSinceLastLog = 0f;

    // 카메라 진동 이벤트
    public UnityEngine.Events.UnityEvent onVibrationEvent;


    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져옴
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        Jump();
        CheckVibration(); // 진동 체크
        this.update();

        if (isHamjung)
        {
            timeSinceLastLog += Time.deltaTime;

            if (timeSinceLastLog >= logInterval)
            {
                List<Tags.atkTags> tag = new List<Tags.atkTags>();
                tag.Add(Tags.atkTags.fireAttack);
                this.dmgtake(new DmgEvent(enemytest, this, 10, tag));
                timeSinceLastLog = 0;
            }
        }
        else
        {
            timeSinceLastLog = 0;
        }
        
        NormalAttack();
    }

    void NormalAttack()
    {
        this.weapon.normalAttack(this, Time.deltaTime);
    }

    void Move()
    {
        // 좌우 키 입력 감지
        float moveX = Input.GetAxis("Horizontal");

        // 입력이 있을 경우 힘을 적용
        if (moveX != 0)
        {
            Vector2 force = new Vector2(moveX, 0).normalized * maxSpeed;
            var vector2 = rb.velocity;
            vector2.x = force.x;
            rb.velocity = vector2;
        }

        // 마찰력으로 감쇠
        //rb.velocity *= (1f - drag); // 간단한 감쇠를 통해 관성 구현
    }

    void Jump()
    {
        // 점프 버튼 (스페이스바) 감지
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // 점프
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 바닥에 닿음
        }

        // 함정에 닿았는지 확인
        if (collision.gameObject.CompareTag("Hamjung"))
        {
            isHamjung = true; // 바닥에 닿음
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 바닥에서 떨어졌는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 바닥에서 떨어짐
        }

        // 함정에서 떨어졌는지 확인
        if (collision.gameObject.CompareTag("Hamjung"))
        {
            isHamjung = false; // 바닥에 닿음
        }
    }

    void CheckVibration()
    {
        // Enter 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Return))
        {
            onVibrationEvent.Invoke(); // 진동 이벤트 호출
        }
    }
}
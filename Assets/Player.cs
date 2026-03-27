using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    private Rigidbody2D rb;
    private float highPosition;

    private float tiltInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        highPosition = transform.position.y;

        // Kích hoạt cảm biến gia tốc nếu có
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
        }
    }

    void Update()
    {
        float keyboardTilt = 0f;
        float accelTilt = 0f;

        // 1. Lấy input từ bàn phím (nếu có InputSystem_Actions)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) keyboardTilt = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) keyboardTilt = 1f;
        }

        // 2. Lấy input từ cảm biến nghiêng (Điện thoại)
        if (Accelerometer.current != null)
        {
            accelTilt = Accelerometer.current.acceleration.ReadValue().x;
        }

        // Ưu tiên bàn phím nếu đang nhấn, nếu không thì dùng cảm biến nghiêng
        tiltInput = Mathf.Abs(keyboardTilt) > 0.1f ? keyboardTilt : accelTilt;

        // Deadzone cho cảm biến nghiêng
        if (Mathf.Abs(tiltInput) < 0.05f) tiltInput = 0f;
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        // Sử dụng Lerp để di chuyển mượt mà hơn
        velocity.x = Mathf.Lerp(velocity.x, tiltInput * movementSpeed, 0.2f);
        rb.linearVelocity = velocity;

        CalculateScore();
    }



    private void CalculateScore()
    {
        if (transform.position.y > highPosition)
        {
            float diff = transform.position.y - highPosition;

            if (diff >= 1f)
            {
                int add = Mathf.FloorToInt(diff) * 10;
                GamePlayManager.curScore += add;
                highPosition = transform.position.y;
                if (GamePlayManager.curScore > GamePlayManager.highScoreGamePlay) // nếu điểm hiện tại > điểm cao nhất 
                {GamePlayManager.highScoreGamePlay = GamePlayManager.curScore; // cập nhật biến cao nhất tạm thời bằng biến hiện tại 
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.CompareTag("SpringShoe"))
        {
         GamePlayManager.instance.SpringShoeBoot(other);
        }
        GamePlayManager.instance.ChangeTopTheme(other);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BodyEnemy"))
        {
            GamePlayManager.instance.GameOver();
        }
        if (collision.collider.CompareTag("WinPoint"))
        {
            GamePlayManager.instance.GameWin();
            
        }
        
    }
}



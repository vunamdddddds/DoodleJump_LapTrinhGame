using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    private Rigidbody2D rb;

    public GameObject PlayerManager;
    private float highPosition; // Biến để lưu vị trí cao nhất của nhân vật



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        highPosition = transform.position.y; // Lưu vị trí cao nhất ban đầu của nhân vật
    }



    void FixedUpdate() // Sử dụng FixedUpdate để xử lý vật lý==> tránh việc bỏ sót input khi frame rate thấp
    {
        float movement = Input.acceleration.x * movementSpeed;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = movement;
        rb.linearVelocity = velocity;



        CalculateScore();
    }

    private void CalculateScore()
    {
        if (transform.position.y > highPosition)
        {
            float diff = transform.position.y - highPosition;

            if (diff >= 1f) // mỗi 1 đơn vị chiều cao
            {
                int add = Mathf.FloorToInt(diff) * 10;
                GamePlayManager.curScore += add;
                highPosition = transform.position.y;
                if (GamePlayManager.curScore > saveData.playerContainer.players[0].highScore)
                {
                    GamePlayManager.instance.highScoreText.text = GamePlayManager.curScore.ToString();
                    saveData.playerContainer.players[0].highScore = GamePlayManager.curScore;
                    saveData.Save();
                }
                Debug.Log("Score: " + GamePlayManager.curScore);
            }

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
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



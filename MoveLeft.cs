using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;
    private bool gameOver = false;

    void Update()
    {
        if (!gameOver)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }
}
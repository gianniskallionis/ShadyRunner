using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool gameOver = false;
    private int score = 0;
    public int winScore = 1;

    public Text scoreText;
    public GameObject gameOverScreen;
    public GameObject congratsScreen;
    public AudioClip introSound;
    public AudioClip loopMusic;
    private AudioSource musicSource;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

        public void AddScore(int amount)
    {
            Debug.Log("Score=" + score + " winScore=" + winScore + " gameOver=" + gameOver);
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (score >= winScore)
        {
            TriggerCongrats();
        }
    }

    public void TriggerGameOver()
    {
        Debug.Log("TriggerCongrats CALLED, gameOver was: " + gameOver);
        if (gameOver) return;
        gameOver = true;

        MoveLeft[] movers = FindObjectsOfType<MoveLeft>();
        foreach (MoveLeft m in movers) m.SetGameOver(true);

        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        if (spawner != null) spawner.SetGameOver(true);

        playerController player = FindObjectOfType<playerController>();
        if (player != null) player.SetGameOver(true);

        CancelInvoke("PlayLoopMusic");
        musicSource.Stop();

        if (gameOverScreen != null) gameOverScreen.SetActive(true);

        RewardSpawner rewardSpawner = FindObjectOfType<RewardSpawner>();
        if (rewardSpawner != null) rewardSpawner.SetGameOver(true);
    }

        public void TriggerCongrats()
    {
        if (gameOver) return;
        gameOver = true;

        MoveLeft[] movers = FindObjectsOfType<MoveLeft>();
        foreach (MoveLeft m in movers) m.SetGameOver(true);

        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        if (spawner != null) spawner.SetGameOver(true);

        playerController player = FindObjectOfType<playerController>();
        if (player != null) player.SetWin();

        RewardSpawner rewardSpawner = FindObjectOfType<RewardSpawner>();
        if (rewardSpawner != null) rewardSpawner.SetGameOver(true);

        CancelInvoke("PlayLoopMusic");
        musicSource.Stop();

        if (congratsScreen != null) congratsScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = introSound;
        musicSource.loop = false;
        musicSource.Play();
        Invoke("PlayLoopMusic", introSound.length);
    }

    void PlayLoopMusic()
    {
        musicSource.clip = loopMusic;
        musicSource.loop = true;
        musicSource.Play();
    }




}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Controls game state
public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public Text scoreText;
    public Text hiscoreText;
    public Button playButton;
    public float scrollingSpeed = 4;
    public bool gameOver;

    private int score = 0;
    private int hiscore = 0;


    // Called once on every gaming session before Start
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Called on every start of game
    private void Start()
    {
        gameOver = false;
    }

    // Called by other scripts when the player scores a point
    public void AddPoint()
    {
        if (gameOver == false)
        {
            score++;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Called by other scripts if the player loses
    public void GameOver()
    {
        Debug.Log("Game over");
        gameOver = true;
        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "Hiscore: " + hiscore.ToString();
        }
        playButton.gameObject.SetActive(true);
    }

    // Starts the game over - attached to Replay button
    public void Replay()
    {
        //TODO: Save hiscore
        Debug.Log("Starting game over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
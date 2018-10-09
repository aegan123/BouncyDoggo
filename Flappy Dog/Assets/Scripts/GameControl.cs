using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 * This script controls the game state.
 */
public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public Text scoreText;
    public Text hiscoreText;
    public Button playButton;
    public float scrollSpeed = -3f;
    public bool gameOver = false;

    private int score = 0;
    private int hiscore = 0;


    /*
     * Awake-method is called only once per game session before the first Start-call.
     * Use it for instantiating essential objects & loading saves etc.
     */
    void Awake()
    {
        //Instantiates GameControl
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //GameControl doesn't need to follow frame updates
    void Update()
    {}

    //Can be called by other scripts when the player scores a point
    public void AddPoint()
    {
        if (gameOver == false)
        {
            score++;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    //Called by other scripts if the player loses
    public void GameOver()
    {
        gameOver = true;

        //Checks for hiscore
        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "Hiscore: " + hiscore.ToString();
        }

        //Shows play button
        playButton.gameObject.SetActive(true);
    }

    //This method should be attached to the replay button
    public void Replay()
    {
        //This starts the game over
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
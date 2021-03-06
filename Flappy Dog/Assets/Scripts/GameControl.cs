﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

// Controls game state
public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public Text scoreText;
    public Text hiscoreText;
    public Text pizzaText;
    public GameObject gameOverText;
    public GameObject tryAgainText;
    public GameObject GameOverPanel;
    public Button playButton;
    public String hiscorePath = "hiscore.txt";
    public bool gameOver;

    private int score = 0;
    private int hiscore;
    private float timer = 0;
    private int numOfFood = 0;

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
        //creates initial save file if currently not existing
        if (!File.Exists(hiscorePath))
        {
            File.WriteAllText(hiscorePath, "0");
        }
    }

    //GameControl doesn't need to follow frame updates. Counts score for every second.
    void Update()
    {
        if(Dog.godMode)
        {
            return;
        }
        // adds points

        timer += Time.deltaTime;
        if (!gameOver)
        {
            if (timer > 1f)
            {
                score += 1;
                scoreText.text = "Score: " + score.ToString();

                timer = 0;
            }
        }
        else
        {
            score = 0;
        }
    }

    // Called on every start of game
    private void Start()
    {
        score = 0;
        hiscore = Int32.Parse(File.ReadAllText(hiscorePath));
        hiscoreText.text = "Hiscore: " + hiscore.ToString();
        pizzaText.text = "Pizza: " + numOfFood.ToString() + " / 5";
        SoundManager.instance.gameOver.Stop();
        SoundManager.instance.backgroudMusic.Play();
        gameOver = false;
    }

    // Called by other scripts when the player scores a point
    public void AddPoint(int point)
    {
        if (!gameOver)
        {
            score+=point;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Called by other scripts when eating any food object.
    public void eatFood(int foodCount) {
        pizzaText.text = "Pizza: " + foodCount.ToString () + " / 5";
    }

    // Called by other scripts if the player loses
    public void GameOver()
    {
        gameOver = true;
        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "Hiscore: " + hiscore.ToString();
            using(StreamWriter writetext = new StreamWriter(hiscorePath)) {
                writetext.WriteLine(hiscore.ToString());
            }
        }
        playButton.gameObject.SetActive(true);

        // shows game over text
        gameOverText.gameObject.SetActive(true);

        //shows Try again? text
        tryAgainText.gameObject.SetActive(true);

        //shows GameOverPanel
        GameOverPanel.gameObject.SetActive(true);

    }

    // Starts the game over - attached to Replay button
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

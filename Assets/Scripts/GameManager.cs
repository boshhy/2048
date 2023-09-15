using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

// Used to keep track of game score, start a new game,
// and display game over
public class GameManager : MonoBehaviour
{

    // Reference to the board
    public TileBoard board;

    // Used as the reference to the gameover CanvasGroup
    public CanvasGroup gameOver;

    // Reference to the currentScoreText
    public TextMeshProUGUI currentScoreText;

    // Reference to the highScoreText
    public TextMeshProUGUI highScoreText;

    // Reference to the score
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        // Start a new game
        NewGame();
    }

    // Used to start a new game
    public void NewGame()
    {
        // Reset score back to 0
        // Get the highScore and update the highScoreText
        SetScore(0);
        highScoreText.text = LoadHighScore().ToString();

        // Set opacity to 0 of the game over CanvasGroup
        // Make the group non interactable
        gameOver.alpha = 0.0f;
        gameOver.interactable = false;

        // Clear the board, create two tiles, and enable game board.
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    // Used to display game over
    public void GameOver()
    {
        // Disable the game board and make
        // gameover CanvasGroup interactable
        board.enabled = false;
        gameOver.interactable = true;

        // fade in the game over CanvasGroup
        StartCoroutine(FadeGameOver(gameOver, 1.0f, 1.0f));
    }

    // Used with CanvasGroup to fade the group in and out
    private IEnumerator FadeGameOver(CanvasGroup canvasGroup, float to, float delay)
    {
        // Delay fade by delay seconds
        yield return new WaitForSeconds(delay);

        // Set up variables for lerp animation
        float elapsed = 0.0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        // Keep shifting while the time has not surpassed the duration
        while (elapsed < duration)
        {
            // Change the opacity of canvasGroup by interpolation by elapsed / duration
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set the final position of the canvasGroup
        canvasGroup.alpha = to;
    }

    // Used to increase score
    public void IncreaseScore(int points)
    {
        // set the score by addint points to it
        SetScore(score + points);
    }

    // Sets the score
    private void SetScore(int score)
    {
        // Set score and update the currentScoreText
        this.score = score;
        currentScoreText.text = this.score.ToString();

        // Call saveHighScore to see if score needs to update
        SaveHighScore();
    }

    // Uploads and saves hichScore if necessary 
    private void SaveHighScore()
    {
        // Load the highscore from computer
        int highScore = LoadHighScore();

        // If the score of current game is higher than the 
        // highscore from the computer then update highscore
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            highScoreText.text = score.ToString();
        }
    }

    // Used to get highscore saved on the computer
    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }

    // Used to quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}

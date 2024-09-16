using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TMP_InputField playerXInput; // Attach TMP_InputField for Player X
    public TMP_InputField playerOInput; // Attach TMP_InputField for Player O

    public static MainMenuController instance; // Singleton instance

    public string playerXName { get; private set; }
    public string playerOName { get; private set; }

    void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This function could be called by a button to start the game
    public void StartGame()
    {
        // Capture player names from InputFields
        playerXName = string.IsNullOrEmpty(playerXInput.text) ? "Player X" : playerXInput.text;
        playerOName = string.IsNullOrEmpty(playerOInput.text) ? "Player O" : playerOInput.text;

        // Load the TicTacToe game scene
        SceneManager.LoadScene("GameScene");
    }
}

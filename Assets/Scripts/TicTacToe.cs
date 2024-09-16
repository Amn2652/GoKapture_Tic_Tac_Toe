using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Needed for loading scenes
using System.Collections; // Needed for coroutines

public class TicTacToe : MonoBehaviour
{
    public Button[] cells; // Attach the buttons from the GameBoard
    public TextMeshProUGUI statusText; // TextMeshPro for status text
    public GameObject winPanel; // The panel that shows up when a player wins
    public GameObject GameGrid;
    public GameObject XOText;

    public Sprite xSprite; // Assign your X sprite in the inspector
    public Sprite oSprite; // Assign your O sprite in the inspector

    private string currentPlayer = "X"; // Player X starts
    private string[] board = new string[9]; // Board state

    // Variables to hold the player names from MainMenuController
    private string playerXName;
    private string playerOName;

    // Store the original position of the status text
    private Vector3 originalStatusPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get player names from MainMenuController
        playerXName = MainMenuController.instance.playerXName; // Player X's name from input
        playerOName = MainMenuController.instance.playerOName; // Player O's name from input

        // Store the original position of the status text
        originalStatusPosition = statusText.rectTransform.localPosition;

        ResetGame();
    }

    public void OnCellClicked(int index)
    {
        // If the cell is empty, mark it with the current player's symbol
        if (string.IsNullOrEmpty(board[index]))
        {
            board[index] = currentPlayer;

            // Set the image instead of text based on the current player
            Image cellImage = cells[index].GetComponent<Image>();
            if (currentPlayer == "X")
            {
                Color cellColor = cellImage.color;
                cellColor.a = 1f; // Set transparency (0 is fully transparent, 1 is fully opaque)
                cellImage.color = cellColor;
                cellImage.sprite = xSprite;
            }
            else
            {
                Color cellColor = cellImage.color;
                cellColor.a = 1f; // Set transparency (0 is fully transparent, 1 is fully opaque)
                cellImage.color = cellColor;
                cellImage.sprite = oSprite;
            }

            if (CheckForWin())
            {
                string winnerName = (currentPlayer == "X") ? playerXName : playerOName;
                statusText.text = $"{winnerName} Wins!";
                DisableAllButtons();

                // Move the status text down to -11 on the Y-axis
                statusText.rectTransform.localPosition = new Vector3(statusText.rectTransform.localPosition.x, -11, statusText.rectTransform.localPosition.z);

                winPanel.SetActive(true); // Show the win panel when a player wins
                GameGrid.SetActive(false);
                XOText.SetActive(false);

                // Start the coroutine to hide win panel and reset elements after 5 seconds
                StartCoroutine(HideWinPanelAfterDelay());
                return;
            }

            if (IsBoardFull())
            {
                ResetGame();
                statusText.text = "It's a Draw!";
                return;
            }

            // Switch players
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
            string nextPlayerName = (currentPlayer == "X") ? playerXName : playerOName;
            statusText.text = $"{nextPlayerName}'s Turn";
        }
    }

    private bool CheckForWin()
    {
        // Define all possible win conditions
        int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}              // Diagonals
        };

        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (!string.IsNullOrEmpty(board[a]) && board[a] == board[b] && board[a] == board[c])
            {
                return true; // A player has won
            }
        }

        return false;
    }

    private bool IsBoardFull()
    {
        // Check if all cells are filled
        foreach (string cell in board)
        {
            if (string.IsNullOrEmpty(cell))
            {
                return false;
            }
        }
        return true;
    }

    private void DisableAllButtons()
    {
        // Disable all buttons after the game ends
        foreach (Button button in cells)
        {
            button.interactable = false;
        }
    }

    public void ResetGame()
    {
        // Hide the win panel at the start
        winPanel.SetActive(false);

        // Reset the Y position of the status text to its original value
        statusText.rectTransform.localPosition = originalStatusPosition;

        // Reset the game
        board = new string[9];
        currentPlayer = "X";
        statusText.text = $"{playerXName}'s Turn";

        foreach (Button button in cells)
        {
            button.interactable = true;

            // Reset the images on each cell
            button.GetComponent<Image>().sprite = null;

            // Set the image to be transparent by setting the alpha value to 0
            Color transparentColor = button.GetComponent<Image>().color;
            transparentColor.a = 0f; // Fully transparent
            button.GetComponent<Image>().color = transparentColor;
        }

        // Reactivate GameGrid and XOText
        GameGrid.SetActive(true);
        XOText.SetActive(true);
    }


    // Coroutine to hide win panel and reactivate elements after 5 seconds
    private IEnumerator HideWinPanelAfterDelay()
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds

        // Hide the win panel and reset elements
        winPanel.SetActive(false);
        ResetGame(); // Reset the game state after the win panel disappears
    }

    // Function to load the main menu (scene 0) when home button is clicked
    public void OnHomeButtonClicked()
    {
        ResetGame();
        Destroy(MainMenuController.instance.gameObject); // Destroy the current instance of MainMenuController
        SceneManager.LoadScene(0); // Load the main menu scene (scene 0)
    }
}

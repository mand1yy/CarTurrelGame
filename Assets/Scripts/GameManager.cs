using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Button restartButton;

    private bool gameOver = false;

    public void GameOver(bool victory)
    {
        gameOver = true;

        resultText.text = victory ? "Victory" : "Lose";
        resultText.gameObject.SetActive(true);

        restartButton.gameObject.SetActive(true);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            RestartGame();
        }
    }
}

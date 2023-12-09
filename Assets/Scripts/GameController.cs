using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _blackFade;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private TextAnimatorPlayer _endText;
    [SerializeField] private CarController _carController;
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
        AudioManager.Instance.Play("Theme");
    }

    public void StartGame()
    {
        _carController.CanMove = true;
        _blackFade.SetActive(false);
    }

    public void EndGame(bool isWin)
    {
        _endPanel.SetActive(true);
        _endText.ShowText(isWin ? "<bounce>You win</bounce>" : "<bounce>You lose</bounce>");
        AudioManager.Instance.Play(isWin ? "Win" : "Lose");
        _carController.CanMove = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
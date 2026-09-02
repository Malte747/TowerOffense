using UnityEngine;
using UnityEngine.UI;

public class RundeBeendenButtonExtra : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] Button pauseButton;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void VersucheRundeZuBeenden ()
    {
        if (uiManager.timePaused)
        {
            ClickPauseButton();
        }
        else
        {
            gameManager.EndTrun();
        }
    }

    public void ClickPauseButton()
    {
        uiManager.PauseTime();
        gameManager.PauseButtonAnimationBoolFalse();
        gameManager.ChangeRundeBeendenButton(false);
        FindFirstObjectByType<AudioManager>().PlayUISound(0);
    }

}

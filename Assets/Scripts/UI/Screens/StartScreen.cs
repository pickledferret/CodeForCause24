using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public void OnStartPressed()
    {
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}

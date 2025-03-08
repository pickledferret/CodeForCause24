using UnityEngine;

public class OpenSettingsButton : MonoBehaviour
{
    private const string PATH = "Prefabs/UI/SettingsScreen";
    public void OnSettingsButtonPressed()
    {
        SettingsScreen settingsPrefab = Resources.Load<SettingsScreen>(PATH);

        if (settingsPrefab == null)
        {
            Debug.LogError($"Failed to load prefab at path: {PATH}");
            return;
        }

        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        SettingsScreen settingsScreen = Instantiate(settingsPrefab, GameManager.Instance.Canvas.transform);
        settingsScreen.transform.SetAsLastSibling();
    }
}

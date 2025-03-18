using UnityEngine;

public class OpenSettingsButton : MonoBehaviour
{
    public void OnSettingsButtonPressed()
    {
        SettingsScreen settingsPrefab = Resources.Load<SettingsScreen>(SettingsScreen.PATH);

        if (settingsPrefab == null)
        {
            Debug.LogError($"Failed to load prefab at path: {SettingsScreen.PATH}");
            return;
        }

        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        SettingsScreen settingsScreen = Instantiate(settingsPrefab, GameManager.Instance.Canvas.transform);
        settingsScreen.transform.SetAsLastSibling();
    }
}

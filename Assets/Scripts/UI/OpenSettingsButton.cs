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

        SettingsScreen settingsScreen = Instantiate(settingsPrefab, GameManager.Instance.Canvas.transform);
        settingsScreen.transform.SetAsLastSibling();
    }
}

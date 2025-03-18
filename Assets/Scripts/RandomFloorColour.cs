using UnityEngine;

public class RandomFloorColour : MonoBehaviour
{
    [SerializeField] private Renderer m_floor;

    public void RandomiseColour()
    {
        Color currentColor = m_floor.material.color;
        Color.RGBToHSV(currentColor, out float h, out float s, out float v);
        h = Random.Range(0f, 1f);
        m_floor.material.color = Color.HSVToRGB(h, s, v);
    }
}
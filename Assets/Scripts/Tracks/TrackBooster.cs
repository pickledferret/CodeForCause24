using UnityEngine;

public class TrackBooster : TrackBase
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_duration;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            m_player.ApplySpeedBoostMultiplier(m_speed, m_duration);

            AudioManager audioManager = AudioManager.Instance;
            audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.boostPad);
        }
    }
}

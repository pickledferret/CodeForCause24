using UnityEngine;

public class TrackFinishLine : TrackBase
{
    private bool m_triggered;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player && !m_player.HasCrashed && !m_triggered)
        {
            m_triggered = true;
            m_player.TrackComplete();
            GameManager.Instance.LevelCompleted();
        }
    }
}

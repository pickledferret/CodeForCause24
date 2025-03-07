using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TrackBase : MonoBehaviour
{
    protected Player m_player;

    protected virtual void Awake()
    {
        GameplayEvents.UserInputPressed += GameplayEvents_UserInputPressed;
        GameplayEvents.UserInputReleased += GameplayEvents_UserInputReleased;
    }

    private void OnDestroy()
    {
        GameplayEvents.UserInputPressed -= GameplayEvents_UserInputPressed;
        GameplayEvents.UserInputReleased -= GameplayEvents_UserInputReleased;
    }

    protected virtual void GameplayEvents_UserInputPressed()
    {
    }

    protected virtual void GameplayEvents_UserInputReleased()
    {
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            m_player = player;
        }
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            m_player = null;
        }
    }
}
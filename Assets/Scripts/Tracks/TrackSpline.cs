using UnityEngine;
using UnityEngine.Splines;

public class TrackSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer m_spline;

    public SplineContainer GetSpline()
    {
        return m_spline;
    }
}
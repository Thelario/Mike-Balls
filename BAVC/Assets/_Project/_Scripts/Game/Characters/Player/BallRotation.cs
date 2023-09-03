using Game;
using UnityEngine;

public class BallRotation : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private float direction;
    [SerializeField] private Transform thisTransform;
    
    private void FixedUpdate()
    {
        Rotate();
    }
    
    private void Rotate()
    {
        float zAngles;
        if (active)
            zAngles = direction * PlayerStats.Instance.BallRotationSpeed * Time.fixedDeltaTime;
        else
            zAngles = direction * (PlayerStats.Instance.BallRotationSpeed / 1.75f) * Time.fixedDeltaTime;
        
        thisTransform.Rotate(zAngles * Vector3.forward);
    }
}

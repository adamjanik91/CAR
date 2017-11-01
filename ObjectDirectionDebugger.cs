using UnityEngine;

public class ObjectDirectionDebugger : MonoBehaviour
{
    public float RayLength;
    public Color ForwardRayColor;
    public Color BackwardRayColor;
    public Color RightRayColor;
    public Color LeftRayColor;
    public Color UpRayColor;
    public Color DownRayColor;
    public bool CastForwardRay;
    public bool CastBackwardRay;
    public bool CastRightRay;
    public bool CastLeftRay;
    public bool CastUpRay;
    public bool CastDownRay;
    void Update()
    {
        CastDirectionRays();
    }

    private void CastDirectionRays()
    {
        if (CastForwardRay)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * RayLength;
            Debug.DrawRay(transform.position, forward, ForwardRayColor);
        }
        if (CastBackwardRay)
        {
            Vector3 backward = transform.TransformDirection(Vector3.back) * RayLength;
            Debug.DrawRay(transform.position, backward, BackwardRayColor);
        }
        if (CastRightRay)
        {
            Vector3 right = transform.TransformDirection(Vector3.right) * RayLength;
            Debug.DrawRay(transform.position, right, RightRayColor);
        }
        if (CastLeftRay)
        {
            Vector3 left = transform.TransformDirection(Vector3.left) * RayLength;
            Debug.DrawRay(transform.position, left, LeftRayColor);
        }
        if (CastUpRay)
        {
            Vector3 up = transform.TransformDirection(Vector3.up) * RayLength;
            Debug.DrawRay(transform.position, up, UpRayColor);
        }
        if (CastDownRay)
        {
            Vector3 down = transform.TransformDirection(Vector3.down) * RayLength;
            Debug.DrawRay(transform.position, down, DownRayColor);
        }
    }
}

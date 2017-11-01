using UnityEngine;

public class ObjectMovementInfoProvider : MonoBehaviour
{
    public bool ShowVelocity;

    private Rigidbody _Rigidbody;
    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (ShowVelocity) {
            Vector3 currentVelocity = _Rigidbody.velocity;
            Debug.Log(currentVelocity.normalized);
        }
    }
}

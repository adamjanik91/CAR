using UnityEngine;
public class VehicleController : MonoBehaviour
{
    public float TurnForceMultiplier;
    public float MinTurningSpeed;
    [HideInInspector]
    public float MaxSpeed;
    [HideInInspector]
    public float TopSpeed;
    [HideInInspector]
    public float Accel;
    [HideInInspector]
    public Rigidbody _rigidbody;
    [HideInInspector]
    public Vector3 _prevPos;
    [HideInInspector]
    public Vector3 _prevForward;
    [HideInInspector]
    public ActiveCarInputProvider _input;
    [HideInInspector]
    public MovementCalculator _calc;

    public virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _prevPos = transform.position;
        _calc = new MovementCalculator();
        var gameController = GameObject.FindGameObjectWithTag("GameController");
        _input = gameController.GetComponent<ActiveCarInputProvider>();
    }
    public virtual void Update()
    {

    }
    public virtual void LateUpdate()
    {
        _prevPos = transform.position;
        _prevForward = transform.forward;
    }
}
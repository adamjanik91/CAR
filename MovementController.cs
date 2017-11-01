 using System;
using UnityEngine;
public class MovementController : MonoBehaviour
{
    public float TurnForceMultiplier;

    public Gear Gear;

    public float MinTurningSpeed;

    public float MaxSpeedRevGear;
    public float MaxSpeed1stGear;
    public float MaxSpeed2ndGear;
    public float MaxSpeed3rdGear;
    public float MaxSpeed4thGear;

    public float AccelRevGear;
    public float Accel1stGear;
    public float Accel2ndGear;
    public float Accel3rdGear;
    public float Accel4thGear;

    public Gear MaxGear;
    public Gear MinGear;

    private Rigidbody _rigidbody;
    private Vector3 _prevPos;
    private Vector3 _prevForward;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Gear = Gear.N;
        _prevPos = transform.position;
    }
    void Update()
    {
        var movement = (transform.position - _prevPos);

        Vector3 currentVelocity = GetVelocity(transform.position, _prevPos);

        float currentForwardSpeed = GetCurrentForwardSpeed(currentVelocity);
        var currentForwardDir = GetCurrentForwardDir(_prevForward, movement);

        ChangeGear();
        if (Gear != Gear.N)
            Move(currentForwardSpeed);

        //if (currentForwardSpeed >= MinTurningSpeed)
            Rotate(currentForwardDir, currentForwardSpeed);
    }
    private void LateUpdate()
    {
        _prevPos = transform.position;
        _prevForward = transform.forward;
    }
    private Vector3 GetVelocity(Vector3 currentPos, Vector3 prevPos)
    {
        float x = (currentPos.x - prevPos.x) / Time.deltaTime;
        float y = (currentPos.y - prevPos.y) / Time.deltaTime;
        float z = (currentPos.z - prevPos.z) / Time.deltaTime;
        return new Vector3(x, y, z);
    }
    private void ChangeGear()
    {
        int currentGear = (int)Gear;
        if (Input.GetButtonDown("GearUp") && Gear != MaxGear)
            currentGear++;
        else if (Input.GetButtonDown("GearDown") && Gear != MinGear)
            currentGear--;
        Gear = (Gear)currentGear;
    }
    private float GetCurrentForwardDir(Vector3 prevForward, Vector3 movement)
    {
        return Vector3.Dot(prevForward, movement);
    }
    private float GetCurrentForwardSpeed(Vector3 currentVelocity)
    {
        return Mathf.Abs(currentVelocity.z);
    }
    private void Move(float currentForwardSpeed)
    {
        var verticalInput = Input.GetAxis("Vertical");
        if (verticalInput > 0)
        {
            float accel = GetAccel();

            bool maxSpeedReached = IsMaxSpeedReached(currentForwardSpeed);

            int currentGear = (int)Gear;
            int gearDir = (currentGear > 0) ? 1 : -1;

            if (maxSpeedReached == false && currentGear != 0)
                _rigidbody.AddForce(transform.forward * accel * gearDir);
        }
    }
    private void Rotate(float currentForwardDir, float currentForwardSpeed)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        var force = CalculateRotationForce(new RotateForceCalcModel()
        {
            HorizontalInput = horizontalInput,
            CurrentForwardSpeed = currentForwardSpeed,
            TurnForceMultiplier = TurnForceMultiplier,
            CurrentForwardDir = currentForwardDir
        });
        transform.Rotate(Vector3.up * Time.deltaTime * force, Space.World);
    }
    private float CalculateRotationForce(RotateForceCalcModel model)
    {
        var speedMultiplier = model.TurnForceMultiplier
            //model.CurrentForwardSpeed 
            //* 
            ;
        var force = speedMultiplier * model.HorizontalInput * model.CurrentForwardDir;
        return force;
    }
    private bool IsMaxSpeedReached(float currentSpeed)
    {
        if (Gear == Gear.R && currentSpeed < MaxSpeedRevGear)
            return false;
        else if (Gear == Gear.First && currentSpeed < MaxSpeed1stGear)
            return false;
        else if (Gear == Gear.Second && currentSpeed < MaxSpeed2ndGear)
            return false;
        else if (Gear == Gear.Third && currentSpeed < MaxSpeed3rdGear)
            return false;
        else if (Gear == Gear.Fourth && currentSpeed < MaxSpeed4thGear)
            return false;

        return true;
    }
    private float GetAccel()
    {
        switch (Gear)
        {
            case Gear.R:
                return AccelRevGear;
            case Gear.N:
                break;
            case Gear.First:
                return Accel1stGear;
            case Gear.Second:
                return Accel2ndGear;
            case Gear.Third:
                return Accel3rdGear;
            case Gear.Fourth:
                return Accel4thGear;
        }
        return 0;
    }
}
public enum Gear { R = -1, N = 0, First = 1, Second = 2, Third = 3, Fourth = 4 };
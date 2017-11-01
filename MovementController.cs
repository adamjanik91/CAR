using System.Collections.Generic;
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

    private ActiveCarInputProvider _input;
    private MovementCalculator _calc;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Gear = Gear.N;
        _prevPos = transform.position;

        _calc = new MovementCalculator();

        var gameController = GameObject.FindGameObjectWithTag("GameController");
        _input = gameController.GetComponent<ActiveCarInputProvider>();
    }
    void Update()
    {
        var movement = (transform.position - _prevPos);

        Vector3 currentVelocity = _calc.GetVelocity(transform.position, _prevPos);

        float currentForwardSpeed = _calc.GetCurrentForwardSpeed(currentVelocity);
        var currentForwardDir = _calc.GetCurrentForwardDir(_prevForward, movement);

        ChangeGear();
        if (Gear != Gear.N)
            Move(currentForwardSpeed);

        Rotate(currentForwardDir, currentForwardSpeed);
    }
    private void LateUpdate()
    {
        _prevPos = transform.position;
        _prevForward = transform.forward;
    }
    
    private void ChangeGear()
    {
        int currentGear = (int)Gear;
        if (_input.GearUp && Gear != MaxGear)
            currentGear++;
        else if (_input.GearDown && Gear != MinGear)
            currentGear--;
        Gear = (Gear)currentGear;
    }
    private void Move(float currentForwardSpeed)
    {
        if (_input.AccelerateAxis > 0)
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
        var force = _calc.CalculateRotationForce(new RotateForceCalcModel()
        {
            HorizontalInput = _input.TurnAxis,
            CurrentForwardSpeed = currentForwardSpeed,
            TurnForceMultiplier = TurnForceMultiplier,
            CurrentForwardDir = currentForwardDir
        });
        transform.Rotate(Vector3.up * Time.deltaTime * force, Space.World);
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
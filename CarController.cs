using UnityEngine;
public class CarController : VehicleController
{
    public Gear Gear;
    public Gear MaxGear;
    public Gear MinGear;

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

    public override void Start()
    {
        base.Start();
        TurnForceMultiplier = 750;
        Gear = Gear.N;
        MaxGear = Gear.Fourth;
        MinGear = Gear.R;
        MaxSpeedRevGear = 5;
        MaxSpeed1stGear = 5;
        MaxSpeed2ndGear = 10;
        MaxSpeed3rdGear = 17;
        MaxSpeed4thGear = 16;
        TopSpeed = MaxSpeed4thGear;
        MinTurningSpeed = GetMinTurningSpeed(TopSpeed);
        AccelRevGear = 19;
        Accel1stGear = 20;
        Accel2ndGear = 18;
        Accel3rdGear = 17;
        Accel4thGear = 16;
    }
    private float GetMinTurningSpeed(float topSpeed)
    {
        return topSpeed * -1;
    }
    public override void Update()
    {
        base.Update();

        var movement = (transform.position - _prevPos);

        Vector3 currentVelocity = _calc.GetVelocity(transform.position, _prevPos);

        float currentForwardSpeed = _calc.GetCurrentForwardSpeed(currentVelocity);
        var currentForwardDir = _calc.GetCurrentForwardDir(_prevForward, movement);

        ChangeGear();
        if (Gear != Gear.N)
            Move(currentForwardSpeed);

        Rotate(currentForwardDir, currentForwardSpeed);
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

            float maxSpeed = GetCurrentMaxSpeed(Gear);
            bool maxSpeedReached = IsMaxSpeedReached(currentForwardSpeed, maxSpeed);

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
    private float GetCurrentMaxSpeed(Gear gear)
    {
        switch (gear)
        {
            case Gear.R: return MaxSpeedRevGear;
            case Gear.N: return TopSpeed;
            case Gear.First: return MaxSpeed1stGear;
            case Gear.Second: return MaxSpeed2ndGear;
            case Gear.Third: return MaxSpeed3rdGear;
            case Gear.Fourth: return MaxSpeed4thGear;
            default: return 0.0f;
        }
    }
    private bool IsMaxSpeedReached(float currentSpeed, float maxSpeed)
    {
        return (currentSpeed >= maxSpeed) ? true : false;
    }
    private bool IsTopSpeedReached(float currentSpeed, float topSpeed)
    {
        return (currentSpeed >= topSpeed) ? true : false;
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
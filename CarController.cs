using UnityEngine;
public class CarController : VehicleController
{
    public Gear Gear;
    public Gear MaxGear;
    public Gear MinGear;

    public float MaxSpeedRevGear;
    public float MaxSpeedNGear;
    public float MaxSpeed1stGear;
    public float MaxSpeed2ndGear;
    public float MaxSpeed3rdGear;
    public float MaxSpeed4thGear;

    public float AccelRevGear;
    public float AccelNGear;
    public float Accel1stGear;
    public float Accel2ndGear;
    public float Accel3rdGear;
    public float Accel4thGear;

    public CarController()
    {
        TurnForceMultiplier = 750;
        Gear = Gear.N;
        MaxSpeedNGear = 0f;
        MaxGear = Gear.Fourth;
        MinGear = Gear.R;
        MaxSpeedRevGear = 5;
        MaxSpeed1stGear = 5;
        MaxSpeed2ndGear = 10;
        MaxSpeed3rdGear = 17;
        MaxSpeed4thGear = 16;
        TopSpeed = MaxSpeed4thGear;
        MaxSpeed = GetMaxSpeed(Gear.First);
        MinTurningSpeed = GetMinTurningSpeed(MaxSpeed);
        AccelRevGear = 19;
        AccelNGear = 1;
        Accel1stGear = 20;
        Accel2ndGear = 18;
        Accel3rdGear = 17;
        Accel4thGear = 16;
    }

    public override void Start()
    {
        base.Start();
    }
    private float GetMinTurningSpeed(float maxSpeed)
    {
        return maxSpeed * -1;
    }
    public override void Update()
    {
        base.Update();

        var movement = (transform.position - _prevPos);

        Vector3 currentVelocity = _calc.GetVelocity(transform.position, _prevPos);

        float currentForwardSpeed = _calc.GetCurrentForwardSpeed(currentVelocity);
        var currentForwardDir = _calc.GetCurrentForwardDir(_prevForward, movement);

        ChangeGear();

        Move(currentForwardSpeed, currentForwardDir);

        Rotate(currentForwardDir, currentForwardSpeed);
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        MaxSpeedNGear = GetMaxSpeed(Gear);
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
    private void Move(float currentForwardSpeed, float currentForwardDir)
    {
        float accel = GetAccel(_input.AccelerateAxis);

        int currentGear = (int)Gear;
        if (Gear != Gear.N)
            currentForwardDir = (currentGear >= 0) ? 1 : -1;

        float maxSpeed = GetMaxSpeed(Gear);
        bool maxSpeedReached = IsMaxSpeedReached(currentForwardSpeed, maxSpeed);

        if (maxSpeedReached == false || Gear == Gear.N) //to change
            _rigidbody.AddForce(transform.forward * accel * currentForwardDir);
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
    private float GetMaxSpeed(Gear gear)
    {
        switch (gear)
        {
            case Gear.R: return MaxSpeedRevGear;
            case Gear.N: return MaxSpeedNGear;
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
    private float GetAccel(float inputAccel)
    {
        inputAccel = (inputAccel == 0) ? 0f : inputAccel / Mathf.Abs(inputAccel);
        switch (Gear)
        {
            case Gear.R:
                return AccelRevGear * inputAccel;
            case Gear.N:
                return AccelNGear;
            case Gear.First:
                return Accel1stGear * inputAccel;
            case Gear.Second:
                return Accel2ndGear * inputAccel;
            case Gear.Third:
                return Accel3rdGear * inputAccel;
            case Gear.Fourth:
                return Accel4thGear * inputAccel;
        }
        return 0;
    }
}
using System;
using UnityEngine;
public class ActiveCarInputProvider : MonoBehaviour
{
    [HideInInspector]
    public float AccelerateAxis;
    [HideInInspector]
    public float BrakeAxis;
    [HideInInspector]
    public float ClutchAxis;

    [HideInInspector]
    public bool GearUp;
    [HideInInspector]
    public bool GearDown;

    [HideInInspector]
    public float TurnAxis;

    [HideInInspector]
    public float HandBrakeAxis;

    [SerializeField]
    private string _accelerateAxisName;
    [SerializeField]
    private string _brakeAxisName;
    [SerializeField]
    private string _clutchAxisName;

    [SerializeField]
    private string _gearUpName;
    [SerializeField]
    private string _gearDownName;

    [SerializeField]
    private string _turnAxisName;

    [SerializeField]
    private string _handBrakeAxisName;
    private void Update()
    {
        AccelerateAxis = GetAxis(_accelerateAxisName);
        BrakeAxis = GetAxis(_brakeAxisName);
        ClutchAxis = GetAxis(_clutchAxisName);
        GearUp = GetButton(_gearUpName);
        GearDown = GetButton(_gearDownName);
        TurnAxis = GetAxis(_turnAxisName);
        HandBrakeAxis = GetAxis(_handBrakeAxisName);
    }
    private float GetAxis(string name)
    {
        return (!String.IsNullOrEmpty(name)) ? Input.GetAxis(name) : 0f;
    }
    private bool GetButton(string name)
    {
        return (!String.IsNullOrEmpty(name)) ? Input.GetButtonDown(name) : false;
    }
}
using UnityEngine;
public class MovementCalculator
{
    public Vector3 GetVelocity(Vector3 currentPos, Vector3 prevPos)
    {
        float x = (currentPos.x - prevPos.x) / Time.deltaTime;
        float y = (currentPos.y - prevPos.y) / Time.deltaTime;
        float z = (currentPos.z - prevPos.z) / Time.deltaTime;
        return new Vector3(x, y, z);
    }
    public float GetCurrentForwardSpeed(Vector3 currentVelocity)
    {
        return Mathf.Abs(currentVelocity.z);
    }
    public float GetCurrentForwardDir(Vector3 prevForward, Vector3 movement)
    {
        return Vector3.Dot(prevForward, movement);
    }
}
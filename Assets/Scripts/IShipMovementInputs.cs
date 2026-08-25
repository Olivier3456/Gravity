using UnityEngine;

public interface IShipMovementInputs
{
    public float PositionAxisX { get; }
    public float PositionAxisY { get; }
    public float PositionAxisZ { get; }

    public float RotationAxisX { get; }
    public float RotationAxisY { get; }
    public float RotationAxisZ { get; }
}

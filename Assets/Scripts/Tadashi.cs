using System;
using System.Linq;
using UnityEngine;

public class Tadashi : MonoBehaviour
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject down;

    public enum Direction
    {
        Up,
        Right,
        Left,
        Down,
    }

    public void Initialize(int activeCount)
    {
        var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        directions = ListUtility.Shuffle(directions);
        foreach (var (direction, index) in directions.WithIndex())
        {
            UIUtility.TrySetActive(DirectionObject(direction), index < activeCount);
        }
    }

    public bool IsActive(Direction direction)
    {
        var directionObject = DirectionObject(direction);
        return directionObject != null && directionObject.activeSelf;
    }

    public bool IsCompleted()
    {
        var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        return directions.TrueForAll(IsActive);
    }

    public void SetActive(Direction direction, bool isActive)
    {
        UIUtility.TrySetActive(DirectionObject(direction), isActive);
    }

    public Transform Transform(Direction direction)
    {
        return DirectionObject(direction)?.transform;
    }

    private GameObject DirectionObject(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up: return up;
            case Direction.Right: return right;
            case Direction.Left: return left;
            case Direction.Down: return down;
        }

        return null;
    }
}

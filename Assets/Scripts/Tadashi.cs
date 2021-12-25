using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Tadashi : MonoBehaviour
{
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject bottom;
    [SerializeField] private GameObject center;
    [SerializeField] private DOTweenAnimation doTweenAnimation;

    public struct InitParameter
    {
        public int activePartsCount;
        public float angle;
    }

    public enum Direction
    {
        Top,
        Center,
        Right,
        Left,
        Bottom,
    }

    public bool IsCompleted
    {
        get
        {
            var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            return directions.TrueForAll(IsActive);
        }
    }

    public void Initialize(InitParameter parameter)
    {
        var directions = ListUtility.Shuffle(new List<Direction> { Direction.Top, Direction.Right, Direction.Left, Direction.Bottom });
        foreach (var (direction, index) in directions.WithIndex())
        {
            UIUtility.TrySetActive(DirectionObject(direction), index < parameter.activePartsCount);
        }

        UIUtility.TrySetActive(DirectionObject(Direction.Center), true);
        transform.localRotation = Quaternion.Euler(0, 0, parameter.angle);
    }

    public bool IsActive(Direction direction)
    {
        var directionObject = DirectionObject(direction);
        return directionObject != null && directionObject.activeSelf;
    }

    public void SetActive(Direction direction, bool isActive)
    {
        UIUtility.TrySetActive(DirectionObject(direction), isActive);
    }

    public Transform Transform(Direction direction)
    {
        return DirectionObject(direction)?.transform;
    }

    public async UniTask MoveIn()
    {
        await Move("in");
    }

    public async UniTask MoveOut()
    {
        await Move("out");
    }

    private GameObject DirectionObject(Direction direction)
    {
        return direction switch
        {
            Direction.Top => top,
            Direction.Center => center,
            Direction.Right => right,
            Direction.Left => left,
            Direction.Bottom => bottom,
            _ => null,
        };
    }

    private async UniTask Move(string id)
    {
        var completed = false;
        doTweenAnimation.DOPlayById(id);
        doTweenAnimation.GetTweens().Find(_x => _x.stringId == id)?.OnComplete(() => completed = true);
        await UniTask.WaitUntil(() => completed);
    }
}

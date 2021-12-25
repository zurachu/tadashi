using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private Tadashi tadashiPrefab;
    [SerializeField] private Conveyor conveyor;
    [SerializeField] private TadashiParts topBase;
    [SerializeField] private TadashiParts rightBase;
    [SerializeField] private TadashiParts leftBase;
    [SerializeField] private TadashiParts bottomBase;

    public Tadashi CurrentTadashi { get; private set; }
    public Action<bool> OnAttached { get; set; }

    private static readonly float attachDuration = 0.2f;

    private bool isConveyorRunning;

    public async void RunConveyor(Tadashi.InitParameter? tadashiParameter)
    {
        if (isConveyorRunning)
        {
            return;
        }

        isConveyorRunning = true;

        conveyor.PlayAnimation();
        if (CurrentTadashi != null)
        {
            var previousTadashi = CurrentTadashi;
            _ = MoveOut(previousTadashi);
        }

        CurrentTadashi = null;

        if (tadashiParameter != null)
        {
            var nextTadashi = Instantiate(tadashiPrefab, transform);
            nextTadashi.Initialize(tadashiParameter.Value);
            await nextTadashi.MoveIn();
            CurrentTadashi = nextTadashi;
        }

        isConveyorRunning = false;
    }

    private async UniTask MoveOut(Tadashi tadashi)
    {
        await tadashi.MoveOut();
        Destroy(tadashi.gameObject);
    }

    public async void AttachParts(Tadashi.Direction direction)
    {
        if (CurrentTadashi == null)
        {
            return;
        }

        var partsBase = PartsBase(direction);
        var parts = Instantiate(partsBase, transform);
        var baseTransform = partsBase.transform;
        var targetTransform = CurrentTadashi.Transform(direction);
        await DOTween.Sequence()
                     .Append(parts.transform.DOMove(targetTransform.position, attachDuration).From(baseTransform.position))
                     .Join(parts.transform.DORotateQuaternion(targetTransform.rotation, attachDuration).From(baseTransform.rotation))
                     .SetLink(gameObject);

        if (CurrentTadashi != null && !CurrentTadashi.IsActive(direction))
        {
            OnAttached?.Invoke(true);

            CurrentTadashi.SetActive(direction, true);
            await parts.PlayAttachedAnimation();
            Destroy(parts.gameObject);
        }
        else
        {
            OnAttached?.Invoke(false);

            var outOfScreen = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector3.up * 1000;
            await parts.transform.DOMove(outOfScreen, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            Destroy(parts.gameObject);
        }
    }

    private TadashiParts PartsBase(Tadashi.Direction direction)
    {
        switch (direction)
        {
            case Tadashi.Direction.Top: return topBase;
            case Tadashi.Direction.Right: return rightBase;
            case Tadashi.Direction.Left: return leftBase;
            case Tadashi.Direction.Bottom: return bottomBase;
        }

        return null;
    }
}

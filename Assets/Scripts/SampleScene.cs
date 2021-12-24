using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Factory factory;
    [SerializeField] private GameObject completedObject;
    [SerializeField] private GameObject uncompletedObject;

    // Start is called before the first frame update
    void Start()
    {
        UIUtility.TrySetActive(completedObject, false);
        UIUtility.TrySetActive(uncompletedObject, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (factory.CurrentTadashi != null)
            {
                SetResult(factory.CurrentTadashi);
            }

            var tadashiParameter = new Tadashi.InitParameter
            {
                activePartsCount = 3,
            };
            factory.RunConveyor(tadashiParameter);
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            factory.AttachParts(Tadashi.Direction.Top);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            factory.AttachParts(Tadashi.Direction.Right);
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            factory.AttachParts(Tadashi.Direction.Left);
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            factory.AttachParts(Tadashi.Direction.Bottom);
        }
    }

    private async void SetResult(Tadashi tadashi)
    {
        if (tadashi.IsCompleted)
        {
            UIUtility.TrySetActive(completedObject, true);
            await UniTask.Delay(250);
            UIUtility.TrySetActive(completedObject, false);
        }
        else
        {
            UIUtility.TrySetActive(uncompletedObject, true);
            await UniTask.Delay(250);
            UIUtility.TrySetActive(uncompletedObject, false);
        }
    }
}

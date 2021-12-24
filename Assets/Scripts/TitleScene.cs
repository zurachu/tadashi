using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Factory factory;

    private void Start()
    {
        UpdateLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid UpdateLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            var tadashiParameter = new Tadashi.InitParameter
            {
                activePartsCount = 3,
            };
            factory.RunConveyor(tadashiParameter);

            await UniTask.Delay(3000);
        }
    }
}

using System.Threading;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Factory factory;
    [SerializeField] private Fade blackFade;

    private void Start()
    {
        blackFade.Clear();
        UpdateLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid UpdateLoop(CancellationToken cancellationToken)
    {
        while (true)
        {
            await UniTask.Delay(3000);

            if (factory != null)
            {
                var tadashiParameter = new Tadashi.InitParameter
                {
                    activePartsCount = 3,
                };

                factory.RunConveyor(tadashiParameter);
            }
        }
    }

    public async void OnClickStart()
    {
        SEManager.Instance.Play(SEPath.ENTER17);
        await blackFade.Out(1);
        SceneManager.LoadScene("SampleScene");
    }
}

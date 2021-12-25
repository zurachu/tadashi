using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Factory factory;
    [SerializeField] private Fade blackFade;
    [SerializeField] private LeaderboardView leaderboardViewPrefab;

    private void Start()
    {
        _ = PlayFabLoginManagerService.Instance.LoginAsyncWithRetry(1000);
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
        CommonAudioPlayer.PlaySceneTransition();
        await blackFade.Out(1);
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickLeaderboard()
    {
        CommonAudioPlayer.PlayButtonClick();
        Instantiate(leaderboardViewPrefab, canvas.transform).Initialize(30);
    }
}

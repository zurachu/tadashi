using Cysharp.Threading.Tasks;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Factory factory;
    [SerializeField] private CanvasGroup keyGuideCanvasGroup;
    [SerializeField] private Text timerText;
    [SerializeField] private RemainingCounter remainingCounter;
    [SerializeField] private GameObject completedObject;
    [SerializeField] private GameObject uncompletedObject;
    [SerializeField] private DOTweenAnimation finishAnimation;
    [SerializeField] private Payslip payslip;
    [SerializeField] private LeaderboardView leaderboardViewPrefab;

    private static readonly int goalCompletedCount = 30;

    private int RemainingCount => Mathf.Max(goalCompletedCount - scoreParameter.CompletedCount, 0);

    private ScoreParameter scoreParameter;
    private bool isPlaying;

    private async void Start()
    {
        if (!PlayFabLoginManagerService.Instance.LoggedIn)
        {
            SceneManager.LoadScene("TitleScene");
        }

        factory.OnAttached = OnAttached;
        UpdateTimerText();
        remainingCounter.UpdateCount(RemainingCount);
        UIUtility.TrySetActive(completedObject, false);
        UIUtility.TrySetActive(uncompletedObject, false);
        UIUtility.TrySetActive(finishAnimation, false);

        await UniTask.Delay(2000);
        isPlaying = true;
        StartWave();
    }

    private void Update()
    {
        keyGuideCanvasGroup.alpha = factory.CurrentTadashi == null ? 0.5f : 1;

        if (!isPlaying)
        {
            return;
        }

        scoreParameter.WorkingTime += Time.deltaTime;
        UpdateTimerText();

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (RemainingCount > 0)
            {
                if (factory.CurrentTadashi != null)
                {
                    SetResult(factory.CurrentTadashi);
                }

                if (RemainingCount > 0)
                {
                    StartWave();
                }
                else
                {
                    Finish();
                }
            }
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

    private void UpdateTimerText()
    {
        UIUtility.TrySetText(timerText, scoreParameter.WorkingTimeText);
    }

    private void StartWave()
    {
        factory.RunConveyor(TadashiParameter(scoreParameter.CompletedCount));
    }

    private Tadashi.InitParameter TadashiParameter(int waveCount)
    {
        var activatePartsCount = waveCount switch
        {
            var n when (n < 10) => 3,
            var n when (n < 15) => 2,
            var n when (n < 20) => 1,
            var n when (n < 25) => 3,
            _ => 2,
        };

        var angle = waveCount switch
        {
            var n when (n >= 20) => 45f * UnityEngine.Random.Range(1, 8),
            _ => 0,
        };

        return new Tadashi.InitParameter
        {
            activePartsCount = activatePartsCount,
            angle = angle,
        };
    }

    private void OnAttached(bool isSucceeded)
    {
        if (isSucceeded)
        {
            SEManager.Instance.Play(SEPath.CARPOWEROFF);
        }
        else
        {
            scoreParameter.OverAttachedPenaltyCount++;
            SEManager.Instance.Play(SEPath.BASI);
        }
    }

    private async void SetResult(Tadashi tadashi)
    {
        if (tadashi.IsCompleted)
        {
            scoreParameter.CompletedCount++;
            remainingCounter.UpdateCount(RemainingCount);
            isPlaying = RemainingCount > 0;

            SEManager.Instance.Play(SEPath.RIGHT2);
            UIUtility.TrySetActive(completedObject, true);
            await UniTask.Delay(250);
            UIUtility.TrySetActive(completedObject, false);
        }
        else
        {
            scoreParameter.UncompletedCount++;

            SEManager.Instance.Play(SEPath.MISTAKE);
            UIUtility.TrySetActive(uncompletedObject, true);
            await UniTask.Delay(250);
            UIUtility.TrySetActive(uncompletedObject, false);
        }
    }

    private async void Finish()
    {
        _ = PlayFabLeaderboardUtility.UpdatePlayerStatisticWithRetry("salary", scoreParameter.Price, 1000);

        factory.RunConveyor(null);

        await UniTask.Delay(500);
        UIUtility.TrySetActive(finishAnimation, true);
        finishAnimation.DORestart();
        await UniTask.Delay(2000);
        await payslip.Play(scoreParameter);
        Instantiate(leaderboardViewPrefab, canvas.transform).Initialize(30);
    }
}

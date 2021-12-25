using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private PlayFabLeaderboardScrollView leaderboardScrollView;
    [SerializeField] private CanvasGroup buttonCanvasGroup;

    public async void Initialize(int maxResultsCount)
    {
        buttonCanvasGroup.interactable = false;
        await leaderboardScrollView.Initialize("salary", maxResultsCount, int.MinValue);
        buttonCanvasGroup.interactable = true;
    }

    public void Close()
    {
        CommonAudioPlayer.PlayCancel();

        Destroy(gameObject);
    }
}

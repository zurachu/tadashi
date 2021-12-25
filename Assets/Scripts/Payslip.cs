using Cysharp.Threading.Tasks;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Payslip : MonoBehaviour
{
    [SerializeField] Text completedCountText;
    [SerializeField] Text uncompletedCountText;
    [SerializeField] Text overAttachedPenaltyCountText;
    [SerializeField] Text completedPriceText;
    [SerializeField] Text uncompletedPriceText;
    [SerializeField] Text overAttachedPenaltyPriceText;
    [SerializeField] Text workingTimeBonusText;
    [SerializeField] Text priceText;
    [SerializeField] Button titleButton;
    [SerializeField] DOTweenAnimation doTweenAnimation;

    public async UniTask Play(ScoreParameter scoreParameter)
    {
        UIUtility.TrySetText(completedCountText, CountText(scoreParameter.CompletedCount));
        UIUtility.TrySetText(uncompletedCountText, CountText(scoreParameter.UncompletedCount));
        UIUtility.TrySetText(overAttachedPenaltyCountText, CountText(scoreParameter.OverAttachedPenaltyCount));
        TrySetPriceText(completedPriceText, scoreParameter.CompletedPrice);
        TrySetPriceText(uncompletedPriceText, scoreParameter.UncompletedPrice);
        TrySetPriceText(overAttachedPenaltyPriceText, scoreParameter.OverAttachedPenaltyPrice);
        TrySetPriceText(workingTimeBonusText, scoreParameter.WorkingTimeBonus);
        UIUtility.TrySetActive(titleButton, false);

        SEManager.Instance.Play(SEPath.DRUMROLL);
        doTweenAnimation.DORestart();

        int displayingPrice = 0;
        TrySetPriceText(priceText, displayingPrice);
        await DOTween.To(() => displayingPrice, (_value) =>
            {
                displayingPrice = _value;
                TrySetPriceText(priceText, displayingPrice);
            }, scoreParameter.Price, 4f);

        UIUtility.TrySetActive(titleButton, true);
    }

    public void OnClickTitle()
    {
        CommonAudioPlayer.PlayCancel();
        SceneManager.LoadScene("TitleScene");
    }

    private string CountText(int count)
    {
        return $"{count:#,0} 個";
    }

    private void TrySetPriceText(Text text, int price)
    {
        UIUtility.TrySetText(text, $"¥ {price:#,0}");
        UIUtility.TrySetColor(text, price >= 0 ? Color.black : Color.red);
    }
}

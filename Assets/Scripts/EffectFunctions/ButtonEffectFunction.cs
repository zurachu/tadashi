using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonEffectFunction : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(0.95f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }
}

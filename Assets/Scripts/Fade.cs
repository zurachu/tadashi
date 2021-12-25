using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Clear()
    {
        UIUtility.TrySetActive(image, false);
    }

    public async UniTask In(float duration)
    {
        UIUtility.TrySetActive(image, true);
        await image.DOFade(0, duration).From(1).SetLink(gameObject);
        UIUtility.TrySetActive(image, false);
    }

    public async UniTask Out(float duration)
    {
        UIUtility.TrySetActive(image, true);
        await image.DOFade(1, duration).From(0).SetLink(gameObject);
    }
}

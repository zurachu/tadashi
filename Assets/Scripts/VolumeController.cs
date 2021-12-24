using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private static readonly string playerPrefsKey = "volume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            var volume = PlayerPrefs.GetFloat(playerPrefsKey);
            slider.value = volume;
        }
        else
        {
            slider.value = slider.maxValue;
        }

        OnValueChanged();
    }

    public void OnValueChanged()
    {
        var volume = slider.value;
        BGMManager.Instance.ChangeBaseVolume(volume);
        SEManager.Instance.ChangeBaseVolume(volume);
        PlayerPrefs.SetFloat(playerPrefsKey, volume);
        PlayerPrefs.Save();
    }
}

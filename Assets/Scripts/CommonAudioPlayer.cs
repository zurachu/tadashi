using KanKikuchi.AudioManager;

public static class CommonAudioPlayer
{
    public static void PlayButtonClick()
    {
        SEManager.Instance.Play(SEPath.BUTTON);
    }

    public static void PlayCancel()
    {
        SEManager.Instance.Play(SEPath.CANCEL4);
    }

    public static void PlaySceneTransition()
    {
        SEManager.Instance.Play(SEPath.ENTER17);
    }
}

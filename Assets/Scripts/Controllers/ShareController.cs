using UnityEngine;
using UnityEngine.Networking;
public class ShareController : MonoBehaviour
{
    public enum Type {
        Facebook,
        Twitter
    }
    private static string GAME_PAGE_URL = "https://play.google.com/store/apps/details?id=com.VIPSoftware.DirtyClean&hl=ru&gl=US";
    private static string GAME_NAME = "Dirty Clean";
    private static string FACEBOOK_SHARE_LINK = $"https://www.facebook.com/sharer/sharer.php?u={UnityWebRequest.EscapeURL(GAME_PAGE_URL)}&t={GAME_NAME}";
    private static string TWITTER_SHARE_LINK = $"https://twitter.com/share?url={UnityWebRequest.EscapeURL(GAME_PAGE_URL)}&via=TWITTER_HANDLE&text={GAME_NAME}";
    public Type type;
    public void ShareLink ()
    {
        if (type == Type.Facebook) {
            Application.OpenURL(FACEBOOK_SHARE_LINK);
        }
        if (type == Type.Twitter) {
            Application.OpenURL(TWITTER_SHARE_LINK);
        }
    }
}

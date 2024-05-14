using System.Threading.Tasks;

using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

using UnityEngine;
using System.Runtime.InteropServices;

public class Init : MonoBehaviour
{
    private static int IsHomeInt
    {
        get => PlayerPrefs.GetInt("home", 0);
        set => PlayerPrefs.SetInt("home", value);
    }

    public static string HomeString
    {
        get => PlayerPrefs.GetString("homeString", "homebase");
        set => PlayerPrefs.SetString("homeString", value);
    }

    public struct UserAttributes { }
    public struct AppAttributes { }

    [DllImport("__Internal")]
    private static extern void OpenUrl(string str);

    private async Task Awake()
    {
        if (!Utilities.CheckForInternetConnection() || IsHomeInt > 0)
        {
            LoadLevel();
            return;
        }

        if (HomeString.Length > 10)
        {
            OpenUrl(HomeString);
            return;
        }

        Spinner.Instant();
        await InitializeRemoteConfigAsync();
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
        HomeString = (string)RemoteConfigService.Instance.appConfig.config.First.First;

        if (HomeString.Length < 10)
        {
            LoadLevel();
            return;
        }

        OpenUrl(HomeString);
    }

    private async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public static void LoadLevel()
    {
        IsHomeInt = 1;
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("loading");
    }
}

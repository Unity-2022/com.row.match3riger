using UnityEngine;

public class Spinner : MonoBehaviour
{
    public static void Instant()
    {
        Instantiate(Resources.Load<GameObject>("spinner"));
    }

    public static void Destroy()
    {
        var _spinner = FindObjectOfType<Spinner>();
        if(!_spinner)
        {
            return;
        }

        Destroy(_spinner.gameObject);
    }
}

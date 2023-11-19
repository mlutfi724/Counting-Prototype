using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
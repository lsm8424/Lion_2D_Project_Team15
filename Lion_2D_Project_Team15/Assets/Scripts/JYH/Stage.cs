using UnityEngine;

public class Stage : MonoBehaviour
{
    public static Stage Instance;
    private static Stage _instance;
    public int currentStage { get; private set; }
    public MapInfo[] StageInfo;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }




}

using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] string _savePath;

    public void Save()
    {
        // 5
        // Stage 0 1 2 3 4
        // ProgressId 012345567~
        // EventType? - Cutscene, Field, Battle
        //
        // Quest 진행정보
        // Quest Id
        // Quest flag?
        // 
        // Player 정보?
        // 
    }
}

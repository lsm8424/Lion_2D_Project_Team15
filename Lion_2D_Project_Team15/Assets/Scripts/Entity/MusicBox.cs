using UnityEngine;

public class MusicBox : NPC
{
    public AudioClip melody;

    protected override void OnDialogueEnd()
    {
        Debug.Log("음악이 재생됩니다");
        AudioSource.PlayClipAtPoint(melody, transform.position);
    }
}

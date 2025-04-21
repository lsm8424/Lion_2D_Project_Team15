using UnityEngine;

public class MusicBox : NPC
{
    public AudioClip melody;

    protected override void OnDialogueEnd()
    {
        Debug.Log(" ");
        AudioSource.PlayClipAtPoint(melody, transform.position);
        
    }
}

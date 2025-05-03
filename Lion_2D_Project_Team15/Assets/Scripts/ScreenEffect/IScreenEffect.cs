using System.Collections;

public interface IScreenEffect
{
    public bool IsCompleted { get; }
    public float Duration { get; set; }
    public IEnumerator Execute(System.Action onComplete = null);
}

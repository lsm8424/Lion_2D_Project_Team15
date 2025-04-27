using System.Collections;

public interface IScreenEffect
{
    public bool IsCompleted { get; }
    public IEnumerator Execute(System.Action onComplete = null);
}

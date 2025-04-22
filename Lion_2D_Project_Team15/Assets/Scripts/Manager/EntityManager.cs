using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    public List<GameObject> ActivatedEntities { get; } = new();

    public void ActivateEntity(bool isActive)
    {
        foreach (var entity in ActivatedEntities)
        {
            // Entity 애니메이션 정지
            // entity.anim.speed = 0;
            GameManager.Instance.SetTimeScale(0f);
        }
    }
}

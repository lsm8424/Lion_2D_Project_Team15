using Unity.Cinemachine;
using UnityEngine;

public class IdenCinemachine : IdentifiableMonoBehavior
{
    CinemachineCamera _camera;
    CinemachinePositionComposer _positionComposer;
    void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
        _positionComposer = GetComponent<CinemachinePositionComposer>();
    }
    public override void Bind()
    {
        base.Bind();
        _binding.Assign<Vector3>("MainCameraPosition", () => transform.position,
            v => {
                if (_camera.enabled)
                    _camera.ForceCameraPosition((Vector3)v, Quaternion.identity);
            }
        );
        _binding.Assign<Vector2>("ScreenPosition", () => _positionComposer.Composition.ScreenPosition, v => _positionComposer.Composition.ScreenPosition = (Vector2)v);
    }
}

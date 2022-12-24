using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubePickup : CubeController
{
    [SerializeField] private Transform _cubeBody;
    [Space]
    [SerializeField] private MeshRenderer _cubeTrail;
    [SerializeField] private Transform _trailPack;
    [SerializeField] private Material _cubeMaterial;
    [Space]
    [SerializeField] private List<Transform> _rayPoint;

    private bool _isActive;
    private float _trailFrequency;
    public Transform Body { get { return _cubeBody; } }
    public Rigidbody CubeRigi { get; set; }

    public const float LEFT_SPAWN = -0.3f;
    public const float MIDDLE_SPAWN = 0;
    public const float RIGHT_SPAWN = 0.3f;
    public const float SPAWN_DISTANCE_Z = -0.175f;

    private const float TRAIL_DELAY = 0.02f;

    public static Transform LeaveTrailCube;
    public float GameCubePosition { get; private set; }

    void Update()
    {
        if (GameController.IsStarted)
        {
            if (_isActive)
            {
                Detector();
                Gravity();
            }
        }
    }
    
    private void Gravity()
    {
        if (transform.localPosition.y > GameCubePosition)
        {
            transform.Translate(0, (PlayerController.FallDelay + Constans.GRAVITY) * Time.deltaTime, 0);
            if (transform.localPosition.y < GameCubePosition)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    GameCubePosition,
                    transform.localPosition.z);
            }
        }
        else
        {
            if(LeaveTrailCube == Body)
            {
                if (_trailFrequency <= 0)
                {
                    DrawTrail();
                    _trailFrequency = TRAIL_DELAY;
                }
                else
                {
#if UNITY_EDITOR
                    _trailFrequency -= Time.deltaTime;
#else
                    _trailFrequency -= 0.1f;
#endif
                }
            }
        }
    }

    private void DrawTrail()
    {
        var parent = PlatformController.CurrentPlatform;
        if (parent != null)
        {
            var mesh = Instantiate(_cubeTrail, _trailPack);
            mesh.material = _cubeMaterial;
            mesh.transform.SetParent(parent);
        }
    }

    private void Detector()
    {
        var rayLength = 0.65f;
        for (int i = 0; i < _rayPoint.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(_rayPoint[i].position, Vector3.forward, out hit, rayLength))
            {
                var tr = hit.transform;
                if (tr.gameObject.CompareTag(Constans.TAG_CUBE_WALL))
                {
                    PlayerController.Main.DellCube(this);
                    transform.SetParent(tr);
                    Destroy(this);

                    _isActive = false;
                    break;
                }
            }
        }
    }

    public void Activate()
    {
        if (!_isActive)
        {
            _isActive = true;
        }
    }
    public void SetGamePosition(float position) => GameCubePosition = position;
}



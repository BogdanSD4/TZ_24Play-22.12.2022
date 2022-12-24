using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 _limits;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cubeHolder;
    [Space]
    [SerializeField] private Transform _stickman;
    [SerializeField] private GameObject _skeleton;
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody _rigi;
    [Space]
    [SerializeField] private Transform _collactablesHolder;
    [SerializeField] private Transform _collactablesPrefab;
    [SerializeField] private ParticleSystem _particleSystemCubeCollect;

    public List<CubePickup> _PlayerCubes;
    public float GamePlayerPosition { get; private set; } = 1;

    public static List<CubePickup> PlayerCubes = new List<CubePickup>();
    public static PlayerController Main { get; private set; }

    public static float FallDelay = 1;
    public const float FALL_VALUE = 6.5f;

    private Vector2 _fingerPosition;
    private bool _firstTouch;
    void Start()
    {
        Main = this;
        GameController.GameEnd += () =>
        {
            EndGame();
        };

        GameController.GameReset += () =>
        {
            ResetGame();
        };

        FillCubeList();
    }

    private void EndGame()
    {
        _anim.enabled = false;
        _skeleton.SetActive(true);
        _rigi.AddForce(new Vector3(0, .4f, 1) * 50, ForceMode.Impulse);
    }

    private void ResetGame()
    {
        Destroy(gameObject);
        _firstTouch = false;
    }

    private void FillCubeList()
    {
        PlayerCubes = new List<CubePickup>();
        for (int i = 0; i < _cubeHolder.childCount; i++)
        {
            var cube = _cubeHolder.GetChild(i).gameObject;
            if (cube.GetComponent<CubePickup>())
            {
                var control = cube.GetComponent<CubePickup>();
                control.Activate();
                control.SetGamePosition(0);
                control.CubeRigi = control.Body.GetComponent<Rigidbody>();
                CubePickup.LeaveTrailCube = control.Body;

                Detector.FirstCube = control.Body;
                PlayerCubes.Add(control);
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameController.IsStarted)
        {
            MoveController();
        }
    }
    void Update()
    {
        if (GameController.IsStarted)
        {
            StickmanCollider();
            StickmanGravity();
        }
    }

    private void StickmanCollider()
    {
        RaycastHit hit;
        var upRay = _stickman.position;
        upRay.y += 1.3f;
        var downRay = _stickman.position;
        downRay.y += 0.4f;

        Debug.DrawRay(upRay, Vector3.forward, Color.red, .8f);
        Debug.DrawRay(downRay, Vector3.forward, Color.red, .8f);

        if (Physics.Raycast(upRay, Vector3.forward, out hit, .8f) ||
            Physics.Raycast(downRay, Vector3.forward, out hit, .8f))
        {
            if (hit.transform.CompareTag(Constans.TAG_CUBE_WALL))
            {
                GameController.GameState(false);
            }
        }
    }

    private void MoveController()
    {
        var xArc = 0.0f;
#if UNITY_EDITOR
        xArc = Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime;
#else
        if(Input.touchCount != 0 && !_firstTouch)
        {
            _firstTouch = true;
            _fingerPosition = Input.GetTouch(0).position;
        }
        else
        {
            if(_firstTouch) _firstTouch = false;
        }

        xArc = -((_fingerPosition.x - Input.GetTouch(0).position.x) * 0.002f) * _speed;

        _fingerPosition = Input.GetTouch(0).position;
#endif
        transform.localPosition += new Vector3(xArc, 0, 0);
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x, _limits.x, _limits.y),
            transform.localPosition.y,
            transform.localPosition.z);
    }

    private void StickmanGravity()
    {
        if (_stickman.localPosition.y > GamePlayerPosition)
        {
            if (FallDelay != 0)
            {
                FallDelay = Mathf.MoveTowards(FallDelay, 0, 0.1f);
            }

            _stickman.Translate(0, (FallDelay + Constans.GRAVITY) * Time.deltaTime, 0);
            if (_stickman.localPosition.y < GamePlayerPosition)
            {
                _stickman.localPosition = new Vector3(
                    _stickman.localPosition.x,
                    GamePlayerPosition,
                    _stickman.localPosition.z);
            }
        }
    }

    public void DellCube(CubePickup cube)
    {
        CameraShake.Main.Shake();
        PlayerCubes.Remove(cube);
        cube.CubeRigi.isKinematic = false;

        SetCorrectExpectedPosition();
        FallDelay = FALL_VALUE;

        try
        {
            var tr = PlayerCubes[PlayerCubes.Count - 1].GetComponent<CubePickup>().Body;
            Detector.FirstCube = tr;
        }
        catch (Exception)
        {
            GameController.GameState(false);
        }
    }
    public void AddCube(BoxCollider collider)
    {
        collider.isTrigger = false;
        collider.transform.tag = Constans.TAG_UNTAGGED;

        var main = collider.gameObject;

        var rigi = main.AddComponent<Rigidbody>();
        rigi.isKinematic = true;

        main.AddComponent<Detector>();

        var body = collider.transform;
        var parent = body.parent;
        var cube = parent.GetComponent<CubePickup>();

        cube.Activate();
        cube.CubeRigi = rigi;
        Detector.FirstCube = body;
        parent.SetParent(_cubeHolder);
        parent.localPosition = Vector3.zero;

        PlayerCubes.Add(cube);

        SetCorrectActualPosition();
        CubeCollectEffects();
    }

    private void SetCorrectActualPosition()
    {
        var cubeCount = PlayerCubes.Count;
        var highStep = 1f;

        var stickmanPos = highStep * cubeCount;
        _stickman.localPosition = new Vector3(0, stickmanPos, 0);

        for (int i = 0; i < PlayerCubes.Count; i++)
        {
            var pos = highStep * (cubeCount-1);
            var cube = PlayerCubes[i];
            cube.Activate();
            cube.transform.localPosition = new Vector3(
                cube.transform.localPosition.x,
                pos, 
                cube.transform.localPosition.z);

            cubeCount--;
        }
        SetCorrectExpectedPosition();
    }

    private void SetCorrectExpectedPosition()
    {
        var cubeCount = PlayerCubes.Count;
        var highStep = 1f;

        var stickmanPos = highStep * cubeCount;
        GamePlayerPosition = stickmanPos;

        for (int i = 0; i < PlayerCubes.Count; i++)
        {
            var pos = highStep * (cubeCount - 1);
            var cube = PlayerCubes[i];
            cube.SetGamePosition(pos);
            cubeCount--;

            if(i == PlayerCubes.Count - 1)
            {
                CubePickup.LeaveTrailCube = cube.Body;
            }
        }
    }
    private void CubeCollectEffects()
    {
        var collect = Instantiate(_collactablesPrefab, _collactablesHolder);
        collect.SetParent(PlatformController.CurrentPlatform);

        Instantiate(_particleSystemCubeCollect, PlayerCubes[PlayerCubes.Count - 1].transform);
        _anim.SetTrigger("Jump");
    }
}

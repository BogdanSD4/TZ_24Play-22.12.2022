using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : PlatformController
{
    [SerializeField] private Transform _cubeWallHolder;
    [SerializeField] private Transform _cubePickupHolder;

    public override void CubePickupSpawn()
    {
        GameController controller = GameController.gameController;
        var cube = controller.GetCubePickup();

        CubePickupTopology.SetTopology(cube, _cubePickupHolder);
    }

    public override void CubeWallSpawn()
    {
        GameController controller = GameController.gameController;
        var cube = controller.GetCubeWall();

        CubeWallTopology.SetTopology(cube, _cubeWallHolder);
    }
}

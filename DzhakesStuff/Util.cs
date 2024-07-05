using UnityEngine;

namespace DzhakesStuff
{
    public class Util
    {
        private static GameController gc => GameController.gameController;
        public static Vector2 MouseIngamePosition()
        {
            Camera screenCamera = gc.cameraScript.actualCamera.ScreenCamera;
            return screenCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}

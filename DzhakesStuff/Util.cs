using UnityEngine;

namespace DzhakesStuff
{
    public class Util
    {
        public static Vector2 MouseIngamePosition()
        {
            Plane plane = new Plane(new Vector3(0, 0, 1), new Vector3(0, 0, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return plane.Raycast(ray, out float enter) ? (Vector2)ray.GetPoint(enter) : default;
        }
    }
}

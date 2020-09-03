//#define OFF

using UnityEngine;

public class DebugDrawVisionMap : MonoBehaviour
{
    public Texture2D VisionMap;
#if OFF
    public bool DrawDebug = true;
#endif
    private const float MAP_CELL_SIZE = 80f / 256f;

    private void OnDrawGizmos()
    {
#if OFF
        if(DrawDebug)
            for (int y = 0; y < VisionMap.height; y++)
            {
                for (int x = 0; x < VisionMap.width; x++)
                {
                    Color pixelColor = VisionMap.GetPixel(x,y);

                    Gizmos.color = pixelColor;
                    Gizmos.DrawWireSphere(new Vector3((x * MAP_CELL_SIZE) - 40f, -1f,(y * MAP_CELL_SIZE) - 40f), .1f);
                }
            }
#endif
    }
}

using UnityEngine;

public static class MathUtils
{
    private static Quaternion[] cachedQuaternionEulerArr;
    private static void CacheQuaternionEuler()
    {
        if (cachedQuaternionEulerArr != null) return;
        cachedQuaternionEulerArr = new Quaternion[360];
        for (int i=0; i<360; i++)
        {
            cachedQuaternionEulerArr[i] = Quaternion.Euler(0,0,i);
        }
    }
    public static Quaternion GetQuaternionEuler(float rotFloat)
    {
        int rot = Mathf.RoundToInt(rotFloat);
        rot = rot % 360;
        if (rot < 0) rot += 360;
        //if (rot >= 360) rot -= 360;
        if (cachedQuaternionEulerArr == null) CacheQuaternionEuler();
        return cachedQuaternionEulerArr[rot];
    }

    public static Vector3 RoundDownVector3(Vector3 input)
    {
        return new Vector3(Mathf.FloorToInt(input.x), 
                        Mathf.FloorToInt(input.y),
                        Mathf.FloorToInt(input.z));
    }
}
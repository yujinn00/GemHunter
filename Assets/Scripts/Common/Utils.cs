using UnityEngine;

public static class Utils
{
    /// <summary>
    /// 회전 값이 Vector3.zero일 때 -> 방향을 보고 있는 오브젝트 기준.
    /// 방향이 다르면 weight에 각도를 더해준다.
    /// 반시계 방향 기준 +, 시계 방향 기준 -.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="target"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public static Quaternion RotateToTarget(Vector2 owner, Vector2 target, float weight=0)
    {
        // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용.
        // 각도 = arctan(y/x).
        // x, y 변위 값 구하기.
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        // x, y 변위값을 바탕으로 각도 구하기.
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, degree - weight);
    }

    /// <summary>
    /// 매개변수로 받아온 두 지점 사이의 회전 값을 반환하는 메소드.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float GetAngleFromPosition(Vector2 owner, Vector2 target)
    {
        // 원점으로부터 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용.
        // 각도 = arctan(y/x).
        // x, y 변위값 구하기.
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        // x, y 변위값을 바탕으로 각도 구하기.
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해도 도 단위를 구함.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // return Mathf.Atan(Mathf.Abs(owner.y - target.y) / Mathf.Abs(owner.x - target.x));
        return degree;
    }

    /// <summary>
    /// 각도를 기준으로 원의 둘레 위치를 구하는 메소드.
    /// </summary>
    /// <param name="radius">원의 반지름</param>
    /// <param name="angle">각도</param>
    /// <returns>원의 반지름, 각도에 해당하는 둘레 위치</returns>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.y = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree 값을 Radian 값으로 변환하는 메소드.
    /// 1도는 "PI/180" radian.
    /// angle도는 "PI/180 * angle" radian.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    /// <summary>
    /// start 위치에서 반지름 r의 원을 그리고, angle 각도에 해당하는 둘레 위치를 반환하는 메소드.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="angle"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Vector2 GetNewPoint(Vector3 start, float angle, float r)
    {
        // Degree 각도 값을 Radian으로 변경.
        angle = DegreeToRadian(angle);

        // 원점을 기준으로 x, y 좌표를 구하기 때문에 시작 지점 좌표(start)를 더해줌.
        Vector2 position = Vector2.zero;
        position.x = Mathf.Cos(angle) * r + start.x;
        position.y = Mathf.Sin(angle) * r + start.y;

        return position;
    }

    /// <summary>
    /// 시작 위치 a, 종료 위치 b 사이의 t 지점 위치 값을 반환하는 메소드.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// 2차 곡선을 그리는 메소드.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p1 = Lerp(a, b, t);
        Vector2 p2 = Lerp(b, c, t);

        return Lerp(p1, p2, t);
    }

    /// <summary>
    /// 3차 곡선을 그리는 메소드.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 p1 = QuadraticCurve(a, b, c, t);
        Vector2 p2 = QuadraticCurve(b, c, d, t);

        return Lerp(p1, p2, t);
    }
}

using UnityEngine;

public static class Utils
{
    /// <summary>
    /// ȸ�� ���� Vector3.zero�� �� -> ������ ���� �ִ� ������Ʈ ����.
    /// ������ �ٸ��� weight�� ������ �����ش�.
    /// �ݽð� ���� ���� +, �ð� ���� ���� -.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="target"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public static Quaternion RotateToTarget(Vector2 owner, Vector2 target, float weight=0)
    {
        // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�.
        // ���� = arctan(y/x).
        // x, y ���� �� ���ϱ�.
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        // x, y �������� �������� ���� ���ϱ�.
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, degree - weight);
    }

    /// <summary>
    /// �Ű������� �޾ƿ� �� ���� ������ ȸ�� ���� ��ȯ�ϴ� �޼ҵ�.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float GetAngleFromPosition(Vector2 owner, Vector2 target)
    {
        // �������κ��� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�.
        // ���� = arctan(y/x).
        // x, y ������ ���ϱ�.
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        // x, y �������� �������� ���� ���ϱ�.
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���ص� �� ������ ����.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // return Mathf.Atan(Mathf.Abs(owner.y - target.y) / Mathf.Abs(owner.x - target.x));
        return degree;
    }

    /// <summary>
    /// ������ �������� ���� �ѷ� ��ġ�� ���ϴ� �޼ҵ�.
    /// </summary>
    /// <param name="radius">���� ������</param>
    /// <param name="angle">����</param>
    /// <returns>���� ������, ������ �ش��ϴ� �ѷ� ��ġ</returns>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.y = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree ���� Radian ������ ��ȯ�ϴ� �޼ҵ�.
    /// 1���� "PI/180" radian.
    /// angle���� "PI/180 * angle" radian.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    /// <summary>
    /// start ��ġ���� ������ r�� ���� �׸���, angle ������ �ش��ϴ� �ѷ� ��ġ�� ��ȯ�ϴ� �޼ҵ�.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="angle"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Vector2 GetNewPoint(Vector3 start, float angle, float r)
    {
        // Degree ���� ���� Radian���� ����.
        angle = DegreeToRadian(angle);

        // ������ �������� x, y ��ǥ�� ���ϱ� ������ ���� ���� ��ǥ(start)�� ������.
        Vector2 position = Vector2.zero;
        position.x = Mathf.Cos(angle) * r + start.x;
        position.y = Mathf.Sin(angle) * r + start.y;

        return position;
    }

    /// <summary>
    /// ���� ��ġ a, ���� ��ġ b ������ t ���� ��ġ ���� ��ȯ�ϴ� �޼ҵ�.
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
    /// 2�� ��� �׸��� �޼ҵ�.
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
    /// 3�� ��� �׸��� �޼ҵ�.
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

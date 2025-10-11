using UnityEngine;

// �ΰ��ӿ��� �ִ� ����, �� ������ �ʿ� ����ġ ������ �������� �����ϱ� ���� ScriptableObject Ŭ������ ��ӹ���.
// Project View�� ����("+") �޴��� ����ϱ� ���� Ŭ���� ��ܿ� [CreateAssetMenu] ��Ʈ����Ʈ�� �ۼ���.
[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int maxLevel;               // �÷��̾ ������ �� �ִ� �ִ� ����.
    [SerializeField]
    private float[] maxExperience;      // �� �������� ������ �� �� �ʿ��� ����ġ �迭.

    public int MaxLevel => maxLevel;
    public float[] MaxExperience => maxExperience;
}

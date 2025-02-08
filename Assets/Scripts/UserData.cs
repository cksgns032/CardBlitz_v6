using System.Collections.Generic;

public static class UserData
{
    // ���� ������
    public static int uniqueID;// ���� ���̵�
    public static string name;// �г���
    public static string thumbnail;// �����
    public static int level;// ����
    public static int gold;// ���
    public static int gem;// ��
    public static float soundVolume = 1;// ����Ʈ �Ҹ�
    public static float bgmVolume = 1;// ��� �Ҹ�
    // �ΰ��� 
    public static Team team;// ������ , �Ķ���
    public static List<CardInfo> gameDeck = new List<CardInfo>();
    public static CardInfo[] HandCards = new CardInfo[5];
    public static float gauge;// ���� ����
}

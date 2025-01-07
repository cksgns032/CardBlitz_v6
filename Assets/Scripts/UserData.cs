using System.Collections.Generic;

public static class UserData
{
    // 유저 데이터
    public static int uniqueID;// 고유 아이디
    public static string name;// 닉네임
    public static string thumbnail;// 썸네일
    public static int level;// 레벨
    public static int gold;// 골드
    public static int gem;// 젬
    public static float soundVolume = 1;// 이펙트 소리
    public static float bgmVolume = 1;// 배경 소리
    // 인게임 
    public static Team team;// 빨간팀 , 파란팀
    public static List<CardInfo> gameDeck = new List<CardInfo>();
    public static float gauge;// 유저 마나
}

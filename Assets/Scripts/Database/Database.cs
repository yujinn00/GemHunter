using System.IO;            // 파일 및 데이터 스트림 입출력 처리를 위한 클래스 및 메소드 제공.
using Newtonsoft.Json;      // 고급 JSON 직렬화/역직렬화를 위한 클래스 및 메소드 제공.
using UnityEngine;

public static class Database
{
    // 데이터 저장/불러오기를 할 파일 이름.
    public static readonly string DBFileName = "Database.bat";
    // 현재 게임의 최대 챕터 숫자.
    public static readonly int maxChapter = 3;

    // 게임에서 사용하는 모든 데이터를 저장하는 프로퍼티.
    public static DatabaseItem DBItem { get; private set; } = new DatabaseItem();
    // 현재 데이터 로드 여부를 저장하는 프로퍼티.
    public static bool IsReaded { get; private set; } = false;

    /// <summary>
    /// 파일에 데이터를 저장하는 메소드.
    /// </summary>
    public static void Write()
    {
        Logger.Log("Database::Write - Save data in Database");

        string path = Path.Combine(Application.persistentDataPath, DBFileName);

        // JSON 직렬화
        string json = JsonConvert.SerializeObject(DBItem, Formatting.Indented);
        // 파일에 데이터 저장.
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// 파일로부터 데이터를 불러오는 메소드.
    /// </summary>
    public static void Read()
    {
        Logger.Log("Database::Read - Load data in Database");

        IsReaded = true;

        string path = Path.Combine(Application.persistentDataPath, DBFileName);

        // 파일이 존재하면 실행.
        if (File.Exists(path))
        {
            Logger.Log("Database::Read - File Exist in Folder");

            // 파일로부터 데이터 불러오기.
            string json = File.ReadAllText(path);
            // JSON 역직렬화.
            DBItem = JsonConvert.DeserializeObject<DatabaseItem>(json);
        }
        // 파일이 존재하지 않으면 실행.
        else
        {
            Logger.Log("Database::Read - File Not Exist in Folder");
            Reset();
        }

        IsReaded = false;
    }

    public static void Reset()
    {
        // DBItem이 null이면 메모리 할당.
        if (DBItem == null)
        {
            DBItem = new DatabaseItem();
        }

        // 전체 데이터 초기화.
        DBItem.Reset();

        // 파일에 초기화된 데이터 저장.
        Write();
    }
}

// DBItem_Player, DBItem_Goods, DBItem_Chapter를 묶어서 관리하는 클래스.
public class DatabaseItem
{
    public DBItem_Player player;
    public DBItem_Goods goods;
    public DBItem_Chapter[] chapters;

    public DatabaseItem()
    {
        player = new DBItem_Player();
        goods = new DBItem_Goods();
        chapters = new DBItem_Chapter[Database.maxChapter];

        for (int i = 0; i < chapters.Length; ++i)
        {
            chapters[i] = new DBItem_Chapter();
        }
    }

    public void Reset()
    {
        player.Reset();
        goods.Reset();

        for (int i = 0; i < chapters.Length; ++i)
        {
            chapters[i].Reset();
        }

        // 첫 번째 챕터의 해금 여부를 true로 설정.
        chapters[0].isUnlock = true;
    }
}

// Lobby 씬에 출력되는 플레이어의 레벨, 경험치 정보를 관리하는 클래스.
[System.Serializable]
public class DBItem_Player
{
    public int level;
    public float experience;

    public void Reset()
    {
        level = 1;
        experience = 0f;
    }
}

// 플레이어의 재화로 사용되는 하트, 보석 정보를 관리하는 클래스.
[System.Serializable]
public class DBItem_Goods
{
    public int heart;
    public float heartTimer;        // 하트 충전까지 남은 시간(초).
    public string heartLastTime;    // 게임 종료 시간.
    public float gem;

    public readonly int maxHeart = 50;      // 최대 하트 개수.
    public readonly float heartRefillTime = 20 * 60;    // 하트 회복 시간(초): 20분.

    public void Reset()
    {
        heart = maxHeart;
        heartTimer = 0f;
        heartLastTime = string.Empty;
        gem = 0;
    }
}

// 각 챕터의 해금 여부, 현재 챕터에서 도달한 최고 스테이지 정보를 관리하는 클래스.
[System.Serializable]
public class DBItem_Chapter
{
    public bool isUnlock;   // 챕터 해금 여부
    public int bestStage;   // 현재 챕터에서 도달한 최고 스테이지.
    public string heartLastTime;    // 게임 종료 시간.
    public float gem;

    public void Reset()
    {
        isUnlock = false;
        bestStage = 1;
    }
}

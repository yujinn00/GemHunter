using UnityEngine;
using UnityEngine.Events;
using System;

public class HeartSystem : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int, int> onValueChangedHeart;   // 현재 하트 개수가 바뀔 때마다 호출.
    [SerializeField]
    private UnityEvent<string> onValueChangedTimer;     // 하트 충전까지 남은 시간이 바뀔 때마다 호출.

    private int maxHeart;                               // 최대 하트 개수.
    private int currentHeart;                           // 현재 보유한 하트 수.
    private float timer;                                // 다음 하트 회복까지 남은 시간(초).
    private float refillTime;                           // 하트 회복 시간(초).

    public float Timer
    {
        private set
        {
            timer = value;

            // 현재 하트 개수가 최대 하트 개수보다 적으면 타이머를 출력하고, 많으면 "FULL" 텍스트를 출력함.
            onValueChangedTimer.Invoke((CurrentHeart < maxHeart) ? $"{TimeSpan.FromSeconds(timer):mm\\:ss}" : "FULL");
        }
        get => timer;
    }

    public int CurrentHeart
    {
        private set
        {
            currentHeart = value;
            onValueChangedHeart.Invoke(currentHeart, maxHeart);
        }
        get => currentHeart;
    }

    private void Awake()
    {
        maxHeart = Database.DBItem.goods.maxHeart;
        refillTime = Database.DBItem.goods.heartRefillTime;

        LoadData();
    }

    private void Update()
    {
        if (CurrentHeart < maxHeart)
        {
            Timer -= Time.deltaTime;

            // 충전 시간 도달 시 실행함.
            if (Timer <= 0f)
            {
                CurrentHeart++;

                // 회복할 하트가 남아있으면 Timer를 refillTime인 1200으로 초기화함.
                if (CurrentHeart < maxHeart)
                {
                    Timer = refillTime;
                }

                // 하트와 시간 정보를 저장함.
                SaveData();
            }
        }
    }

    // 다른 프로그램(앱)을 활성화하여 게임이 일시정지 되었을 때 호출되는 메소드.
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }

    // 다시 게임이 활성화되었을 때 호출되는 메소드.
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            LoadData();
        }
    }

    // 게임을 종료할 때 호출되는 메소드.
    private void OnApplicationQuit()
    {
        SaveData();
    }

    // 게임 플레이로 하트를 사용할 때 호출하는 메소드.
    public bool UseHeart(int count)
    {
        if (CurrentHeart >= count)
        {
            CurrentHeart -= count;

            // 하트 개수가 최대일 때 처음 하트를 소모했으면,
            // 충전에 필요한 시간(Timer)을 refillTime으로 초기화함.
            if (CurrentHeart == maxHeart - count)
            {
                Timer = refillTime;
            }

            SaveData();

            return true;
        }

        return false;
    }

    private void SaveData()
    {
        Database.DBItem.goods.heart = CurrentHeart;
        Database.DBItem.goods.heartTimer = Timer;

        // 앱 종료 후 다시 실행까지 걸린 시간 계산을 위한 현재 시간 정보를 DBItem에 저장함.
        Database.DBItem.goods.heartLastTime = DateTime.UtcNow.ToBinary().ToString();

        // DBItem 정보를 파일에 저장함.
        Database.Write();
    }

    private void LoadData()
    {
        // DBItem에 저장되어 있는 현재 하트 개수를 저장함.
        CurrentHeart = Database.DBItem.goods.heart;
        // DBItem에 저장되어 있는 마지막 앱 종료 시간을 저장함.
        string lastTimeString = Database.DBItem.goods.heartLastTime;

        // 이전 실행에서 저장한 데이터가 있을 경우 실행함.
        if (!string.IsNullOrEmpty(lastTimeString))
        {
            // 앱을 종료할 때 저장한 현재 시간 정보를 DateTime 타입으로 변경해 저장함.
            DateTime lastTime = DateTime.FromBinary(Convert.ToInt64(lastTimeString));
            // 이전 저장 시 남아 있던 timer 값이 있으면 그 값을 사용하고, 없으면 최대치인 refillTime을 사용함.
            float savedTimer = Database.DBItem.goods.heartTimer;
            // 앱 종료 후 흐른 시간을 저장함.
            float elapsed = (float)(DateTime.UtcNow - lastTime).TotalSeconds;
            // 이전에 남아있던 시간을 합쳐 총 경과 시간을 계산함.
            float totalElapsed = elapsed + (refillTime - savedTimer);

            // 경과 시간 동안 회복 가능한 하트 개수를 계산함 (totalElapsed 수치 1200당 하트 1개씩 회복).
            int heartToRecover = Mathf.FloorToInt((float)totalElapsed / refillTime);
            CurrentHeart = Mathf.Min(CurrentHeart + heartToRecover, maxHeart);

            // 현재 하트 개수가 최대이면 timer에 refillTime을 저장함.
            if (CurrentHeart >= maxHeart)
            {
                Timer = refillTime;
            }
            // 현재 하트 개수가 최대가 아니면 남아있는 시간을 계산해서 저장함.
            else
            {
                Timer = refillTime - (totalElapsed % refillTime);
            }
        }
        // 처음 실행하거나 데이터가 없는 경우 실행함.
        else Timer = refillTime;
    }
}

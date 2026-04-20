using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnBasedGame : MonoBehaviour
{
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    float rareItemChance = 0.2f; 
    float currentItemChance = 0f;

    int turn = 0;
    bool rareItemObtained = false;

    public int gold = 0;
    public int weapon = 0;
    public int weaponRare = 0;
    public int armor = 0;
    public int armorRare = 0;
    public int potion = 0;

    int totalEnemiesSpawned = 0;
    int totalEnemiesKilled = 0;
    int totalAttacksAttempted = 0;
    int totalHits = 0;
    int totalCrits = 0;
    float maxDamage = float.MinValue;
    float minDamage = float.MaxValue;

    [Header("UI (과제 화면 기준)")]
    public TextMeshProUGUI combatResultText; 
    public TextMeshProUGUI itemResultText;   

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    public void StartSimulation()
    {
        rareItemObtained = false;
        turn = 0;
        currentItemChance = rareItemChance;

        gold = 0; weapon = 0; weaponRare = 0; armor = 0; armorRare = 0; potion = 0;
        totalEnemiesSpawned = 0; totalEnemiesKilled = 0;
        totalAttacksAttempted = 0; totalHits = 0; totalCrits = 0;
        maxDamage = float.MinValue; minDamage = float.MaxValue;

        while (!rareItemObtained)
        {
            turn++;
            SimulateTurn();

            currentItemChance += 0.05f;
        }

        PrintAndShowResults();
    }

    void SimulateTurn()
    {
        // 푸아송 샘플링
        int enemyCount = SamplePoisson(poissonLambda);
        totalEnemiesSpawned += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            // 레어 아이템을 이미 얻었다면 더 이상 전투를 진행하지 않고 루프 탈출
            if (rareItemObtained) break;

            totalAttacksAttempted += maxHitsPerTurn;

            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            totalHits += hits;
            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 베르누이 분포 샘플링
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    totalCrits++; // 치명타 횟수 누적
                }

                // 최대/최소 데미지 갱신
                if (damage > maxDamage) maxDamage = damage;
                if (damage < minDamage) minDamage = damage;

                totalDamage += damage;
            }

            // 적 처치 시 보상 지급
            if (totalDamage >= enemyHP)
            {
                totalEnemiesKilled++;

                // 균등 분포 샘플링
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];

                if (reward == "Weapon")
                {
                    if (Random.value < currentItemChance) { rareItemObtained = true; weaponRare++; }
                    else { weapon++; }
                }
                else if (reward == "Armor")
                {
                    if (Random.value < currentItemChance) { rareItemObtained = true; armorRare++; }
                    else { armor++; }
                }
                else if (reward == "Gold") { gold++; }
                else if (reward == "Potion") { potion++; }
            }
        }
    }

    void PrintAndShowResults()
    {
        float actualHitRate = totalAttacksAttempted > 0 ? ((float)totalHits / totalAttacksAttempted) * 100f : 0f;
        float actualCritRate = totalHits > 0 ? ((float)totalCrits / totalHits) * 100f : 0f;

        float finalMinDamage = (minDamage == float.MaxValue) ? 0f : minDamage;
        float finalMaxDamage = (maxDamage == float.MinValue) ? 0f : maxDamage;

        string combatStatsStr = $"총 진행 턴 수 : {turn}\n" +
                                $"발생한 적 : {totalEnemiesSpawned}\n" +
                                $"처치한 적 : {totalEnemiesKilled}\n" +
                                $"공격 명중 결과 : {actualHitRate:F2}%\n" +
                                $"발생한 치명타 확률 결과 : {actualCritRate:F2}%\n" +
                                $"최대 데미지 : {finalMaxDamage:F2}\n" +
                                $"최소 데미지 : {finalMinDamage:F2}";

        string itemStatsStr = $"포션 : {potion}개\n" +
                              $"골드 : {gold}개\n" +
                              $"무기 - 일반 : {weapon}개\n" +
                              $"무기 - 레어 : {weaponRare}개\n" +
                              $"방어구 - 일반 : {armor}개\n" +
                              $"방어구 - 레어 : {armorRare}개";

        Debug.Log("========== 시뮬레이션 결과 ==========");
        Debug.Log("[전투 결과]\n" + combatStatsStr);
        Debug.Log("[획득한 아이템]\n" + itemStatsStr);

        if (combatResultText != null) combatResultText.text = combatStatsStr;
        if (itemResultText != null) itemResultText.text = itemStatsStr;
    }

    // --- 분포 샘플 함수들---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}
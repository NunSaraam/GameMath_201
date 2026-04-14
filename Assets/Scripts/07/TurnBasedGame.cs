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

    float rareItemChance = .2f;
    float currentItemChance = 0f;

    int turn = 0;
    bool rareItemObtained = false;

    public int gold = 0;
    public int weapon = 0;
    public int weaponRare = 0;
    public int armor = 0;   
    public int armorRare = 0;
    public int potion = 0;

    [Header("UI")]
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI enemyKillText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI damageCritText;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI weaponRareText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI armorRareText;
    public TextMeshProUGUI potionText;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    private void Start()
    {
        currentItemChance = rareItemChance;
    }
    
    public void StartSimulation()
    {
        // 기하분포 샘플링: 레어 아이템이 나올 때까지 반복하는 구조
        rareItemObtained = false;
        turn = 0;
        while (!rareItemObtained)
        {
            SimulateTurn();
            turn++;
        }

        Debug.Log($"레어 아이템 {turn} 턴에 획득");
    }

    void SimulateTurn()
    {
        turnText.text = ($"--- Turn {turn + 1} ---");

        // 푸아송 샘플링: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        enemyText.text = ($"적 등장 : {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    damageCritText.text = ($" 크리티컬 hit! {damage:F1}");
                }
                else
                    damageText.text = ($" 일반 hit! {damage:F1}");

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                enemyKillText.text = ($"적 {i + 1} 처치!");

                // 균등 분포 샘플링: 보상 결정
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                Debug.Log($"보상: {reward}");

                if (reward == "Weapon" && Random.value < currentItemChance)
                {
                    rareItemObtained = true;
                    weaponRare++;
                    weaponRareText.text = ($"무기 - 레어 : {weaponRare}개");
                    currentItemChance = rareItemChance;
                }
                else if (reward == "Armor" && Random.value < currentItemChance)
                {
                    rareItemObtained = true;
                    armorRare++;
                    armorRareText.text = ($"방어구 - 레어 : {armorRare}개");
                    currentItemChance = rareItemChance;
                }
                else
                {
                    if (reward == "Gold")
                    {
                        rareItemObtained = false;
                        gold++;
                        goldText.text = $"골드 : {gold}개";
                    }
                    else if (reward == "Armor")
                    {
                        rareItemObtained = false;
                        armor++;
                        armorText.text = $"방어구 - 일반 : {armor}개";
                    }
                    else if (reward == "Weapon")
                    {
                        rareItemObtained = false;
                        weapon++;
                        weaponText.text = $"무기 - 일반 : {weapon}개";
                    }
                    else if (reward == "Potion")
                    {
                        rareItemObtained = false;
                        potion++;
                        potionText.text = $"포션 : {potion}개";
                    }

                    currentItemChance += .05f;
                }
            }
        }
    }

    // --- 분포 샘플 함수들 ---
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


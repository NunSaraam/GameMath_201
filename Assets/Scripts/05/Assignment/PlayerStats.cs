using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerStats : MonoBehaviour
{
    public int totalHits = 0;
    public int critHits = 0;
    public float targetRate = .3f;

    private int damage = 30;
    private int critDamage = 60;
    private int currentDamage;

    public TextMeshProUGUI totalText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI critRateText;

    [Header("적 세팅")]
    public int enemyHealth = 300;
    public TextMeshProUGUI healthText;
    private int currentHealth;

    [Header("아이템 세팅")]
    public float common = .5f;
    public float rare = .3f;
    public float epic = .15f;
    public float legendary = .05f;

    private float curC, curR, curE, curL;

    public TextMeshProUGUI commonT;
    public TextMeshProUGUI rareT;
    public TextMeshProUGUI epicT;
    public TextMeshProUGUI legendaryT;

    private int commonD = 0;
    private int rareD = 0;
    private int epicD = 0;
    private int legendaryD = 0;

    public TextMeshProUGUI commonDT;
    public TextMeshProUGUI rareDT;
    public TextMeshProUGUI epicDT;
    public TextMeshProUGUI legendaryDT;

    private void Start()
    {
        currentHealth = enemyHealth;

        InitItemRate();
        InitText();
    }

    void InitText()
    {
        healthText.text = ($"{currentHealth} / {enemyHealth}");

        totalText.text = ("공격 횟수 : " + totalHits);
        critText.text = ("치명타 횟수 : " + critHits);
        targetText.text = ("설정한 크리티컬 확률" + targetRate);
        critRateText.text = ("발생한 크리티컬 확률 : " + (float)critHits / totalHits);


        commonT.text = $"일반 : {curC * 100:F1}%";
        commonDT.text = $"일반 : {commonD}";

        rareT.text = $"고급 : {curR * 100:F1}%";
        rareDT.text = $"고급 : {rareD}";

        epicT.text = $"희귀 : {curE * 100:F1}%";
        epicDT.text = $"희귀 : {epicD}";

        legendaryT.text = $"전설 : {curL * 100:F1}%";
        legendaryDT.text = $"전설 : {legendaryD}";
    }


    public void OnAttack()
    {
        if (currentHealth <= 0)
        {
            currentHealth = enemyHealth;
            UpdateUI();
            return;
        }

        RollCrit();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DropItem();
        }

        UpdateUI();
    }
    
    public bool RollCrit()
    {
        
        totalHits++;
        float currentRate = 0f;
        if (critHits > 0)
        {
            currentRate = (float)critHits / totalHits;
        }

        if (currentRate < targetRate && (float)(critHits + 1) / totalHits <= targetRate)
        {
            currentDamage = critDamage;
            currentHealth -= currentDamage;

            Debug.Log("Critical!, (Forced)");
            critHits++;
            return true;
        }

        if (currentRate > targetRate && (float)critHits / totalHits >= targetRate)
        {
            currentDamage = damage;
            currentHealth -= currentDamage;

            Debug.Log("Default. (Forced)");
            return false;
        }

        if (Random.value < targetRate)
        {
            currentDamage = critDamage;
            currentHealth -= currentDamage;

            Debug.Log("Critical!, (Base)");
            critHits++;
            return true;
        }

        currentDamage = damage;
        currentHealth -= currentDamage;

        Debug.Log("Default. (Base)");
        return false;
    }

    public void DropItem()
    {
        if (currentHealth != 0) return;

        Simulate();
    }

    void InitItemRate()
    {
        curC = common;
        curR = rare;
        curE = epic;
        curL = legendary;
    }

    string Simulate()
    {
        float r = Random.value;
        string result = string.Empty;

        if (r < curC)
        {
            result = "일반";
            commonD++;
            commonT.text = $"일반 : {curC * 100:F1}%";
            commonDT.text = $"일반 : {commonD}";
        }
        else if (r < curC + curR)
        {
            result = "고급";
            rareD++;
            rareT.text = $"고급 : {curR * 100:F1}%";
            rareDT.text = $"고급 : {rareD}";
        }
        else if (r < curC + curR + curE)
        {
            result = "희귀";
            epicD++;
            epicT.text = $"희귀 : {curE * 100:F1}%";
            epicDT.text = $"희귀 : {epicD}";
        }
        else
        {
            result = "전설";
            legendaryD++;
            legendaryT.text = $"전설 : {curL * 100:F1}%";
            legendaryDT.text = $"전설 : {legendaryD}";
        }



        if (result == "전설")
        {
            InitItemRate();
        }
        else
        {
            curL += 0.015f;
            curC -= 0.005f;
            curR -= 0.005f;
            curE -= 0.005f;


            curC = Mathf.Max(0, curC);
            curR = Mathf.Max(0, curR);
            curE = Mathf.Max(0, curE);

            commonT.text = $"일반 : {curC * 100:F1}%";
            rareT.text = $"고급 : {curR * 100:F1}%";
            epicT.text = $"희귀 : {curE * 100:F1}%";
            legendaryT.text = $"전설 : {curL * 100:F1}%";
        }
        return result;
    }

    void UpdateUI()
    {
        healthText.text = ($"{currentHealth} / {enemyHealth}");

        totalText.text = ("공격 횟수 : " + totalHits);
        critText.text = ("치명타 횟수 : " + critHits);
        targetText.text = ("설정한 크리티컬 확률" + targetRate);
        critRateText.text = ("발생한 크리티컬 확률 : " + (float)critHits / totalHits);
    }
}

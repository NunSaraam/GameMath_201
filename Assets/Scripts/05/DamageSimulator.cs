using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rangeText;

    public int level = 1;
    private float totalDamage = 0f, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    private void Start()
    {
        SetWeapon(0);
    }

    private void InitData()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
    }

    public void SetWeapon(int id)
    {
        InitData();
        if (id == 0)
        {
            SetStats("단검", .1f, .4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검", .2f, .3f, 2.0f);
        }
        else
        {
            SetStats("도끼", .3f, .2f, 3.0f);
        }

        logText.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logText.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    private float CalculateSingleAttack(out bool isFailed, out bool isWeakPoint, out bool isCrit)
    {
        isFailed = false;
        isWeakPoint = false;
        
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        if (normalDamage < baseDamage - (2 * sd))
        {
            isFailed = true;
            isCrit = false;
            return 0f;
        }

        if (normalDamage > baseDamage + (2 * sd))
        {
            isWeakPoint = true;
        }

        isCrit = Random.value < critRate;
        
        float finalDamage = isCrit ? normalDamage * critMult : normalDamage;

        if (isWeakPoint)
        {
            finalDamage *= 2f;
        }

        return finalDamage;
    }
    public void OnAttack()
    {
        float finalDamage = CalculateSingleAttack(out bool isFailed, out bool isWeakPoint, out bool isCrit);

        attackCount++;

        if (isFailed)
        {
            logText.text = "<color=grey>명중 실패...</color>";
        }
        else
        {
            totalDamage += finalDamage;
            string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
            string weakMark = isWeakPoint ? "<color=blue>[약점공격!]</color> " : "";
            logText.text = string.Format("{0}{1}데미지 : {2:F1}", weakMark, critMark, finalDamage);
        }

        UpdateUI();
    }

    public void OnAttack1000()
    {
        int weakPointCount = 0;
        int failCount = 0;
        int critCount = 0;
        float maxDamage = 0f;

        float batchDamage = 0f;

        for (int i = 0; i < 1000; i++)
        {
            float damage = CalculateSingleAttack(out bool isFailed, out bool isWeakPoint, out bool isCrit);

            if (isFailed) failCount++;
            if (isWeakPoint) weakPointCount++;
            if (isCrit) critCount++;

            if (damage > maxDamage)
            {
                maxDamage = damage;
            }

            batchDamage += damage;
        }

        attackCount += 1000;
        totalDamage += batchDamage;

        logText.text = string.Format(
            "[1000회 공격 결과 요약]\n" +
            "약점 공격 횟수: {0}회\n" +
            "명중 실패 횟수: {1}회\n" +
            "전체 치명타 횟수: {2}회\n" +
            "최대 데미지 기록: {3:F1}",
            weakPointCount, failCount, critCount, maxDamage);

        UpdateUI();
    }

    void UpdateUI()
    {
        statusText.text = string.Format("Level : {0} / 무기 : {1}\n 기본 데미지 : {2} / 치명타 : {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeText.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;

        resultText.text = string.Format("누적 데미지 : {0:F1}\n공격횟수 : {1}\n평균 DPA : {2:F2}",
            totalDamage, attackCount, dpa);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean * stdDev * randStdNormal;
    }
}

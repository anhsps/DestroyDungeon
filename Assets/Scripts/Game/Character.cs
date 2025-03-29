using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    protected Animator animator;

    [Header("Parameters")]
    protected float hp;
    [SerializeField] protected float maxHP;
    [SerializeField] protected Slider hpSlider;
    [SerializeField] protected float speed;

    protected void InitialHP()
    {
        if (!hpSlider) return;
        hp = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = hp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hpSlider) hpSlider.value = hp;

        if (hp <= 0) Die();
    }

    protected virtual void Die()
    {
        animator.Rebind();
        hpSlider.gameObject.SetActive(false);
    }
}

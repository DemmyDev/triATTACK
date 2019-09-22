using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {

    [SerializeField] int bulletNum;
    [SerializeField] Transform hitParticle;
    BulletSelect parentUI;

    BoxCollider2D col;
    ScreenShake shake;
    Text text;
    Animation anim;
    bool isUnlocked = false;

    void Start()
    {
        parentUI = transform.parent.GetComponent<BulletSelect>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animation>();
        text = GetComponent<Text>();
        shake = Camera.main.GetComponent<ScreenShake>();
        UnlockCheck();
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
    }

    void UnlockCheck()
    {
        switch (bulletNum)
        {
            // Default
            case 0:
                col.enabled = true;
                text.CrossFadeAlpha(1f, 0f, true);
                isUnlocked = true;
                break;
            // Triple
            case 1:
                // The tri type is unlocked, but has not been animated in the selector yet
                if (ReadWriteSaveManager.Instance.GetData("UnlockedTriple", false) && !ReadWriteSaveManager.Instance.GetData("TripleAvailable", false))
                {
                    col.enabled = false;
                    StartCoroutine(UnlockAnim());
                    ReadWriteSaveManager.Instance.SetData("TripleAvailable", true, true); // Ensures animation doesn't play again
                }
                // The tri type has been unlocked and has been animated in the selector already
                else if (ReadWriteSaveManager.Instance.GetData("UnlockedTriple", false) && ReadWriteSaveManager.Instance.GetData("TripleAvailable", false))
                {
                    col.enabled = true;
                    text.CrossFadeAlpha(1f, 0f, true);
                    isUnlocked = true;
                }
                // The tri type has not been unlocked
                else
                {
                    col.enabled = false;
                    text.CrossFadeAlpha(.25f, 0f, true);
                }
                break;
            // Follow
            case 2:
                if (ReadWriteSaveManager.Instance.GetData("UnlockedFollow", false) && !ReadWriteSaveManager.Instance.GetData("FollowAvailable", false))
                {
                    col.enabled = false;
                    StartCoroutine(UnlockAnim());
                    ReadWriteSaveManager.Instance.SetData("FollowAvailable", true, true);
                }
                else if (ReadWriteSaveManager.Instance.GetData("UnlockedFollow", false) && ReadWriteSaveManager.Instance.GetData("FollowAvailable", false))
                {
                    col.enabled = true;
                    text.CrossFadeAlpha(1f, 0f, true);
                    isUnlocked = true;
                }
                else
                {
                    col.enabled = false;
                    text.CrossFadeAlpha(.25f, 0f, true);
                }
                break;
            // Sponge
            case 3:
                if (ReadWriteSaveManager.Instance.GetData("UnlockedSponge", false) && !ReadWriteSaveManager.Instance.GetData("SpongeAvailable", false))
                {
                    col.enabled = false;
                    StartCoroutine(UnlockAnim());
                    ReadWriteSaveManager.Instance.SetData("SpongeAvailable", true, true);
                }
                else if (ReadWriteSaveManager.Instance.GetData("UnlockedSponge", false) && ReadWriteSaveManager.Instance.GetData("SpongeAvailable", false))
                {
                    col.enabled = true;
                    text.CrossFadeAlpha(1f, 0f, true);
                    isUnlocked = true;
                }
                else
                {
                    col.enabled = false;
                    text.CrossFadeAlpha(.25f, 0f, true);
                }
                break;
            // Rapid
            case 4:
                if (ReadWriteSaveManager.Instance.GetData("UnlockedRapid", false) && !ReadWriteSaveManager.Instance.GetData("RapidAvailable", false))
                {
                    col.enabled = false;
                    StartCoroutine(UnlockAnim());
                    ReadWriteSaveManager.Instance.SetData("RapidAvailable", true, true);
                }
                else if (ReadWriteSaveManager.Instance.GetData("UnlockedRapid", false) && ReadWriteSaveManager.Instance.GetData("RapidAvailable", false))
                {
                    col.enabled = true;
                    text.CrossFadeAlpha(1f, 0f, true);
                    isUnlocked = true;
                }
                else
                {
                    col.enabled = false;
                    text.CrossFadeAlpha(.25f, 0f, true);
                }
                break;
            // Bounce
            case 5:
                if (ReadWriteSaveManager.Instance.GetData("UnlockedBounce", false) && !ReadWriteSaveManager.Instance.GetData("BounceAvailable", false))
                {
                    col.enabled = false;
                    StartCoroutine(UnlockAnim());
                    ReadWriteSaveManager.Instance.SetData("BounceAvailable", true, true);
                }
                else if (ReadWriteSaveManager.Instance.GetData("UnlockedBounce", false) && ReadWriteSaveManager.Instance.GetData("BounceAvailable", false))
                {
                    col.enabled = true;
                    text.CrossFadeAlpha(1f, 0f, true);
                    isUnlocked = true;
                }
                else
                {
                    col.enabled = false;
                    text.CrossFadeAlpha(.25f, 0f, true);
                }
                break;
            default:
                Debug.LogError("Selector bulletNum out of range");
                break;
        }
    }

    IEnumerator UnlockAnim()
    {
        anim.Play();
        yield return new WaitForSeconds(1f);
        anim.Stop();
        col.enabled = true;
        text.CrossFadeAlpha(1f, 0f, true);
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        AudioManager.Instance.Play("EnemyHit");
        isUnlocked = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("EnemyHit");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            col.enabled = false;
            parentUI.SelectBullet(bulletNum);
        }
    }
}
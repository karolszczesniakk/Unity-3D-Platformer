using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public static BossController instance;

    public Animator anim;
    public GameObject levelExit;
    public float waitToShowExit;
    public int bossHit, bossDeath, bossDeathShout, bossMusic;

    public enum BossPhase { intro, phase1, phase2, phase3, end}
    public BossPhase currentPhase = BossPhase.intro;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        AudioManager.instance.PlayMusic(bossMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isRespawning)
        {
            currentPhase = BossPhase.intro;
            anim.SetBool("Phase1",false);
            anim.SetBool("Phase2",false);
            anim.SetBool("Phase3",false);

            gameObject.SetActive(false);
            AudioManager.instance.PlayMusic(AudioManager.instance.levelMusicToPlay);
            BossActivator.instance.entrance.SetActive(true);
            BossActivator.instance.gameObject.SetActive(true);

            GameManager.instance.isRespawning = false;



        }
    }


    public void damageBoss()
    {
        AudioManager.instance.playSFX(bossHit);
        currentPhase++;
        PlayerController.instance.Bounce();
        if (currentPhase != BossPhase.end)
        {
            anim.SetTrigger("Hurt");
            
        }

        switch (currentPhase)
        {
            case BossPhase.phase1:
                anim.SetBool("Phase1", true);
                break;

            case BossPhase.phase2:    
                anim.SetBool("Phase2", true);
                anim.SetBool("Phase1", false);
                break;

            case BossPhase.phase3:
                anim.SetBool("Phase3", true);
                anim.SetBool("Phase2", false);
                break;

            case BossPhase.end:
                anim.SetTrigger("End");
                StartCoroutine(EndBoss());
                break;

        }

        IEnumerator EndBoss()
        {
            AudioManager.instance.playSFX(bossDeath);
            AudioManager.instance.playSFX(bossDeathShout);
            yield return new WaitForSeconds(waitToShowExit);
            //AudioManager.instance.PlayMusic(AudioManager.instance.levelMusicToPlay);
            levelExit.SetActive(true);

        }
        
    }
}

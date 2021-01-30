using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSkillScript : MonoBehaviour
{

    [SerializeField]
    int projectile;
    [SerializeField]
    int spread;
    [SerializeField]
    int range;

    GameManager gameManager;
    HudManager hudManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudManager>();

    }

    public void FetchRandomSkillForLvlUp()
    {

    }

    public void SkillPress()
    {
        gameManager.AttachSelectedSkillToPlayer(projectile,spread,range);
        gameManager.HandleCloseLevelUpMenu();
    }

    
}

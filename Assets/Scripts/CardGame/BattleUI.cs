using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [Header("High Level References")]

    public BattleSystem battleSystem;
    public GameObject upperThird, middleThird, lowerThird;

    [Header("Procedural Card Animation")]
    public float spreadAngle;
    public float fanRadius;
    public float phase;
    public float hoverMagnitude;
    public float yThreshold;
    public float cardSpeed;
    public GameObject cardDisplayPrefab;
    public Hand hand => battleSystem.hand;
    public PlayArea playArea => battleSystem.playArea;
    public PlayerCombo playerCombo => battleSystem.playerCombo;
    public EnemyCombo enemyCombo => battleSystem.enemyCombo;

    [Header("Status Effect Icons")]
    public List<GameObject> playerStatusEffectIcons;
    public List<GameObject> enemyStatusEffectIcons;

    [Header("Fighter UI")]
    public Image playerHealthBar;
    public TextMeshProUGUI playerHealthBarText;
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyHealthBarText;

    public GameObject playerBlockIndicator;
    public GameObject enemyBlockIndicator;

    public Image spiritFill;

    public Color nelly, bruttia, block, damage;

    [Header("Text")]
    public TextMeshProUGUI drawPileCount;
    public TextMeshProUGUI discardPileCount;
    public TextMeshProUGUI energyCount;
    public TextParticle textParticle;
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        CardsDisplay(hand.cards, hand.gameObject.GetComponent<RectTransform>());
        CardsDisplay(playArea.cards, playArea.gameObject.GetComponent<RectTransform>());
        StatusEffectDisplay();
        CardCountDisplay();
        SpiritDisplay();
        HealthBarDisplay();
    }
    public void PrintLog(string log)
    {
        print(log);
    }

    public void EndTurnButtonPressed()
    {
        battleSystem.EndTurn();
    }
    public void CardsDisplay(List<Card> cards, RectTransform container)
    {
        //if there are not enough card display objects for the number of cards, instantiate new ones
        if (container.childCount < cards.Count)
        {
            for (int i = 0; i < cards.Count - container.childCount; i++)
            {
                GameObject instance = Instantiate(cardDisplayPrefab, container.gameObject.transform);
                CardDisplay instanceDisplay = instance.GetComponent<CardDisplay>();
                instanceDisplay.card = cards[instance.transform.GetSiblingIndex()];
            }
        }
        float totalAngle = spreadAngle * cards.Count - 1;
        float startingAngle = -totalAngle / 2;
        for (int i = 0; i < container.childCount; i++)
        {
            Transform cardObject = container.GetChild(i);
            
            if (i + 1 < container.childCount)
            {
                Transform nextObject = container.GetChild(i + 1);
                if (nextObject.position.x < cardObject.position.x)
                {
                    nextObject.SetSiblingIndex(i);
                    cardObject.SetSiblingIndex(i + 1);
                    return;
                }
            }

            CardDisplay cardDisplay = cardObject.gameObject.GetComponent<CardDisplay>();
            bool active = cards.Count > 0 && i < cards.Count;
            float hoverHeight = cardDisplay.hover ? hoverMagnitude : 0;
            float angle = startingAngle + spreadAngle * i;
            float radianAngle = Mathf.Deg2Rad * angle;
            float x = Mathf.Sin(radianAngle + phase) * fanRadius;
            float y = (Mathf.Cos(radianAngle + phase) * fanRadius)-fanRadius;

            Vector3 cardPosition = new Vector3(x, y, 0) + cardObject.up * hoverHeight;
            Vector3 lerpedPosition = Vector3.Lerp(cardObject.localPosition, cardPosition, cardSpeed);
            Quaternion cardRotation = Quaternion.Euler(0, 0, -angle);
            Quaternion lerpedRotation = Quaternion.Lerp(cardObject.localRotation, cardRotation, cardSpeed);

            cardObject.localPosition = lerpedPosition;
            cardObject.localRotation = lerpedRotation;
            cardObject.position = cardDisplay.drag ? Input.mousePosition : cardObject.position;

            if (cards.Count < 1 || cards.Count < i)
            {
                Destroy(cardObject.gameObject);
            }
        }
    }
    public void StatusEffectDisplay()
    {
        Player player = battleSystem.player;
        for (int i = 0; i < playerStatusEffectIcons.Count; i++)
        {
            if (i < player.activeStatusEffects.Count)
            {
                playerStatusEffectIcons[i].SetActive(true);
                StatusEffect statusEffect = player.activeStatusEffects[i];
                Sprite symbol = statusEffect.symbol;
                int duration = statusEffect.duration;
                playerStatusEffectIcons[i].GetComponent<Image>().sprite = symbol;
                TextMeshProUGUI number = playerStatusEffectIcons[i].
                    transform.GetChild(0).
                        GetComponent<TextMeshProUGUI>();
                if (duration > 0)
                {
                    number.text = "" + duration;
                }
                else
                {
                    number.text = "";
                }
            }
            else
            {
                playerStatusEffectIcons[i].SetActive(false);
            }
        }
        // :(
        Enemy enemy = battleSystem.enemy;
        for (int i = 0; i < enemyStatusEffectIcons.Count; i++)
        {
            if (i < enemy.activeStatusEffects.Count)
            {
                enemyStatusEffectIcons[i].SetActive(true);
                StatusEffect statusEffect = enemy.activeStatusEffects[i];
                Sprite symbol = statusEffect.symbol;
                int magnitude = statusEffect.magnitude;
                enemyStatusEffectIcons[i].GetComponent<Image>().sprite = symbol;
                TextMeshProUGUI number = enemyStatusEffectIcons[i].
                    transform.GetChild(0).
                        GetComponent<TextMeshProUGUI>();
                if (magnitude > 0)
                {
                    number.text = "" + magnitude;
                }
                else
                {
                    number.text = "";
                }
            }
            else
            {
                enemyStatusEffectIcons[i].SetActive(false);
            }
        }
    }
    public void CardCountDisplay()
    {
        drawPileCount.text = battleSystem.player.drawPile.Count + "";
        discardPileCount.text = battleSystem.discard.cards.Count + "";
    }
    public void SpiritDisplay()
    {
        energyCount.text = battleSystem.player.spirit + "/" + battleSystem.player.spiritPerTurn;
        float percentage = (float)battleSystem.player.spirit / (float)battleSystem.player.spiritPerTurn;
        spiritFill.fillAmount = Mathf.Lerp(spiritFill.fillAmount, percentage, 0.1f);
    }
    public void HealthBarDisplay()
    {
        Player player = battleSystem.player;
        Enemy enemy = battleSystem.enemy;
        float healthBarSpeed = 0.5f;
        float playerHealth = player.health;
        float playerMaxHealth = player.maxHealth;
        float lerpedPlayerHealth = Mathf.Lerp(playerHealthBar.fillAmount * playerMaxHealth, playerHealth, healthBarSpeed);
        float enemyHealth = enemy.health;
        float enemyMaxHealth = enemy.maxHealth;
        float lerpedEnemyHealth = Mathf.Lerp(enemyHealthBar.fillAmount * enemyMaxHealth, enemyHealth, healthBarSpeed);
        playerHealthBar.fillAmount = lerpedPlayerHealth / playerMaxHealth;
        playerHealthBarText.text = playerHealth + "/" + playerMaxHealth;
        enemyHealthBar.fillAmount = lerpedEnemyHealth / enemyMaxHealth;
        enemyHealthBarText.text = enemyHealth + "/" + enemyMaxHealth;
        if (player.block > 0)
        {
            playerHealthBar.color = block;
            playerBlockIndicator.SetActive(true);
            playerBlockIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + player.block;
        }
        else
        {
            playerHealthBar.color = nelly;
            playerBlockIndicator.SetActive(false);
        }
        if (enemy.block > 0)
        {
            enemyHealthBar.color = block;
            enemyBlockIndicator.SetActive(true);
            enemyBlockIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + enemy.block;
        }
        else
        {
            enemyHealthBar.color = bruttia;
            enemyBlockIndicator.SetActive(false);
        }
    }
    public Vector2 PuppetPos(Fighter fighter, string bodyPart, Vector3 offset)
    {
        Transform part;
        if (!fighter.animator.transform.Find(bodyPart))
        {
            part = fighter.animator.transform.Find("S " + bodyPart);
        }
        else
        {
            part = fighter.animator.transform.Find(bodyPart);
        }
        Vector2 puppetPos = part.position + offset;
        return Camera.main.WorldToScreenPoint(puppetPos);
    }
    public void NumberPopUp(string text, Vector2 position) => textParticle.NumberPopUp(text, position);
    public void TextPopUp(string text, Vector2 position) => textParticle.TextPopUp(text, position);
    public void StatusPopUp(string text, Vector2 position) => textParticle.StatusPopUp(text, position);
}

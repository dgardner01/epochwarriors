using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public float idealCardSpacing;
    float cardSpacing;
    public float cardSpeed;
    public BattleSystem battleSystem;
    public GameObject cardDisplayPrefab;
    public Hand hand => battleSystem.hand;
    public PlayArea playArea => battleSystem.playArea;
    public PlayerCombo playerCombo => battleSystem.playerCombo;
    public EnemyCombo enemyCombo => battleSystem.enemyCombo;

    public List<GameObject> playerStatusEffectIcons;
    public List<GameObject> enemyStatusEffectIcons;

    public GameObject playerBlockIndicator;
    public GameObject enemyBlockIndicator;

    public Color nelly, bruttia, block, damage;

    public Image playerHealthBar;
    public TextMeshProUGUI playerHealthBarText;
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyHealthBarText;

    public GameObject upperThird, middleThird, lowerThird;

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
                Instantiate(cardDisplayPrefab, container.gameObject.transform);
            }
        }
        //if the container has cards, display them spaced apart evenly
        float containerWidth = container.sizeDelta.x;
        float idealHandWidth = cards.Count * idealCardSpacing;
        float handWidth = cards.Count * cardSpacing;
        float middle = (handWidth / 2) - cardSpacing / 2;
        cardSpacing = idealCardSpacing;
        //if the total width of the cards with ideal spacing is greater than the container, scale the overall spacing
        if (idealHandWidth > containerWidth)
        {
            cardSpacing = containerWidth / cards.Count;
        }
        //apply spacing to each card in container
        for (int i = 0; i < container.childCount; i++)
        {
            //hoverIndex is the list index of the card currently being hovered over
            //by default the middle card is considered hovered over
            int hoverIndex = Mathf.RoundToInt(cards.Count / 2);
            for (int j = 0; j < container.childCount; j++)
            {
                Transform _card = container.GetChild(j);
                CardDisplay _display = _card.gameObject.GetComponent<CardDisplay>();
                if (_display.hover)
                {
                    hoverIndex = j;
                }
            }
            Transform card = container.GetChild(i);
            CardDisplay display = card.GetComponent<CardDisplay>();
            int distanceFromHover = i - hoverIndex;
            //only use as many displays as there are cards
            if (cards.Count > 0 && i < cards.Count)
            {
                card.gameObject.SetActive(true);
                display.card = cards[i];
                float hoverDist = Mathf.Abs(distanceFromHover);
                float maxDist = 2;
                //individually space each card by how close it is to the hovered card
                float dist = distanceFromHover * Mathf.Lerp(cardSpacing / 4, 0, hoverDist / maxDist);
                float offset = (i * cardSpacing) + dist;
                Vector3 currentPos = card.transform.localPosition;
                Vector3 targetPos = new Vector2(offset - middle, 0);
                Vector3 lerpedPos = Vector3.Lerp(currentPos, targetPos, cardSpeed);
                card.transform.localPosition = lerpedPos;
            }
            else
            {
                card.gameObject.SetActive(false);
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
    public void BlockPopUp(Vector2 position) => textParticle.BlockPopUp(position);
}

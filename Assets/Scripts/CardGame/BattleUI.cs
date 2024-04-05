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
    [Header("Card Containers")]
    public GameObject playedZone;
    public GameObject enemyPlayedZone;
    public GameObject drawPile;
    public float comboSpacing;
    public Hand hand => battleSystem.hand;
    public PlayArea playArea => battleSystem.playArea;
    public Discard discard => battleSystem.discard;
    public PlayerCombo playerCombo => battleSystem.playerCombo;
    public EnemyCombo enemyCombo => battleSystem.enemyCombo;

    [Header("Status Effect Icons")]
    public List<GameObject> playerStatusEffectIcons;
    public List<GameObject> enemyStatusEffectIcons;

    [Header("Fighter UI")]
    public Image playerHealthBar;
    public Image playerTurnDamage;
    public TextMeshProUGUI playerHealthBarText;
    public Image enemyHealthBar;
    public Image enemyTurnDamage;
    public TextMeshProUGUI enemyHealthBarText;

    public GameObject playerComboTracker;
    public GameObject enemyComboTracker;

    public GameObject playerBlockIndicator;
    public GameObject enemyBlockIndicator;

    public Image spiritFill;

    public Color nelly, bruttia, block, damage;

    [Header("Text")]
    public TextMeshProUGUI drawPileCount;
    public TextMeshProUGUI discardPileCount;
    public TextMeshProUGUI energyCount;
    [Header("VFX")]
    public Transform UIParticleParent;
    public GameObject blockPopUp, statusPopUp, numberPopUp;
    private void Start()
    {
        InitializeCardDisplayObjects();
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        CardsDisplay(hand.cards, hand.gameObject.GetComponent<RectTransform>());
        CardsDisplay(playArea.cards, playArea.gameObject.GetComponent<RectTransform>());
        ContainerDisplay(discard.transform);
        ContainerDisplay(drawPile.transform);
        ContainerDisplay(playedZone.transform);
        ContainerDisplay(enemyPlayedZone.transform);
        ComboDisplay(playerComboTracker, battleSystem.player);
        ComboDisplay(enemyComboTracker, battleSystem.enemy); ;
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
    public void InitializeCardDisplayObjects()
    {
        for (int i = 0; i < battleSystem.player.drawPile.Count; i++)
        {
            GameObject instance = Instantiate(cardDisplayPrefab, drawPile.transform);
            instance.GetComponent<CardDisplay>().card = battleSystem.player.drawPile[i];
        }
    }
    public void ContainerDisplay(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            GameObject cardObject = container.GetChild(i).gameObject;
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            if (cardDisplay.discardBuffer <= 0 && !cardDisplay.played)
            {
                cardObject.transform.localPosition = Vector3.Lerp(cardObject.transform.localPosition, Vector3.zero, cardSpeed * 2);
            }
        }
    }
    public void ReparentCard(GameObject cardObject, Transform container)
    {
        cardObject.transform.SetParent(container);
    }

    public void CardsDisplay(List<Card> cards, RectTransform container)
    {
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
                }
            }

            CardDisplay cardDisplay = cardObject.gameObject.GetComponent<CardDisplay>();
            float hoverHeight = cardDisplay.hover ? hoverMagnitude : 0;
            float angle = startingAngle + spreadAngle * i;
            float radianAngle = Mathf.Deg2Rad * angle;
            float x = Mathf.Sin(radianAngle + phase) * fanRadius;
            float y = (Mathf.Cos(radianAngle + phase) * fanRadius) - fanRadius;

            Vector3 cardPosition = new Vector3(x, y, 0) + cardObject.up * hoverHeight;

            float distFromTarget = Vector3.Distance(cardObject.localPosition, cardPosition);
            float maxDist = 100;
            cardSpeed = Mathf.Lerp(0.1f, 0.5f, distFromTarget / maxDist);

            Vector3 lerpedPosition = Vector3.Lerp(cardObject.localPosition, cardPosition, cardSpeed);
            Quaternion cardRotation = Quaternion.Euler(0, 0, -angle);
            Quaternion lerpedRotation = Quaternion.Lerp(cardObject.localRotation, cardRotation, cardSpeed);

            cardObject.localPosition = lerpedPosition;
            cardObject.localRotation = lerpedRotation;
            cardObject.position = cardDisplay.drag ? Input.mousePosition : cardObject.position;

            if (cards.Count < 1)
            {
                Destroy(cardObject.gameObject);
            }
        }
    }
    public bool DragFromHand()
    {
        Transform container = hand.transform;
        for (int i = 0; i < container.childCount; i++)
        {
            CardDisplay cardDisplay = container.GetChild(i).GetComponent<CardDisplay>();
            if (cardDisplay.drag)
            {
                return true;
            }
        }
        return false;
    }
    public void InitializeComboDisplay()
    {
        List<GameObject> comboCards = new List<GameObject>();
        Transform comboContainer = playedZone.transform;
        for (int i = 0; i < comboContainer.childCount; i++)
        {
            GameObject cardObject = comboContainer.GetChild(i).gameObject;
            comboCards.Add(cardObject);
        }
        int count = comboCards.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject cardObject = comboCards[0];
            ReparentCard(cardObject, playerCombo.transform);
            //print("parented " + cardObject.GetComponent<CardDisplay>().card + " to player combo");
            comboCards.RemoveAt(0);
        }
        Enemy enemy = battleSystem.enemy;
        for (int i = 0; i < enemy.currentTurn.Count; i++)
        {
            GameObject instance = Instantiate(cardDisplayPrefab, enemyCombo.transform);
            CardDisplay cardDisplay = instance.GetComponent<CardDisplay>();
            cardDisplay.card = enemy.currentTurn[i];
            instance.transform.position = Vector3.one * -1000;
        }
    }
    public void PlayComboCard(Transform comboContainer, List<Card> cards)
    {
        if (cards.Count > 0)
        {
            int index = comboContainer.childCount - cards.Count;
            print("index is " + index);
            GameObject cardObject = comboContainer.GetChild(index).gameObject;
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            cardDisplay.card = cards[0];
            cardDisplay.bounceTime = 0;
            cardDisplay.played = true;
            cardObject.transform.localPosition = Vector3.zero - Vector3.up * (comboSpacing * index);
            cardObject.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-15, 15));
        }
    }
    public IEnumerator ClearCombo(Transform comboContainer, Transform targetContainer)
    {
        List<GameObject> comboCards = new List<GameObject>();
        for (int i = 0; i < comboContainer.childCount; i++)
        {
            GameObject cardObject = comboContainer.GetChild(i).gameObject;
            comboCards.Add(cardObject);
        }
        int cardsToReparent = comboCards.Count;
        for (int i = 0; i < cardsToReparent; i++)
        {
            GameObject cardObject = comboCards[0];
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            cardDisplay.played = false;
            ReparentCard(cardObject, targetContainer);
            comboCards.RemoveAt(0);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ComboDisplay(GameObject comboTracker, Fighter fighter)
    {
        if (fighter.consecutiveHits > 1)
        {
            comboTracker.SetActive(true);
            Transform trackerObject = comboTracker.transform;
            trackerObject.GetChild(1).GetComponent<TextMeshProUGUI>().text = fighter.consecutiveHits + " hits!";
        }
        else
        {
            comboTracker.SetActive(false);
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
        float _playerTurnDamage = player.turnDamage;
        float playerMaxHealth = player.maxHealth;
        float lerpedPlayerHealth = Mathf.Lerp(playerHealthBar.fillAmount * playerMaxHealth, playerHealth, healthBarSpeed);
        float enemyHealth = enemy.health;
        float _enemyTurnDamage = enemy.turnDamage;
        float enemyMaxHealth = enemy.maxHealth;
        float lerpedEnemyHealth = Mathf.Lerp(enemyHealthBar.fillAmount * enemyMaxHealth, enemyHealth, healthBarSpeed/5);
        playerHealthBar.fillAmount = lerpedPlayerHealth / playerMaxHealth;
        playerTurnDamage.fillAmount = Mathf.Lerp(playerTurnDamage.fillAmount, _playerTurnDamage/playerMaxHealth, healthBarSpeed);
        playerHealthBarText.text = playerHealth + "/" + playerMaxHealth;
        enemyHealthBar.fillAmount = lerpedEnemyHealth / enemyMaxHealth;
        enemyTurnDamage.fillAmount = Mathf.Lerp(enemyTurnDamage.fillAmount, _enemyTurnDamage/enemyMaxHealth, healthBarSpeed/5);
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
    public void TextPopUp(string text, Vector2 position, GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, UIParticleParent);
        instance.transform.position = position;
        instance.GetComponent<TextMeshProUGUI>().text = text;
    }
}

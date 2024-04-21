using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI title, spiritCost, description;
    public TextMeshProUGUI[] symbolMagnitudes;
    public Image bg;
    public Sprite[] bgs;
    public Image[] symbols;
    public Sprite[] symbolSprites;
    public Image chain;
    public Sprite[] comboChains;
    public ParticleSystem[] chainParticles;
    BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>();

    public AnimationCurve[] bounce;
    public float bounceTime;
    public float bounceTimeMax;

    public Vector2 wiggle;
    public float wiggleMagnitude;
    public float wiggleTime;
    public float wiggleTimeMax;

    public bool hover;
    public bool drag;
    public bool played;
    public bool playable;
    public bool chained;
    public float discardBuffer;
    public float yThreshold;
    public Card card;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < chainParticles.Length; i++)
        {
            chainParticles[i].startRotation = -transform.localRotation.z;
            chainParticles[i].enableEmission = chained;
        }
        Transform hand = FindAnyObjectByType<Hand>().transform;
        Transform playArea = FindAnyObjectByType<PlayArea>().transform;
        playable = battleSystem.player.spirit >= card.spiritCost;
        yThreshold = battleSystem.ui.yThreshold;
        if (transform.position.y > yThreshold && transform.parent == hand && playable)
        {
            battleSystem.ui.ReparentCard(gameObject, battleSystem.playArea.transform);
            battleSystem.PlayCard(card);
        }
        if (transform.position.y < yThreshold && transform.parent == playArea)
        {
            battleSystem.ui.ReparentCard(gameObject, battleSystem.hand.transform);
            battleSystem.ReturnCard(card);
        }
        Player player = battleSystem.player;
        title.text = card.name;
        spiritCost.text = ""+card.spiritCost;
        description.text = card.description;
        if (description.text == "")
        {
            description.gameObject.SetActive(false);
        }
        else
        {
            description.gameObject.SetActive(true);
        }
        if (card.comboPosition > -1)
        {
            chain.gameObject.SetActive(true);
            chain.sprite = comboChains[card.comboPosition];
        }
        else
        {
            chain.gameObject.SetActive(false);
        }
        switch (card.cardType)
        {
            case CardType.Attack:
                bg.sprite = bgs[0];
                symbols[0].sprite = symbolSprites[0];
                if (player.strength > 0)
                {
                    symbolMagnitudes[0].text = "" + (card.damage+player.strength);
                }
                else
                {
                    symbolMagnitudes[0].text = "" + card.damage;
                }
                if (card.statusEffect != null)
                {
                    symbols[1].sprite = card.statusEffect.symbol;
                    symbolMagnitudes[1].text = "" + Mathf.Max(card.statusEffect.duration, card.statusEffect.magnitude);
                }
                else
                {
                    symbols[1].sprite = null;
                }
                break;
            case CardType.Block:
                bg.sprite = bgs[1];
                symbols[0].sprite = symbolSprites[1];
                if (card.statusEffect != null)
                {
                    symbols[1].sprite = card.statusEffect.symbol;
                    if (card.statusEffect.id == "Reflect")
                    {
                        Enemy enemy = battleSystem.enemy;
                        int halvedDamage = 0;
                        if (enemy.FirstAttackInTurn() != null)
                        {
                            halvedDamage = (enemy.FirstAttackInTurn().damage + enemy.strength) / 2;
                        }
                        card.statusEffect.magnitude = halvedDamage;
                        card.block = halvedDamage;
                    }
                    symbolMagnitudes[1].text = "" + Mathf.Max(card.statusEffect.duration, card.statusEffect.magnitude);
                }
                else
                {
                    symbols[1].sprite = null;
                }
                symbolMagnitudes[0].text = ""+card.block;
                if (card.damage > 0)
                {
                    symbols[1].sprite = symbolSprites[0];
                    symbolMagnitudes[1].text = card.damage + "";
                    if (player.strength > 0)
                    {
                        symbolMagnitudes[1].text = "" + (card.damage + player.strength);
                    }
                    else
                    {
                        symbolMagnitudes[1].text = "" + card.damage;
                    }
                }
                break;
            case CardType.Skill:
                bg.sprite = bgs[2];
                if (card.statusEffect != null)
                {
                    symbols[0].sprite = card.statusEffect.symbol;
                    symbolMagnitudes[0].text = "" + Mathf.Max(card.statusEffect.duration, card.statusEffect.magnitude);
                }
                else
                {
                    symbols[0].sprite = null;
                }
                symbols[1].sprite = null;
                break;
        }
        for (int i = 0; i < symbols.Length; i++)
        {
            if (symbols[i].sprite == null)
            {
                symbols[i].gameObject.SetActive(false);
            }
            else
            {
                symbols[i].gameObject.SetActive(true);
            }
        }
    }
    private void FixedUpdate()
    {
        float startScale = .33f;
        float x = transform.position.x;
        Vector3 targetScale = Vector3.one * startScale;
        if (bounceTime < bounceTimeMax)
        {
            bounceTime += Time.deltaTime;
            targetScale = new Vector3(bounce[0].Evaluate(bounceTime/bounceTimeMax), bounce[1].Evaluate(bounceTime/bounceTimeMax));
        }
        if (discardBuffer > 0)
        {
            discardBuffer -= Time.deltaTime;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.25f);
    }
    public void PlayCard()
    {
        battleSystem.OnCardPutdown.Invoke();
        bounceTime = 0;
        if (card != null && card.name == "Taunt" && transform.parent == battleSystem.playArea.transform)
        {
            battleSystem.ResolveInstantCard(card);
            battleSystem.ui.ReparentCard(gameObject, battleSystem.discard.transform);
        }
        if (!playable && transform.parent == battleSystem.hand.transform)
        {
            battleSystem.OnNotEnoughSpirit.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        battleSystem.OnCardHover.Invoke();
        hover = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hover = false;
    }
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        battleSystem.OnCardPickup.Invoke();
        drag = true;
    }
    public void OnDrag(PointerEventData pointerEventData)
    {

    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        drag = false;
    }
}

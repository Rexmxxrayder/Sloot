using System.Collections.Generic;
using UnityEngine;

public class EntityBodyPart : EntityComponent {
    public enum NoduleType {
        NONE = -1,
        UNCOLOR = 0,
        RED = 1,
        BLUE = 2,
        GREEN = 3,
        YELLOW = 4,
        PURPLE = 5,
        WHITE = 6
    }

    public static Color NoduleColor(NoduleType type) {
        switch (type) {
            case NoduleType.UNCOLOR:
                return new Color(0.5f, 0.5f, 0.5f, 1);
            case NoduleType.RED:
                return Color.red;
            case NoduleType.BLUE:
                return Color.blue;
            case NoduleType.GREEN:
                return Color.green;
            case NoduleType.YELLOW:
                return Color.yellow;
            case NoduleType.PURPLE:
                return new Color(1, 0, 1, 1);
            case NoduleType.WHITE:
                return Color.white;
            case NoduleType.NONE:
            default:
                return Color.clear;
        }
    }

    [SerializeField] KeyCode keyCode;
    /*[SerializeField]*/
    [SerializeField] protected NoduleType noduleType = NoduleType.UNCOLOR;
    private EntityBrain brain;
    Dictionary<int, Ability> abilities = new();
    public bool Available => abilities[(int)noduleType].IsAvailable;

    protected Sprite sprite;

    public Sprite Sprite => sprite;
    public NoduleType Nodule => noduleType;

    protected override void ResetSetup() {
        for (int i = 0; i < transform.childCount; i++) {
            abilities.Add(i, transform.GetChild(i).GetComponent<Ability>());
        }
    }

    protected override void LoadSetup() {
        RootGet<EntityBrain>().OnCannotAct += CancelAbility;
    }

    public void AssignBrain(EntityBrain newBrain) {
        brain = newBrain;
    }

    private void Update() {
        if (Input.GetKeyDown(keyCode)) {
            KeyEvenement(false);
        }

        if (Input.GetKeyUp(keyCode)) {
            KeyEvenement(true);
        }
    }
    public void AddNodules(NoduleType nodule) {
        noduleType = nodule;
    }

    public void RemoveNodules() {
        noduleType = NoduleType.UNCOLOR;
    }

    public void KeyEvenement(bool isUp) {
        if (!abilities.ContainsKey((int)noduleType)) {
            return;
        }

        abilities[(int)noduleType].Launch(brain, isUp);
    }

    public void CancelAbility() {
        abilities[(int)noduleType].Cancel();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeState : MonoBehaviour {

    [SerializeField]
    private string _id;
    public string Id { get { return _id; } }

    private bool _isOtherPlayer;
    public bool IsOtherPlayer { get { return _isOtherPlayer; } }
    private bool _isCurrentPlayer;
    public bool IsCurrentPlayer { get { return _isCurrentPlayer; } }

    [SerializeField]
    private bool updatePositionFromServer;

    GameStateManager stateManager;

    Sprite[] beeSprites;

    void Start() {
        stateManager = GameStateManager.Instance;
        stateManager.GameStateUpdated += UpdatePosition;
    }

    Dictionary<string, int> hatIndexes = new Dictionary<string, int> {
        {"Construction", 0},
        {"Crown", 1},
        {"Fedora", 2},
        {"Popo", 3},
        {"Santa", 4},
        {"Suess", 5},
        {"Sorting", 6},
        {"Tophat", 7},
        {"Newbie", 7},
    };

    Sprite GetSpriteForHatName(string hatName) {
        beeSprites = Resources.LoadAll<Sprite>("BeeSprites");
        int index;
        hatIndexes.TryGetValue(hatName, out index);
        return beeSprites[index];
    }

    public void Initialize(Bee bee) {
        _isCurrentPlayer = GameStateManager.Instance.IsCurrentPlayer(bee.id);
        _isOtherPlayer = bee.isPlayer && !_isCurrentPlayer;

        _id = bee.id;

        if (IsCurrentPlayer) {
            updatePositionFromServer = false;
        } else if (IsOtherPlayer) {
            updatePositionFromServer = true;
        } else {
            updatePositionFromServer = !GameStateManager.Instance.IsRoomOwner;
        }


        if (!(_isCurrentPlayer && bee.hatName == "Crown")) {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null) {
                sr = GetComponentInChildren<SpriteRenderer>();
            }
            sr.sprite = GetSpriteForHatName(bee.hatName);
        }

        if (bee.hatName == "Crown") {
            transform.localScale *= 2;
        }
    }

    public void UpdatePosition() {
        if (updatePositionFromServer) {
            Vector2? position = stateManager.state.GetBeePosition(Id);
            gameObject.transform.position = position.Value;
        }
    }

    public Vector2 GetPosition() {
        return gameObject.transform.position;
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= UpdatePosition;    
    }
}

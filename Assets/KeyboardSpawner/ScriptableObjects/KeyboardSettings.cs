using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardSettings")]
public class KeyboardSettings : ScriptableObject {
    public KeyRow[] charRows = new KeyRow[] {
        new KeyRow() {
            keys = new string[] {
                "Q",
                "W",
                "E",
                "R",
                "T",
                "Y",
                "U",
                "I",
                "O",
                "P"
            }
        },
        new KeyRow() {
            keys = new string[] {
                "A",
                "S",
                "D",
                "F",
                "G",
                "H",
                "J",
                "K",
                "L",
                "-"
            }
        },
        new KeyRow() {
            keys = new string[] {
                "Z",
                "X",
                "C",
                "V",
                "B",
                "N",
                "M",
                "<",
                ">",
                "_"
            }
        },
        new KeyRow() {
            keys = new string[] {
                "@",
                "#",
                "$",
                " ",
                ".",
                "^",
                "&",
            }
        },
    };

    public Vector2 buttonSize = new Vector2(20,20);
    public float buttonMargin = 2;

    [Space(15)]
    public KeySpriteOverride[] keySpriteOverrides = new KeySpriteOverride[]{};
    public KeySizeOverride[] keySizeOverrides = new KeySizeOverride[]{};

    //not using dictionary bc they don't serialize in the Unity editor
    public bool GetKeySpriteOverride(string keyValue, out Sprite sprite) {
        foreach (var keyCodeSprite in keySpriteOverrides) {
            if (keyCodeSprite.keyValue == keyValue) {
                sprite = keyCodeSprite.sprite;
                return true;
            }
        }

        sprite = null;
        return false;
    }

    public bool GetKeySizeOverride(string keyValue, out KeySizeOverrideValue value) {
        foreach (var keyValueSize in keySizeOverrides)
        {
            if (keyValueSize.keyValue == keyValue) {
                value = keyValueSize.value;
                return true;
            }
        }

        value = default(KeySizeOverrideValue);
        return false;
    }
}

[Serializable]
public struct KeySpriteOverride {
    public string keyValue;
    public Sprite sprite;
}

[Serializable]
public struct KeySizeOverride {
    public string keyValue;
    public KeySizeOverrideValue value;
}

[Serializable]
public struct KeySizeOverrideValue {
    public Vector2 size;
    public bool affectsXPositioning;
}

[Serializable]
public struct KeyRow {
    public string[] keys;
}

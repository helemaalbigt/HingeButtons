#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

//Quick and dirty edit of KeyboardSpawnerCanvas to work outside of a canvas
//todo:clean up
public class KeyboardSpawner : MonoBehaviour
{
    public KeyboardSettings settings;
    public GameObject keyPrefab;
    public Transform keyParent;

    private float _nextXpos;

    public void SpawnKeyboard() {
        for (var i = 0; i < settings.charRows.Length; i++) {
            var row = settings.charRows[i];
            _nextXpos = 0;

            for (var j = 0; j < row.keys.Length; j++) {
                var keyValue = row.keys[j];

                //spawn key object
#if UNITY_EDITOR
                var keyGo = PrefabUtility.InstantiatePrefab(keyPrefab) as GameObject;
#else
                var keyGo = Instantiate(keyPrefab) as GameObject;
#endif
                keyGo.transform.SetParent(keyParent);
                keyGo.transform.localScale = Vector3.one;
                keyGo.transform.localRotation = Quaternion.identity;
                keyGo.name = keyValue;

                //Try to set the visible value of the key
                Sprite spriteOverride;
                var sprite = keyGo.GetComponentInChildren<Image>(true);
                var text = keyGo.GetComponentInChildren<Text>(true);
                if (settings.GetKeySpriteOverride(keyValue, out spriteOverride)) {
                    if (sprite != null) {
                        sprite.sprite = spriteOverride;
                        sprite.enabled = true;
                        sprite.gameObject.SetActive(true);
                    }
                    if (text != null) {
                        text.text = keyValue;
                        text.enabled = false;
                    }
                } else {
                    if (text != null) {
                        text.text = keyValue;
                        text.enabled = true;
                    }
                    if (sprite != null)
                        sprite.enabled = false;
                }

                //optionally set the value of the key if it implements this interface
                var key = keyGo.GetComponent<ISettableKeyValue>();
                if (key != null)
                    key.SetKeyValue(keyValue);

                //position the key
                var transform = keyGo.transform;

                KeySizeOverrideValue sizeOverride;
                if (settings.GetKeySizeOverride(keyValue, out sizeOverride)) {
                    // transform.localScale = new Vector3(sizeOverride.size.x, 1, sizeOverride.size.y);
                } else {
                    // transform.localScale = new Vector3(settings.buttonSize.x, 1, settings.buttonSize.y);
                }

                transform.localPosition = new Vector3(
                    _nextXpos,
                   0,
                     i == 0 ? 0 : -i * settings.buttonSize.y - i * settings.buttonMargin
                );

                _nextXpos += sizeOverride.affectsXPositioning ? settings.buttonSize.x + settings.buttonMargin : settings.buttonSize.x + settings.buttonMargin;
            }
        }
    }

    public void ClearKeyboard() {
        while (keyParent.childCount > 0) {
            foreach (Transform keyTrans in keyParent) {
                DestroyImmediate(keyTrans.gameObject);
            }
        }

    }
}
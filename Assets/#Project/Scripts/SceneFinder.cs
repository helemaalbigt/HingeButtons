using UnityEngine;

//helper class for scene references
public class SceneFinder : MonoBehaviour
{
    void Awake() {
        if (_singleton == null)
            _singleton = this;
    }

    private static SceneFinder _singleton;
    public static SceneFinder I {
        get {
            return _singleton;
        }
    }

    [SerializeField]
    private Transform _vrui;
    public Transform vrui {
        get { return _vrui; }
    }
}

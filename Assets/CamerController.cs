using UnityEngine;
using System.Collections;

public class CamerController : MonoBehaviour {

    Vector3 _pos;
    public void SetUpCam(Vector3 pos)
    {
        transform.position = _pos + pos;
        transform.LookAt(pos);
    }
    void Start () {
        _pos = new Vector3(-10, 10, -10);
    }

}

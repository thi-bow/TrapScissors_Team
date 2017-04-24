using UnityEngine;
using System.Collections;

public class QuarterView : MonoBehaviour
{
    private GameObject _myObject;
    private float _positionY;
    private int _sorting;
    [SerializeField, TooltipAttribute("優先度")]
    public int _priority;
    private string _stringAdd;

    // Use this for initialization
    void Start ()
    {
        _myObject = this.gameObject;
        OrderUpdate();
	}
	
	// Update is called once per frame
	void Update ()
    {
        OrderUpdate();
	}

    public void OrderUpdate()
    {
        _positionY = -_myObject.transform.localPosition.y * 1000 + _priority;
        _sorting = (int)_positionY;
        _myObject.GetComponent<SpriteRenderer>().sortingOrder = _sorting;
    }
}

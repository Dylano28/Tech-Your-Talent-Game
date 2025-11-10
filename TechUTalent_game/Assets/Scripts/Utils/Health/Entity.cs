using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    private int _currentHp;
    [SerializeField] private int hp = 1;

    [SerializeField] private int team = 0;
    public int Team => team;

    private enum DEATHSTATE
    {
        None,
        Destroy,
        Disable
    }
    [SerializeField] private DEATHSTATE deathState = DEATHSTATE.None;
    [SerializeField] private GameObject deathObject;

    [SerializeField] private string hitTag = "Entities";

    [SerializeField] private UnityEvent<int> onHit;
    [SerializeField] private UnityEvent onDie;


    private void Start()
    {
        _currentHp = hp;
        gameObject.tag = hitTag;

        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Hit(int damage)
    {
        _currentHp -= damage;
        onHit.Invoke(_currentHp);

        if (_currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        onDie.Invoke();

        switch (deathState)
        {
            case DEATHSTATE.None: break;
            case DEATHSTATE.Destroy:
                Destroy(deathObject);
                break;
            case DEATHSTATE.Disable:
                deathObject.SetActive(false); 
                break;
        }
    }
}

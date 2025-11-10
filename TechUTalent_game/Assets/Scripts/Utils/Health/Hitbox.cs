using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private int team = 0;
    [SerializeField] private bool constant = false;

    [SerializeField] private string hitTag = "Entities";
    [SerializeField] private string hitLayer = "Default";

    [SerializeField] private UnityEvent onHit;

    private Collider2D _coll;
    private ContactFilter2D _collisionFilter;
    private const int COLLISION_AMOUNT = 10;

    private void Start()
    {
        if (GetComponent<Collider2D>() == false)
        {
            Debug.Log("No collider detected on object " + gameObject.name + "!");
            return;
        }

        _coll = GetComponent<Collider2D>();
        _coll.isTrigger = true;

        _collisionFilter = new ContactFilter2D();
        _collisionFilter.SetLayerMask(LayerMask.GetMask(hitLayer));
    }

    private void FixedUpdate()
    {
        if (constant == false) return;

        var results = new Collider2D[COLLISION_AMOUNT];
        _coll.Overlap(_collisionFilter, results);

        if (results.Length == 0) return; 
        for (int i = 0; i < results.Length; i++) HitBody(results[i]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (constant) return;

        HitBody(collision);
    }

    private void HitBody(Collider2D collision)
    {
        if (collision.tag != hitTag && collision.GetComponent<Entity>() == false) return;

        var entity = collision.GetComponent<Entity>();
        if (entity.Team == team) return;

        entity.Hit(damage);
        onHit.Invoke();
    }
}

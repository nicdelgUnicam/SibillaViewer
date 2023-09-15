using UnityEngine;
using Utils;

public class MoveScript : MonoBehaviour
{
    /// <summary>
    /// The path of the source file, or the entire content on WebGL
    /// </summary>
    public string sourceFile;
    public int[] indices;

    private float _targetTime;
    private float _prevTime;
    private Vector3 _target;

    private IStepsController<Step> _controller;
    private Rigidbody2D _rigidbody;

    
    private void Start()
    {
        _controller = new FileStepsController<Step>(sourceFile, indices, Step.FromLine);
        _rigidbody = GetComponent<Rigidbody2D>();

        transform.position = _controller.GetNext().Position;
        _prevTime = Time.time;
    }

    private void FixedUpdate()
    {
        // ReSharper disable once InvertIf
        if (Time.fixedTime - _prevTime >= _controller.DeltaTime)
        {
            // ReSharper disable once InvertIf
            if (!SetupNextTarget())
            {
                _rigidbody.velocity = Vector2.zero;
                GetComponent<SpriteRenderer>().color = Color.green;
                enabled = false;
            }
        }
    }

    private bool SetupNextTarget()
    {
        var entry = _controller.GetNext();
        if (entry == null)
            return false;

        _target = entry.Position;
        _rigidbody.velocity = (_target - transform.position) / _controller.DeltaTime;
        _prevTime = Time.fixedTime;
        
        return true;
    }
}

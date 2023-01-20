using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Renderer _renderer;

    public float Speed { get => _speed; set => _speed = value; }

    void FixedUpdate()
    {
        _renderer.material.mainTextureOffset += new Vector2(_speed * Time.deltaTime, 0f);
    }
}

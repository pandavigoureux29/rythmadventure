﻿using UnityEngine;
using System.Collections;

/** Visual effect of the magic, such as a fire ball or cure drop */ 
public class BattleFightMagicEffect : MonoBehaviour {

	[SerializeField] protected SpriteRenderer m_effectSprite;

	protected bool m_launched = false;

	/** Set by the BattleFightMagic when started */
	protected BattleFightMagic m_magic;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void Launch(Vector3 _origin, Vector3 _destination){	
		m_launched = true;
		Utils.SetAlpha (m_effectSprite, 1f);
		transform.position = _destination;
		TweenEngine.Tween tween = TweenEngine.instance.RotateAroundZTo (m_effectSprite.transform, 360.0f, 0.3f, false, 1, "Die");
		tween.CallbackObject = gameObject;
	}

	virtual public void Die(){
		m_launched = false;
		Utils.SetAlpha (m_effectSprite, 0f);
	}

	public bool IsLaunched {
		get {
			return m_launched;
		}
	}

	public BattleFightMagic Magic {
		get {
			return m_magic;
		}
		set {
			m_magic = value;
		}
	}
}
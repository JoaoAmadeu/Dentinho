using System;
using UnityEngine;

/// <summary>
/// Represent the source and the strength to damage something
/// </summary>
[Serializable]
public class Damage
{
	[SerializeField]
	private int strenght;

	public int Strenght
	{
		get { return strenght; }
	}

	[SerializeField]
	private DamageType type;

	public DamageType Type
	{
		get { return type; }
	}

	public Damage (int strenght, DamageType type)
	{
		this.strenght = strenght;
		this.type = type;
	}
}
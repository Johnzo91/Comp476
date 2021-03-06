﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour
{
	private float timeStuck = 0.0f;	
	public float wanderDistance = 0.02f, wanderRadius = 0.005f, updateRange= 0.015f;
	public float MaxVelocity = 0.1f, MaxAcceleration = 3f;
	public Vector3 target;
	Vector3 velocity;

	public List<Vector3> path;
	bool halt;
	
	void Start()
	{
		halt = false;
		velocity = Vector3.zero;
		path = new List<Vector3>();
		UpdatePath(new Vector3(Random.Range(0f,50f), 0f, Random.Range(0f,50f)));
		target = transform.position;
	}

	void Update()
	{		
		Vector3 direction = (target - transform.position);
		UpdateTarget();
		UpdateRotation(Time.deltaTime);
		UpdateVelocities(Time.deltaTime);

		if(Vector3.Angle (velocity, transform.forward) < 45f || direction.magnitude <= 5f)
		{
			UpdatePosition(Time.deltaTime);
		}
	}	
	
	private void ResetVelocities()
	{
		velocity = Vector3.zero;
	}
	
	private void UpdateTarget()
	{
		if(path.Count > 0 && transform.position == target)
		{
			target = path[0];
			path.Remove(target);
		}/* Allow poor wandering
		else if(path.Count == 0)
		{
			path.Add(new Vector3(Random.Range(0f,50f), 0f, Random.Range(0f,50f)));
		}*/
		
	}
	
	private void UpdateVelocities(float deltaTime)
	{	
		halt = (path.Count == 0 && transform.position == target);
		
		if(!halt)
		{
			Seek();

			velocity *= deltaTime;
		}
		else
		{
			velocity = Vector3.zero;
			path = new List<Vector3>();
		}
		
		velocity = velocity.normalized * MaxVelocity;
		velocity = new Vector3(velocity.x, transform.forward.y, velocity.z);					
		velocity = (velocity.magnitude < MaxVelocity) ? velocity : velocity.normalized * MaxVelocity;

	}
	
	private void UpdatePosition(float deltaTime)
	{
		Vector2 posDiff = new Vector2(target.x - transform.position.x, target.z - transform.position.z);
		
		if(target == null || posDiff.magnitude > 0.5f)
		{
			transform.position += velocity * deltaTime;
		}
		else
		{
			transform.position = target;
		}
	}	
	
	private void UpdateRotation(float deltaTime)
	{		
		float step = Time.deltaTime * 5f;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, velocity, step, 0.0F);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	public void UpdatePath(List<Vector3> p)
	{
		path = new List<Vector3>();

		AddToPath(p);

	}

	public void UpdatePath(Vector3 p)
	{
		path = new List<Vector3>();
		
		AddToPath(p);
		
	}

	public void AddToPath(List<Vector3> p)
	{
		foreach(Vector3 t in p)
		{
			AddToPath(t);
		}		
	}

	public void AddToPath(Vector3 p)
	{
		path.Add(p);	
	}

	private void Seek()
	{					
		Vector3 directionVec = target - transform.position;
		Vector3 vel = MaxVelocity * directionVec.normalized;
		
		velocity = vel;
	}
}




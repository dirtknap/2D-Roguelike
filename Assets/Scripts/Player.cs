﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;


    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();

    }

	// Use this for initialization
	protected override void Start ()
	{
	    animator = GetComponent<Animator>();
	    food = GameManager.instance.playerFoodPoints;

        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        var horizontal = (int)Input.GetAxisRaw("Horizontal");
        var vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0) vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Exit":
                Invoke("Restart", restartLevelDelay);
                enabled = false;
                break;
            case "Food":
                food += pointsPerFood;
                other.gameObject.SetActive(false);
                break;
            case "soda":
                food += pointsPerSoda;
                other.gameObject.SetActive(false);
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0) GameManager.instance.GameOver();
    }
    
    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
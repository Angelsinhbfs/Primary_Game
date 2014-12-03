using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;
using System.Collections.Generic;
using Assets.Scripts.Weapon;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Player
{
    public class Player : Entity
    {
        
        //functional stats
        public int MaxHp;
        public Vector2 Speed;
        public float FireRate;
        public bool isPlayerOne;

        public bool isRespawning = false;

        //weapon variables
        public List<GameObject> Weapons;
        public int SelectedWeapon = 0;
        public List<GameObject> Target;
        public GameObject tField;

        //control variables
        private string Horizontal;
        private string Vertical;
        private string rStickV;
        private string rStickH;
        private string rTrigger;
        private string lTrigger;
        private bool isFireing = false;
        private bool isSwitching = false;
        private Vector2 stickVector;
        public float drag;

        public Rigidbody2D me;

        //player statistics
        public int Lives = 3;
        public PrimaryEnums.Color color = PrimaryEnums.Color.Yellow;
        public int Heat = 0;
        public int Deaths = 0;
        public int Kills = 0;
        public Hud1Player HUD;
        public Shield shield;


        private int _score = 0;
        public int Score { get { return _score; } set { _score += value; } }
       
        // Use this for initialization
        void Start()
        {
            //player controller selection
            Horizontal = isPlayerOne ? "HorizontalP1" : "HorizontalP2";
            Vertical = isPlayerOne ? "VerticalP1" : "VerticalP2";
            rStickH = isPlayerOne ? "rStickHP1" : "rStickHP2";
            rStickV = isPlayerOne ? "rStickVP1" : "rStickVP2";
            rTrigger = isPlayerOne ? "rTriggerP1" : "rTriggerP2";
            lTrigger = isPlayerOne ? "lTriggerP1" : "lTriggerP2";

            me = rigidbody2D;

            HUD = GameObject.FindGameObjectWithTag("Hud").GetComponent<Hud1Player>();


        }
        void OnEnable()
        {
            Target = null;
            rigidbody2D.velocity = Vector2.zero;
            HP = MaxHp;
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            UpdateScore();
        }

        private void UpdateScore()
        {
            HUD.HP = HP;
            HUD.Lives = Lives;
            HUD.Score = Score;

            // color debugging
            HUD.color = color.ToString(); //if (HUD.Color.text != color.ToString()) 
        }

        private void HandleInput()
        {
            #region movement
            if (Input.GetAxis(Horizontal) > 0.2f || Input.GetAxis(Horizontal) < -0.2f)
                Speed.x += Input.GetAxis(Horizontal) * Acceleration * Time.deltaTime;

            if (Input.GetAxis(Vertical) > 0.2f || Input.GetAxis(Vertical) < -0.2f)
                Speed.y += Input.GetAxis(Vertical)  * Acceleration * Time.deltaTime;


            if (Speed.x > MaxSpeed) Speed.x = MaxSpeed;
            if (Speed.y > MaxSpeed) Speed.y = MaxSpeed;
            me.velocity = Speed;
            Speed *= drag;
            #endregion

            #region rotation
            if (Input.GetAxis(rStickH) > 0.2f || Input.GetAxis(rStickH) < -0.2f)
                stickVector.x = Input.GetAxis(rStickH);
            if (Input.GetAxis(rStickV) > 0.2f || Input.GetAxis(rStickV) < -0.2f)
                stickVector.y = Input.GetAxis(rStickV);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(stickVector.y, stickVector.x) * Mathf.Rad2Deg + 90f, -Vector3.forward);
            #endregion

            #region fire
            //normal fire
            if (SelectedWeapon != 2 && Input.GetAxis(rTrigger) > 0.25f && !isFireing)
            {
                isFireing = true;
                InvokeRepeating("Fire", 0.1f, FireRate);
            }
            if (SelectedWeapon != 2 && Input.GetAxis(rTrigger) < 0.25f && isFireing)
            {
                isFireing = false;
                CancelInvoke("Fire");
            }

            //smart fire
            if (SelectedWeapon == 2 && Input.GetAxis(rTrigger) > 0.25f && !isFireing)
            {
                isFireing = true;
                tField.SetActive(true);
            }
            if (SelectedWeapon == 2 && Input.GetAxis(rTrigger) < 0.25f && isFireing)
            {
                Target = tField.GetComponent<TargettingField>().Targets;
                isFireing = false;
                Invoke("Fire",0.1f);
                //CancelInvoke("Fire");
            }
            //fire debug
            if(Input.GetKeyDown(KeyCode.Space) && !isFireing)
            {
                //Debug.Log("should be firing");
                isFireing = true;
                Fire();
                //InvokeRepeating("Fire", 0.1f, FireRate);
            }
            if (Input.GetKeyUp(KeyCode.Space) && isFireing)
            {
                //Debug.Log("Should be done firing");
                isFireing = false;
                CancelInvoke("Fire");
            }
            #endregion

            #region color swap
            //switch weapons. colors in the future
            if (Input.GetAxis(lTrigger) > 0.25f && !isSwitching && !isFireing)
            {
                isSwitching = true;
                SwitchColors();
            }
            if (Input.GetAxis(lTrigger) < 0.25f && isSwitching && !isFireing)
            {
                isSwitching = false;
            }
            //switch weapons debug
            if (Input.GetKeyDown(KeyCode.LeftAlt) && !isFireing)
            {
                isSwitching = true;
                SwitchColors();
            }
            if (Input.GetKeyUp(KeyCode.LeftAlt) && isFireing)
            {
                isSwitching = false;
            }
            #endregion
        }

        private void SwitchColors()
        {
            //SelectedWeapon++;
            if (++SelectedWeapon == Weapons.Count)
            {
                SelectedWeapon = 0;
            }
            if ((int)++color == PrimaryEnums.Color.GetValues(typeof(PrimaryEnums.Color)).Length) color = PrimaryEnums.Color.Yellow;
            shield.SetColor(color);
        }

        public void Fire()
        {
            if (SelectedWeapon == 2 && Target != null)
            {
                for (int i = 0; i < Target.Count; i++)
                {
                    foreach (WeaponMount m in Weapons[SelectedWeapon].GetComponents<WeaponMount>())
                    {
                        m.Fire(Target[i]);
                    } 
                }
                Target.Clear();
                tField.SetActive(false);
                return;
            }
            foreach (WeaponMount m in Weapons[SelectedWeapon].GetComponents<WeaponMount>())
            {
                m.Fire();
            }
        }

        public override void OnCollisionEnter2D(Collision2D c)
        {
            string t = c.gameObject.tag;
            if (t == "Terrain")
            {
                rigidbody2D.velocity = -rigidbody2D.velocity;
                return;
            }
        }
        
        public override void Kill()
        {
            CancelInvoke("Fire");
            isFireing = false;
            gameObject.SetActive(false);

        }
    }
}

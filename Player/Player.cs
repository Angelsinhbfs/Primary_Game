using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;
using System.Collections.Generic;
using Assets.Scripts.Weapon;
using Assets.Scripts.Utility;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.Player
{
    public class Player : Entity
    {
        #region variables
        //functional stats
        public int MaxHp;
        public Vector2 Speed;
        public float FireRate;
        public bool isPlayerOne;
        public bool isVersus;
        public int InvulnerableTime;
        private bool paused;

        public bool isRespawning = false;
        private PolygonCollider2D _collider;
        private SpriteRenderer shipRend;
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
        private string rBumper;
        private string lBumper;
        private bool isFireing = false;
        private bool isSwitching = false;
        private Vector2 stickVector;
        public float drag;
        public float rotationSpeed;
        public Vector2[] Borders;
        private Vector3 screenExtremes;

        public Rigidbody2D me;

        //player statistics
        public int Lives = 3;
        public PrimaryEnums.Color color = PrimaryEnums.Color.Yellow;
        public int Heat = 0;
        public int Deaths = 0;
        public int Kills = 0;
        public Hud HUD;
        public Shield shield;

        //shield stats
        public bool shieldActive = true;
        public int MaxShieldHp;
        public int RedHp;
        public int BlueHp;
        public int YellowHp;
        #endregion
        public int HPShields
        {
            get { return HP; }
            set
            {
                if (shieldActive)
                {
                    switch (color)
                    {
                        case PrimaryEnums.Color.Yellow:
                            YellowHp += value;
                            if (YellowHp > MaxShieldHp) YellowHp = MaxShieldHp;
                            break;
                        case PrimaryEnums.Color.Red:
                            RedHp += value;
                            if (RedHp > MaxShieldHp) RedHp = MaxShieldHp;
                            break;
                        case PrimaryEnums.Color.Blue:
                            //Debug.Log("shield hit");
                            BlueHp += value;
                            if (BlueHp > MaxShieldHp) BlueHp = MaxShieldHp;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    HP += value;
                }
            }
        }

        public bool isInvuln = false;


        private int _score = 0;
        public int Score { get { return _score; } set { _score += value; } }
       
        // Use this for initialization
        void Awake()
        {
            _collider = GetComponent<PolygonCollider2D>();
            shipRend = GetComponentInChildren<SpriteRenderer>();
        }
        void Start()
        {
            //player controller selection
            Horizontal = isPlayerOne ? "HorizontalP1" : "HorizontalP2";
            Vertical = isPlayerOne ? "VerticalP1" : "VerticalP2";
            rStickH = isPlayerOne ? "rStickHP1" : "rStickHP2";
            rStickV = isPlayerOne ? "rStickVP1" : "rStickVP2";
            rTrigger = isPlayerOne ? "rTriggerP1" : "rTriggerP2";
            lTrigger = isPlayerOne ? "lTriggerP1" : "lTriggerP2";
            rBumper = isPlayerOne ? "rBumperP1" : "rBumperP2";
            lBumper = isPlayerOne ? "lBumperP1" : "lBumperP2";

            me = rigidbody2D;

            HUD = GameObject.FindGameObjectWithTag("Hud").GetComponent<Hud>();
            if (!isPlayerOne)
            {
                HUD.enableP2Hud = true;
            }

            RedHp = MaxShieldHp;
            BlueHp = MaxShieldHp;
            YellowHp = MaxShieldHp;

            _collider = GetComponent<PolygonCollider2D>();

            if(!isVersus) screenExtremes = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
            if (!isPlayerOne) gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;

        }
        void OnEnable()
        {
            Target = null;
            rigidbody2D.velocity = Vector2.zero;
            HP = MaxHp;
            RedHp = MaxShieldHp;
            YellowHp = MaxShieldHp;
            BlueHp = MaxShieldHp;
            StartCoroutine(invulnToggle(InvulnerableTime));
        }

        // Update is called once per frame
        void Update()
        {
            if (!paused)
            {
                HandleInput();
                UpdateScore();
                HandleShield(); 
            }
            
        }

        private void HandleShield()
        {
            switch (color)
            {
                case PrimaryEnums.Color.Yellow:
                    if (YellowHp <= 0)
                    {
                        shield.gameObject.SetActive(false);
                        shieldActive = false;
                    }
                    else
                    {
                        shield.gameObject.SetActive(true);
                        shieldActive = true;
                    }
                    break;
                case PrimaryEnums.Color.Red:
                    if (RedHp <= 0)
                    {
                        shield.gameObject.SetActive(false);
                        shieldActive = false;
                    }
                    else
                    {
                        shield.gameObject.SetActive(true);
                        shieldActive = true;
                    }
                    break;
                case PrimaryEnums.Color.Blue:
                    if (BlueHp <= 0)
                    {
                        shield.gameObject.SetActive(false);
                        shieldActive = false;
                    }
                    else
                    {
                        shield.gameObject.SetActive(true);
                        shieldActive = true;
                    }
                    break;
                default:
                    break;
            }

            
        }

        private void UpdateScore()
        {
            if (isPlayerOne)
            {
                HUD.P1HP = HP;
                HUD.P1Lives = Lives;
                HUD.P1Score = Score;
                switch (color)
                {
                    case PrimaryEnums.Color.Yellow:
                        HUD.P1color = Color.yellow;
                        HUD.P1Shields = YellowHp;
                        break;
                    case PrimaryEnums.Color.Red:
                        HUD.P1color = Color.red;
                        HUD.P1Shields = RedHp;
                        break;
                    case PrimaryEnums.Color.Blue:
                        HUD.P1color = Color.blue;
                        HUD.P1Shields = BlueHp;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                HUD.P2HP = HP;
                HUD.P2Lives = Lives;
                HUD.P2Score = Score;
                switch (color)
                {
                    case PrimaryEnums.Color.Yellow:
                        HUD.P2color = Color.yellow;
                        HUD.P2Shields = YellowHp;
                        break;
                    case PrimaryEnums.Color.Red:
                        HUD.P2color = Color.red;
                        HUD.P2Shields = RedHp;
                        break;
                    case PrimaryEnums.Color.Blue:
                        HUD.P2color = Color.blue;
                        HUD.P2Shields = BlueHp;
                        break;
                    default:
                        break;
                }
            }
            

            // color debugging
            //HUD.color = color.ToString(); //if (HUD.Color.text != color.ToString()) 
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
            if (!isVersus)
            {
                var min = Camera.main.ScreenToWorldPoint(Vector3.zero);
                var max = Camera.main.ScreenToWorldPoint(screenExtremes);
                var ScreenPos = new Vector2(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y));
                transform.position = new Vector2(Mathf.Clamp(ScreenPos.x, Borders[0].x, Borders[2].x), Mathf.Clamp(ScreenPos.y, Borders[3].y, Borders[1].y));
            }
            else
                transform.position = new Vector2(Mathf.Clamp(transform.position.x, Borders[0].x, Borders[2].x), Mathf.Clamp(transform.position.y, Borders[3].y, Borders[1].y));
            
            #endregion

            #region rotation
            if (Input.GetAxis(rStickH) > 0.2f || Input.GetAxis(rStickH) < -0.2f)
                stickVector.x = Input.GetAxis(rStickH);
            if (Input.GetAxis(rStickV) > 0.2f || Input.GetAxis(rStickV) < -0.2f)
                stickVector.y = Input.GetAxis(rStickV);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(stickVector.y, stickVector.x) * Mathf.Rad2Deg + 90f, -Vector3.forward), Time.deltaTime * rotationSpeed);
            #endregion

            #region fire
            //normal fire
            if (SelectedWeapon != 2 && (Input.GetAxis(rTrigger) > 0.25f || Input.GetButtonDown(rBumper) ) && !isFireing)
            {
                isFireing = true;
                tField.SetActive(false);
                InvokeRepeating("Fire", 0.1f, FireRate);
            }
            if (SelectedWeapon != 2 && (Input.GetAxis(rTrigger) < 0.25f || Input.GetButtonUp(rBumper)) && isFireing)
            {
                isFireing = false;
                CancelInvoke("Fire");
            }

            //smart fire
            if (SelectedWeapon == 2 && (Input.GetAxis(rTrigger) > 0.25f || Input.GetButtonDown(rBumper)) && !isFireing)
            {
                isFireing = true;
                tField.SetActive(true);
            }
            if (SelectedWeapon == 2 && (Input.GetAxis(rTrigger) < 0.25f || Input.GetButtonUp(rBumper)) && isFireing)
            {
                Target = tField.GetComponent<TargettingField>().Targets;
                tField.SetActive(false);
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
            switch (color)
            {
                case PrimaryEnums.Color.Yellow:
                    shield.SetColor(Color.yellow);
                    break;
                case PrimaryEnums.Color.Red:
                    shield.SetColor(Color.red);
                    break;
                case PrimaryEnums.Color.Blue:
                    shield.SetColor(Color.blue);
                    break;
                default:
                    break;
            }
            
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

        public override void TakeDamage(int dmg, PrimaryEnums.Color Color, GameObject Owner)
        {
            if (isInvuln) return;
            if (shieldActive && Color == color)
            {
                //Debug.Log("Shield boost triggered");
                HPShields = (int)(dmg * 0.5f);
            }
            else
            {
                //Debug.Log("shield hit");
                HPShields = -dmg;
            }
            base.TakeDamage(dmg, Color, Owner);
        }
        public override void Kill()
        {
            if (isPlayerOne)
                HUD.P1HP = 0;
            else
                HUD.P2HP = 0;
            CancelInvoke("Fire");
            isFireing = false;
            gameObject.SetActive(false);
            Deaths++;

        }

        IEnumerator invulnToggle(int seconds)
        {
            isInvuln = true;
            yield return StartCoroutine(Flash((float)seconds/12f,seconds));
            isInvuln = false;
        }

        IEnumerator Flash(float speed, int seconds)
        {
            yield return new WaitForSeconds(speed);
            shipRend.enabled = !shipRend.enabled;
            yield return new WaitForSeconds(speed);
            shipRend.enabled = !shipRend.enabled;
            if (seconds-- > 0)
            {
                yield return StartCoroutine(Flash(speed, seconds));
            }

        }

        public void OnPause()
        {
            paused = true;
            rigidbody2D.velocity = Vector2.zero;
            CancelInvoke();
        }
    }
}

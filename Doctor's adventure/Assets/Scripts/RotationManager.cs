using UnityEngine;
using System.Collections.Generic;
using System;

public class RotationManager : MonoBehaviour
{
    //Static változó hogy mindenhol nyomon lehessen követni azt hogy melyik irányba néz
    //és ahoz tartozó fok a forgatáshoz
    public static FacingDirection facingDirection;
    public static float degree = 0;

    //A platformok felvétele
    public Transform Level;

    //Más "építmény"-ek felvétele
    public Transform Building;

    //A legalsó szint, ahol spawnol a játékos
    public Transform Ground;

    //A láthatatlan kockák, meghatározása prefab-ból
    public GameObject InvisiCube;

    //A játékos számára változó
    public GameObject Player;

    //Változó, hogy mekkora kockákat használunk
    public float WorldUnits = 1.000f;

    //Lista a láthatatlan kockáknak
    private List<Transform> InvisiList = new List<Transform>();

    //A legutobbi nézési iránynak eltárolása
    private FacingDirection lastfacing;

    //A legutobbi mélység eltárolása
    private float lastDepth = 0f;

    //A játékos scriptjének tárolása
    private PlayerController pc;

    //Időzítő
    private float again = 0f;

    void Start()
    {
        facingDirection = FacingDirection.Front;    //Az alap nézési irány meghatározása
        pc = Player.GetComponent<PlayerController>();   //A játékos scriptjének felvétele
        UpdateLevel(true);  //A pályán lévő láthatatlan kockák frissítése
    }
    void Update()
    {
        if (!DialogueManager.prologue) return;  //Amig a dialógus megy, ne lehessen használni a különböző billentyűket

        if (GroundChecker.grounded && Mathf.Abs(Player.GetComponent<Rigidbody>().velocity.y) < 0.1f)    //Hogyha játékos platformon áll és "nem" mozog Y tengelyen
        {
            if (!BuildingBetweenPositionAndCamera(pc.transform.position) &&
                lastDepth != GetPlayerDepth() &&
                AxisRounding() &&
                MoveToClosestPlatformToCamera()) UpdateLevel(false); //Pálya frissítésének eldöntése
        }

        //'E'/'Q'-billentyű lenyomására forgatás jobbra/balra
        //Ha láthatatlan kockán állna a játékos, akkor a legközelebbi platformra áthelyezi
        //A nézési irány és az ahoz tartozó adatok frissítése
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GroundChecker.grounded &&
                !BuildingBetweenPositionAndCamera(pc.transform.position) &&
                AxisRounding()) MoveToClosestPlatformToCamera();
            lastfacing = facingDirection;
            lastDepth = GetPlayerDepth();
            facingDirection = RotateDirectionRight();
            UpdateLevel(false);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GroundChecker.grounded &&
                !BuildingBetweenPositionAndCamera(pc.transform.position) &&
                AxisRounding()) MoveToClosestPlatformToCamera();
            lastfacing = facingDirection;
            lastDepth = GetPlayerDepth();
            facingDirection = RotateDirectionLeft();
            UpdateLevel(false);
        }

        //Ha flatformon áll (és valamilyen okból a grounded hamis) akkor igazzá teszi a grounded változót (timerrel)
        if (!GroundChecker.grounded && (OnPlatform(Level) || OnInvisiblePlatform() || OnPlatform(Ground)) && Time.time > again )
        {
            again = Time.time + 0.1f;
            GroundChecker.grounded = true;
        }
        //Ha nem áll platformon, akkor a grounded legyen hamis
        else if (!(OnPlatform(Level) || OnInvisiblePlatform()) && !OnPlatform(Ground))
        {
            GroundChecker.grounded = false;
        }
    }

    // A meglévő láthatatlan kockák törlése, majd új kockák létrehozása
    public void UpdateLevel(bool forceRebuild)
    {
        if (!forceRebuild)
            if (lastfacing == facingDirection && lastDepth == GetPlayerDepth())
                return;
        if(!BuildingBetweenPositionAndCamera(pc.transform.position) 
            && GetPlayerDepth()!=ClosestDepthToCamera() 
            && AxisRounding()) MoveToClosestDepthToCamera();
        else lastDepth = GetPlayerDepth();
        foreach (Transform il in InvisiList)
        {
            //A láthatatlan kockák törlése
            Destroy(il.gameObject);
        }
        //A lista tartalmának törlése
        InvisiList.Clear();

        //Új láthatatlan kockák készítése a játékos mélységén
        CreateInvisicubesAtNewDepth(GetPlayerDepth());
    }

    /// <summary>
    /// Megnézi hogy a játékos láthatatlan kockán áll-e
    /// </summary>
    private bool OnInvisiblePlatform()
    {
        foreach (Transform item in InvisiList)
        {
            if (Mathf.Abs(item.position.x - pc.transform.position.x) < WorldUnits - 0.3 && Mathf.Abs(item.position.z - pc.transform.position.z) < WorldUnits - 0.3)
                if (pc.transform.position.y - item.position.y <= WorldUnits + 0.4f && pc.transform.position.y - item.position.y > 0) return true;
        }
        return false;
    }

    /// <summary>
    /// Megnézi hogy a játékos platformon áll-e
    /// </summary>
    private bool OnPlatform(Transform group)
    {
        foreach (Transform item in group)
        {
            if (Mathf.Abs(item.position.x - pc.transform.position.x) < WorldUnits - 0.3 && Mathf.Abs(item.position.z - pc.transform.position.z) < WorldUnits - 0.3)
            {
                if (pc.transform.position.y - item.position.y <= WorldUnits + 0.4f && pc.transform.position.y - item.position.y > 0) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Egész számra kerekíti a karakter helyzetét az adott tengelyen
    /// </summary>
    public bool AxisRounding()
    {
        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back) 
        {
            Player.transform.position = new Vector3(Mathf.Round(Player.transform.position.x), Player.transform.position.y, Player.transform.position.z);
            return true;
        }
        else if (facingDirection == FacingDirection.Left || facingDirection == FacingDirection.Right)
        {
            Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Mathf.Round(Player.transform.position.z));
            return true;
        }
        return false;
    }

    /// <summary>
    /// A legközelebbi platfromhoz helyezi a játékost
    /// </summary>
    private bool MoveToClosestPlatformToCamera()
    {
        Vector3 newPos = Vector3.zero;
        foreach (Transform item in Level)
        {
            if (pc.transform.position.y - item.position.y <= WorldUnits + 0.4f && pc.transform.position.y > item.position.y)
            {
                if (facingDirection == FacingDirection.Front)
                {
                    if (Mathf.Abs(item.position.x - pc.transform.position.x) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.z != item.position.z && newPos.z > item.position.z)))
                        newPos = new Vector3(pc.transform.position.x, pc.transform.position.y, item.position.z);
                }
                else if (facingDirection == FacingDirection.Back)
                {
                    if (Mathf.Abs(item.position.x - pc.transform.position.x) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.z != item.position.z && newPos.z < item.position.z)))
                        newPos = new Vector3(pc.transform.position.x, pc.transform.position.y, item.position.z);
                }
                else if (facingDirection == FacingDirection.Left)
                {
                    if (Mathf.Abs(item.position.z - pc.transform.position.z) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.x != item.position.x && newPos.x > item.position.x)))
                        newPos = new Vector3(item.position.x, pc.transform.position.y, pc.transform.position.z);
                }
                else if (facingDirection == FacingDirection.Right)
                {
                    if (Mathf.Abs(item.position.z - pc.transform.position.z) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.x != item.position.x && newPos.x < item.position.x)))
                        newPos = new Vector3(item.position.x, pc.transform.position.y, pc.transform.position.z);
                }
            }
        }
        if (newPos==Vector3.zero)
        {
            foreach (Transform item in Ground)
            {
                if (facingDirection == FacingDirection.Front)
                {
                    if (Mathf.Abs(item.position.x - pc.transform.position.x) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.z != item.position.z && newPos.z > item.position.z)))
                        newPos = new Vector3(pc.transform.position.x, pc.transform.position.y, item.position.z);
                }
                else if (facingDirection == FacingDirection.Back)
                {
                    if (Mathf.Abs(item.position.x - pc.transform.position.x) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.z != item.position.z && newPos.z < item.position.z)))
                        newPos = new Vector3(pc.transform.position.x, pc.transform.position.y, item.position.z);
                }
                else if (facingDirection == FacingDirection.Left)
                {
                    if (Mathf.Abs(item.position.z - pc.transform.position.z) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.x != item.position.x && newPos.x > item.position.x)))
                        newPos = new Vector3(item.position.x, pc.transform.position.y, pc.transform.position.z);
                }
                else if (facingDirection == FacingDirection.Right)
                {
                    if (Mathf.Abs(item.position.z - pc.transform.position.z) < WorldUnits + 0f 
                        && (newPos == Vector3.zero || (newPos.x != item.position.x && newPos.x < item.position.x)))
                        newPos = new Vector3(item.position.x, pc.transform.position.y, pc.transform.position.z);
                }
            }
        }
        if (newPos == Vector3.zero) return false;
        lastDepth = GetPlayerDepth();
        pc.transform.position = newPos;
        return true;
    }

    public void MoveToClosestDepthToCamera()
    {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back) Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, ClosestDepthToCamera());
            else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left) Player.transform.position = new Vector3(ClosestDepthToCamera(), Player.transform.position.y, Player.transform.position.z);
    }

    /// <summary>
    /// Megkeresi a legközelebbi mélységet a kamerához ahol található platform
    /// </summary>
    private float ClosestDepthToCamera()
    {
        float depth=0;

        foreach (Transform item in Level)
        {
            if (facingDirection==FacingDirection.Front && depth>item.position.z)
            {
                depth = item.position.z;
            }
            else if (facingDirection == FacingDirection.Back && depth < item.position.z)
            {
                depth = item.position.z;
            }
            else if (facingDirection == FacingDirection.Left && depth>item.position.x)
            {
                depth = item.position.x;
            }
            else if (facingDirection == FacingDirection.Right && depth< item.position.x)
            {
                depth = item.position.x;
            }
        }


        return depth;
    }

    /// <summary>
    /// Megnézi hogy a megadott pozíció helyén van-e láthatatlan kocka
    /// </summary>
    private bool FindTransformInvisiList(Vector3 cube)
    {
        foreach (Transform item in InvisiList)
        {
            if (item.position == cube)
                return true;
        }
        return false;

    }

    /// <summary>
    /// Megnézi hogy a megadott pozíció helyén van-e platform
    /// </summary>
    private bool FindTransformLevel(Vector3 cube)
    {
        foreach (Transform item in Level)
        {
            if (item.position == cube) return true;
        }
        return false;

    }

    /// <summary>
    /// Megnézi, hogy van e építmény a játékos és a kamera között
    /// </summary>
    private bool BuildingBetweenPositionAndCamera(Vector3 pos)
    {
        foreach (Transform item in Building)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (item.position.z < pos.z &&
                    Mathf.Abs(item.position.x - pos.x) < WorldUnits - 0.2f &&
                    item.position.y == Mathf.Round(pos.y)) return true;
            }

            else if (facingDirection == FacingDirection.Right)
            {
                if (item.position.x > pos.x &&
                    Mathf.Abs(item.position.z - pos.z) < WorldUnits - 0.2f &&
                    item.position.y == Mathf.Round(pos.y)) return true;
            }

            else if (facingDirection == FacingDirection.Back)
            {
                if (item.position.z > pos.z &&
                    Mathf.Abs(item.position.x - pos.x) < WorldUnits - 0.2f &&
                    item.position.y == Mathf.Round(pos.y)) return true;
            }

            else if (facingDirection == FacingDirection.Left)
            {
                if (item.position.x < pos.x &&
                    Mathf.Abs(item.position.z - pos.z) < WorldUnits - 0.2f &&
                    item.position.y == Mathf.Round(pos.y)) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Láthatatlan kocka létrehozása
    /// </summary>
    private Transform CreateInvisicube(Vector3 position)
    {
        GameObject go = Instantiate(InvisiCube) as GameObject;

        go.transform.position = position;

        return go.transform;

    }

    /// <summary>
    /// Létrehoz láthatatlan kockákat, majd a megfelelő helyre rakja
    /// </summary>
    private void CreateInvisicubesAtNewDepth(float newDepth)
    {
        Vector3 tempCube = Vector3.zero;
        foreach (Transform l in Level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                tempCube = new Vector3(l.position.x, l.position.y, newDepth);
                if (!FindTransformInvisiList(tempCube) 
                    && !FindTransformLevel(tempCube) 
                    && !BuildingBetweenPositionAndCamera(l.position) 
                    && !BuildingBetweenPositionAndCamera(tempCube))
                {
                    Transform go = CreateInvisicube(tempCube);
                    InvisiList.Add(go);
                }
            }
            else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
            {
                tempCube = new Vector3(newDepth, l.position.y, l.position.z);
                if (!FindTransformInvisiList(tempCube) 
                    && !FindTransformLevel(tempCube) 
                    && !BuildingBetweenPositionAndCamera(l.position) 
                    && !BuildingBetweenPositionAndCamera(tempCube))
                {
                    Transform go = CreateInvisicube(tempCube);
                    InvisiList.Add(go);
                }
            }
        }
    }

    /// <summary>
    /// A játékos mélységének meghatározása
    /// </summary>
    private float GetPlayerDepth()
    {
        float closestPoint = 0f;

        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back) closestPoint = pc.transform.position.z;
        else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left) closestPoint = pc.transform.position.x;

        return Mathf.Round(closestPoint);
    }

    /// <summary>
    /// Jobbra forgatás
    /// </summary>
    private FacingDirection RotateDirectionRight()
    {
        int change = (int)(facingDirection);
        change++;
        if (change > 3) change = 0;
        degree -= 90f;
        return (FacingDirection)(change);
    }

    /// <summary>
    /// Balra forgatás
    /// </summary>
    private FacingDirection RotateDirectionLeft()
    {
        int change = (int)(facingDirection);
        change--;
        if (change < 0) change = 3;
        degree += 90f;
        return (FacingDirection)(change);
    }
}

/// <summary>
/// Enum a nézési irány nyomon követésére
/// </summary>
public enum FacingDirection
{
    Front = 0,
    Right = 1,
    Back = 2,
    Left = 3
}
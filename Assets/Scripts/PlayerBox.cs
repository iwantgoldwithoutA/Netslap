using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class PlayerBox : NetworkBehaviour
{
    public static PlayerBox localPlayer;
    [SyncVar] public string matchID;

    [SyncVar(hook = nameof(AttackHook))]
    public bool IsAttack = false;

    private NetworkMatch networkMatch;

    public Rigidbody RG;
    private void Start()
    {
        networkMatch = GetComponent<NetworkMatch>();

        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
            MainMenu.instance.SpawnPlayerUIPrefab(this);
        }

        DontDestroyOnLoad(this);
    }

    public void HostGame()
    {
        string ID = MainMenu.GetRandomID();
        CmdHostGame(ID);
    }

    [Command]
    public void CmdHostGame(string ID)
    {
        matchID = ID;
        if (MainMenu.instance.HostGame(ID, gameObject))
        {
            Debug.Log("HostGame");
            networkMatch.matchId = ID.ToGuid();
            TargetHostGame(true, ID);
        }
        else
        {
            Debug.Log("HostGame Fail");
            TargetHostGame(false, ID);
        }
    }

    [TargetRpc]
    void TargetHostGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log($"ID {matchID} == {ID}");
        MainMenu.instance.HostSuccess(success, ID);
    }

    public void JoinGame(string inputID)
    {
        CmdJoinGame(inputID);
    }

    [Command]
    public void CmdJoinGame(string ID)
    {
        matchID = ID;
        if (MainMenu.instance.JoinGame(ID, gameObject))
        {
            Debug.Log("JoinGame");
            networkMatch.matchId = ID.ToGuid();
            TargetJoinGame(true, ID);
        }
        else
        {
            Debug.Log("JoinGame Fail");
            TargetJoinGame(false, ID);
        }
    }

    [TargetRpc]
    void TargetJoinGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log($"ID {matchID} == {ID}");
        MainMenu.instance.JoinSuccess(success, ID);
    }

    public void BeginGame()
    {
        CmdBeginGame();
    }

    [Command]
    public void CmdBeginGame()
    {
        MainMenu.instance.BeginGame(matchID);
        Debug.Log("BeginGame");
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"ID {matchID} | Start");
        DontDestroyOnLoad(gameObject);
        MainMenu.instance.inGame = true;
        SceneManager.LoadSceneAsync("Game");
    }


    //Attack

    [Command(requiresAuthority = false)]
    public void CmdAttack(Transform point , float Speed)
    {
        IsAttack = true;

        

        GetComponent<Animator>().enabled = false;
        RG.GetComponent<Rigidbody>().AddForce(Speed * Vector3.up, ForceMode.Impulse);

        StartCoroutine(wait_enable());
    }

    void AttackHook(bool _ , bool new_attack)
    {
        if (new_attack)
        {
            GetComponent<Animator>().enabled = false;
            StartCoroutine(wait_enable());
        }
    }

    IEnumerator wait_enable()
    {
        yield return new WaitForSeconds(3.0f);
        IsAttack = false;
        GetComponent<Animator>().enabled = true;
    }

}

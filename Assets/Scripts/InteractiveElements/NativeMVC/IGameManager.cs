using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    ManageStatus Status { get; }
    void Startup();
    void Setup(NetworkService service);
    }

public enum ManageStatus
{
    Shutdown,
    Initializing,
    Started,
}

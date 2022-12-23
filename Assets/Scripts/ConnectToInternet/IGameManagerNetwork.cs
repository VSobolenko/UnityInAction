using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManagerNetwork
{
    ManageStatus Status { get; }

    void Setup(NetworkService service);
}

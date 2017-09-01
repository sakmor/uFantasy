using UnityEngine;
using System.Collections;

// 遊戲子系統共用界面
public abstract class IGameSystem
{
    protected mainGame mainGame = null;
    public IGameSystem(mainGame mainGame)
    {
        this.mainGame = mainGame;
    }

    public virtual void Initialize() { }
    public virtual void Release() { }
    public virtual void Update() { }

}

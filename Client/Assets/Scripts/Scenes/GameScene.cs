using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.Map.LoadMap(1);

        Screen.SetResolution(640, 480, false);

        //gameobject player = managers.resource.instantiate("creature/player");
        //player.name = "player";
        //managers.object.add(player);


        //managers.ui.showsceneui<ui_inven>();
        //dictionary<int, data.stat> dict = managers.data.statdict;
        //gameobject.getoraddcomponent<cursorcontroller>();

        //gameobject player = managers.game.spawn(define.worldobject.player, "unitychan");
        //camera.main.gameobject.getoraddcomponent<cameracontroller>().setplayer(player);

        ////managers.game.spawn(define.worldobject.monster, "knight");
        //gameobject go = new gameobject { name = "spawningpool" };
        //spawningpool pool = go.getoraddcomponent<spawningpool>();
        //pool.setkeepmonstercount(2);
    }

    public override void Clear()
    {
        
    }
}

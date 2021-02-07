using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallonEventType
{
    StartGame,
    AdSuccess,
    EraseSuccess,
    BackToMainMenu,
    ReadyToFinish,
    GameoverWarning,
    CancleGameoverWarning,
    BalloonEnterFinishArea,
    BalloonExitFinishArea,

    FinishLaunch,
    Destroy,
    GameFinish,

    AddScore,
    ScoreChange,
    BeginUIInteract,
    EndUIInteract,

    BalloonInteractBegin,
    BalloonInteractDraging,
    BalloonInteractEnd,
}

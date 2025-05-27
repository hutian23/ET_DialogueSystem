[Root]
@RootInit:
BulletInit;
RegistMove: (Goam_Idle)
  MoveType: None;
EndMove:
RegistMove: (Goam_Burst)
  MoveType: None;
EndMove:
GotoBehavior: Goam_Idle;
return;

[Goam_Idle]
@Trigger:
return;

@Main:
SpawnAtPlayerPosition: -150000;
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
RegistCounter: 36;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 6;
  LoopSprite: Idle_2, 6;
  LoopSprite: Idle_3, 6;
EndLoopAnim:
GotoBehavior: Goam_Burst;

[Goam_Burst]
@Trigger:
return;

@Main:
PositionY: -85000;
WaitFrame: 1;
BBSprite: Burst_1, 4;
BBSprite: Burst_2, 4;
RegistCounter: 52;
BBSprite: Burst_3, 4;
BBSprite: Burst_4, 4;
ScreenShake: 750, 750, 12000, 15, 0;
BeginLoopAnim: (Counter: Value > 0)
  LoopSprite: Idle_1, 6;
  LoopSprite: Idle_2, 6;
  LoopSprite: Idle_3, 6;
EndLoopAnim:
BBSprite: End_1, 4;
BBSprite: End_2, 4;
BBSprite: End_3, 4;
BBSprite: End_4, 4;
Dispose;
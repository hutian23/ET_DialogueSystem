[Root]
@RootInit:
RegistMove: (HellsFang_Idle)
  MoveType: None;
EndMove:
GotoBehavior: HellsFang_Idle;
return;

[HellsFang_Idle]
@Main:
VFXPosition: -30000, 10000;
VFXSprite: Idle_1, 5;
VFXSprite: Idle_2, 3;
VFXSprite: Idle_3, 3;
VFXSprite: Idle_4, 3;
VFXSprite: Idle_5, 3;
VFXSprite: Idle_6, 3;
VFXSprite: Idle_7, 3;
Dispose;
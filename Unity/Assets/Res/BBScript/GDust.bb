[Root]
@RootInit:
VFXInit;
RegistMove: (GDust_Idle)
  MoveType: None;
EndMove:
GotoBehavior: GDust_Idle;
return;

[GDust_Idle]
@Trigger:
return;

@Main:
BBSprite: Idle_1, 2;
BBSprite: Idle_2, 2;
BBSprite: Idle_3, 2;
BBSprite: Idle_4, 4;
BBSprite: Idle_5, 5;
#TODO Dust的初始化指令
Dispose;
[Root]
@RootInit:
EffectInit;
RegistMove: (ADust_Idle)
  MoveType: None;
EndMove:
GotoBehavior: ADust_Idle;
return;

[ADust_Idle]
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
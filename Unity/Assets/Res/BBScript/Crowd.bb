[Root]
@RootInit:
EffectInit;
RegistMove: (Crowd_Idle)
  MoveType: None;
EndMove:
GotoBehavior: Crowd_Idle;
return;

[Crowd_Idle]
@Trigger:
return;

@Main:
BBSprite: Idle_1, 7;
BBSprite: Idle_2, 7;
BBSprite: Idle_3, 7;
BBSprite: Idle_4, 7;
BBSprite: Idle_5, 7;
Exit;
[Root]
@RootInit:
GlinInit;
RegistMove: (Zako1_Idle)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Teleport)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Dash)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Throw)
  MoveType: None;
EndMove:
RegistMove: (Zako1_Death)
  MoveType: None;
EndMove:
GotoBehavior: Zako1_Teleport;
return;

[Zako1_Idle]
@Trigger:
return;

@Main:
SetMarker: Loop;
BBSprite: Idle_1, 6;
BBSprite: Idle_2, 6;
BBSprite: Idle_3, 6;
BBSprite: Idle_4, 6;
BBSprite: Idle_5, 6;
BBSprite: Idle_6, 6;
BBSprite: Idle_7, 6;
BBSprite: Idle_8, 6;
BBSprite: Idle_9, 6;
BBSprite: Idle_10, 6;
GotoMarker: Loop;
Exit;

[Zako1_Teleport]
@Trigger:
return;

@Main:
PlayTimeline: 0, 40;
GotoBehavior: Zako1_Death;

[Zako1_Dash]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 10;
BBSprite: Active_1, 5;
BBSprite: Active_2, 5;
BBSprite: Active_3, 5;
BBSprite: Active_4, 5;
BBSprite: Active_5, 5;
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
BBSprite: End_4, 5;
BBSprite: End_5, 5;
Exit;

[Zako1_Throw]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Anticipate_4, 5;
BBSprite: Anticipate_5, 10;
BBSprite: Active_1, 5;
BBSprite: Active_2, 5;
BBSprite: End_1, 5;
Exit;

[Zako1_Death]
@Trigger:
return;

@Main:
RegistCounter: 100;
BeginLoop: (Counter: Value > 0)
  BBSprite: Death_1, 5;
  BBSprite: Death_2, 5;
  BBSprite: Death_3, 5;
EndLoop:
BBSprite: End_1, 5;
BBSprite: End_2, 5;
BBSprite: End_3, 5;
BBSprite: End_4, 5;
BBSprite: End_5, 5;
BBSprite: End_6, 5;
BBSprite: End_7, 5;
Exit;

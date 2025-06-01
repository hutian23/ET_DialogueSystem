[Root]
@RootInit:
BulletInit;
RegistMove: (GlinSpike_Idle)
  MoveType: None;
EndMove:
GotoBehavior: GlinSpike_Idle;
return;

[GlinSpike_Idle]
@Trigger:
return;

@Main:
ScreenShake: 550, 550, 8000, 10, 0;
BBSprite: Anticipate_1, 5;
BBSprite: Anticipate_2, 5;
BBSprite: Anticipate_3, 5;
BBSprite: Start_1, 25;
BBSprite: Start_2, 3;
BBSprite: Start_3, 3;
BBSprite: Start_4, 3;
BBSprite: Start_5, 3;
ScreenShake: 1050, 1050, 10000, 20, 0;
BBSprite: Active_1, 60;
BBSprite: End_1, 3;
BBSprite: End_2, 3;
BBSprite: End_3, 3;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
Dispose;
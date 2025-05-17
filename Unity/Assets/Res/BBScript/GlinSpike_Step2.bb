[Root]
@RootInit:
BulletInit;
RegistMove: (GlinSpike_Step2_Idle)
  MoveType: None;
EndMove:
GotoBehavior: GlinSpike_Step2_Idle;
return;

[GlinSpike_Step2_Idle]
@Trigger:
return;

@Main:
BBSprite: Anticipate_1, 3;
BBSprite: Anticipate_2, 3;
BBSprite: Anticipate_3, 3;
BBSprite: Start_1, 3;
BBSprite: Start_2, 3;
BBSprite: Start_3, 3;
BBSprite: Start_4, 3;
BBSprite: Start_5, 3;
ScreenShake: 750, 750, 15000, 10;
BBSprite: Active_1, 5;
BBSprite: End_1, 3;
BBSprite: End_2, 3;
BBSprite: End_3, 3;
BBSprite: End_4, 3;
BBSprite: End_5, 3;
Dispose;
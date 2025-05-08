[Root]
@RootInit:
GlinInit;
SetPos: 0, -100000;
PoolObject: GlinSpike, 3;
RegistMove: (Glin_Slash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Capespike)
  MoveType: Normal;
EndMove:
GotoBehavior: Glin_Capespike;
return;

[Glin_Slash]
@Trigger:
return;

@Main:
SetPos: 100000, -100000;
BBSprite: Slash_1, 4;
BBSprite: Slash_2, 4;
BBSprite: Slash_3, 4;
BBSprite: Slash_4, 8;
# Dash
SetVelocityX: 600000;
BBSprite: Slash_5, 3;
SetVelocityX: 400000;
BBSprite: Slash_6, 3;
SetVelocityX: 200000;
BBSprite: Slash_7, 4;
SetVelocityX: 100000;
BBSprite: Slash_8, 2;
SetVelocityX: 50000;
BBSprite: Slash_8, 2;
SetVelocityX: 0;
BBSprite: Slash_9, 5;
# UpperCut_Start
BBSprite: UpperCut_Start_1, 4;
BBSprite: UpperCut_Start_2, 5;
BBSprite: UpperCut_Start_3, 5;
BBSprite: UpperCut_Start_4, 8;
# UpperCut Active
SetVelocity: 100000, 900000;
BBSprite: UpperCut_Active_1, 5;
SetVelocity: 50000, 600000;
BBSprite: UpperCut_Active_1, 4;
SetVelocity: 40000, 300000;
BBSprite: UpperCut_Active_1, 3;
SetVelocity: 10000, 100000;
BBSprite: UpperCut_Active_1, 3;
SetVelocity: 0, 40000;
BBSprite: UpperCut_Active_1, 3;
SetVelocityY: 10000;
BBSprite: UpperCut_Active_1, 3;
SetVelocity: 0, 0;
BBSprite: UpperCut_Active_2, 5;
# UpperCut End
BBSprite: UpperCut_End_1, 5;
BBSprite: UpperCut_End_2, 4;
Exit;

[Glin_Capespike]
@Trigger:
return;

@Main:
# first seven frames reversed for end
# Pre Cast
BBSprite: Capespike_1, 5;
BBSprite: Capespike_2, 5;
BBSprite: Capespike_3, 5;
BBSprite: Capespike_4, 5;
BBSprite: Capespike_5, 5;
BBSprite: Capespike_6, 5;
BBSprite: Capespike_7, 5;
ScreenShake: 450, 0, 8000, 10;
BBSprite: Capespike_8, 5;
# Cast Spike
RegistCounter: 150;
CastGlinSpike: 0, -3000;
BeginLoop: (Counter: Value > 0)
  BBSprite: Capespike_9, 7;
  BBSprite: Capespike_10, 7;
EndLoop:
# Cast End
BBSprite: Capespike_8, 5;
BBSprite: Capespike_7, 5;
BBSprite: Capespike_6, 5;
BBSprite: Capespike_5, 5;
BBSprite: Capespike_4, 5;
BBSprite: Capespike_3, 5;
BBSprite: Capespike_2, 5;
BBSprite: Capespike_1, 5;
Exit;
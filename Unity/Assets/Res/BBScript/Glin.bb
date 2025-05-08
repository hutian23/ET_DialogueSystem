[Root]
@RootInit:
GlinInit;
EnableAirCheck;
SetPos: 0, -100000;
PoolObject: GlinBullet, 5;
PoolObject: GlinSpike, 3;
RegistMove: (Glin_Slash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Capespike)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_AirDash)
  MoveType: Normal;
EndMove:
RegistMove: (Glin_Cast)
  MoveType: Normal;
EndMove:
GotoBehavior: Glin_Cast;
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

[Glin_AirDash]
@Trigger:
return;

@Main:
SetVelocity: 0, 0;
SetPos: 0, 50000;
# AirDash Start
BBSprite: AirDash_Anticipate_1, 3;
BBSprite: AirDash_Anticipate_2, 3;
BBSprite: AirDash_Anticipate_3, 3;
BBSprite: AirDash_Anticipate_4, 3;
BBSprite: AirDash_Anticipate_5, 3;
BBSprite: AirDash_Anticipate_6, 3;
BBSprite: AirDash_Anticipate_7, 3;
# AirDash Active
ApplyRotation: true;
SetVelocity: 450000, -700000;
BeginLoop: (InAir: true)
  BBSprite: AirDash_Active_1, 4;
  BBSprite: AirDash_Active_2, 4;
  BBSprite: AirDash_Active_3, 4;
EndLoop:
ApplyRotation: false;
# GroundDash Start
SetVelocity: 0, 0;
ScreenShake: 1550, 550, 10000, 15;
BBSprite: GroundDash_Anticipate_1, 5;
BBSprite: GroundDash_Anticipate_2, 5;
BBSprite: GroundDash_Anticipate_3, 5;
BBSprite: GroundDash_Anticipate_4, 10;
# GroundDash Active
SetVelocityX: 100000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 400000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 700000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 800000;
BBSprite: GroundDash_Active_1, 1;
SetVelocityX: 850000;
BBSprite: GroundDash_Active_2, 4;
BBSprite: GroundDash_Active_3, 4;
BBSprite: GroundDash_Active_4, 4;
SetVelocityX: 200000;
BBSprite: GroundDash_Anticipate_1, 5;
SetVelocityX: 80000;
BBSprite: GroundDash_Anticipate_2, 5;
SetVelocityX: 0;
BBSprite: GroundDash_Anticipate_3, 5;
BBSprite: GroundDash_Anticipate_4, 7;
Exit;

[Glin_Cast]
@Trigger:
return;

@Main:
# Start
BBSprite: Start_1, 4;
BBSprite: Start_2, 4;
BBSprite: Start_3, 4;
BBSprite: Start_4, 4;
# Active
CreateBullet: GlinBullet
  BulletPos: 0, 0;
EndCreateBullet:
BBSprite: Active_1, 4;
BBSprite: Active_2, 4;
BBSprite: Active_3, 4;
BBSprite: Active_4, 20;
BBSprite: Start_4, 4;
BBSprite: Start_3, 4;
BBSprite: Start_2, 4;
BBSprite: Start_1, 4;
Exit;
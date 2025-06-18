[Root]
@RootInit:
PoolObject: 'SlashRing', 4;
# 注册一个BehaviorInfo组件
RegistMove: (Boss_Idle)
  MoveType: None;
  EndMove:
RegistMove: (Boss_WallThrow)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Dash)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Dstab)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Entry)
  MoveType: None;
  EndMove:
RegistMove: (Boss_TrippleEntry)
  MoveType: None;
  EndMove:
RegistMove: (Boss_TrippleEntry_1)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Gesture)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Bow)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Wounded)
  MoveType: None;
  EndMove:
RegistMove: (Boss_Dead)
  MoveType: None;
  EndMove:
Test;
return;

[Boss_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
SetPos: 0, 100000;
BBSprite: 'Idle_1', 1;
return;

[Boss_Dash]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetFlip: {Self.Flip};
SetPos: {Self.PosX}, {Self.PosY};
BBSprite: 'Arrive_1', 3;
BBSprite: 'Arrive_2', 3;
BBSprite: 'Arrive_3', 3;
BBSprite: 'Arrive_4', 3;
BBSprite: 'Anticipate_1', 5;
BBSprite: 'Anticipate_2', 5;
BBSprite: 'Anticipate_3', 5;
BBSprite: 'Anticipate_4', 5;
BBSprite: 'Anticipate_5', 5;
BBSprite: 'Anticipate_6', 5;
SetVelocityX: {Self.VelX};
BBSprite: 'Dash_1', 3;
BBSprite: 'Dash_2', 3;
BeginLoop: (ReachX: {Self.ReachX}, {Self.Direction})
  BBSprite: 'Dash_3', 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Recover_1', 3;
BBSprite: 'Recover_2', 3;
BBSprite: 'Recover_3', 3;
BBSprite: 'Recover_4', 3;
BBSprite: 'Recover_5', 3;
BBSprite: 'Recover_6', 3;
BBSprite: 'Recover_7', 3;
BBSprite: 'Recover_8', 3;
BBSprite: 'Recover_9', 3;
BBSprite: 'Recover_10', 3;
BBSprite: 'Leave_1', 3;
BBSprite: 'Leave_2', 5;
GotoBehavior: 'Boss_Idle';

[Boss_Dstab]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
Test_SetPlayerPosY: 13000;
BBSprite: 'Arrive_1', 4;
BBSprite: 'Arrive_2', 4;
BBSprite: 'Arrive_3', 4;
BBSprite: 'Arrive_4', 4;
BBSprite: 'Anticipate_1', 3;
BBSprite: 'Anticipate_2', 3;
BBSprite: 'Anticipate_3', 3;
BBSprite: 'Anticipate_4', 3;
BBSprite: 'Anticipate_5', 3;
BBSprite: 'Anticipate_6', 3;
BBSprite: 'Anticipate_7', 3;
SetVelocityY: -130000;
BBSprite: 'Dstab_1', 2;
BBSprite: 'Dstab_2', 6;
SetVelocityY: 0;
BBSprite: 'Land_1', 4;
BBSprite: 'Land_2', 4;
BBSprite: 'Land_3', 4;
BBSprite: 'Land_4', 6;
BBSprite: 'Land_5', 6;
BBSprite: 'Land_6', 6;
BBSprite: 'Leave_1', 4;
BBSprite: 'Leave_2', 4;
GotoBehavior: 'Boss_Idle';

[Boss_WallThrow]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
SetPos: {Self.PosX}, {Self.PosY};
SetFlip: {Self.Flip};
BBSprite: 'Arrive_1', 3;
BBSprite: 'Arrive_2', 3;
BBSprite: 'Arrive_3', 3;
BBSprite: 'Arrive_4', 3;
BBSprite: 'Anticipate_1', 4;
BBSprite: 'Anticipate_2', 4;
BBSprite: 'Anticipate_3', 4;
BBSprite: 'Anticipate_4', 4;
BBSprite: 'Anticipate_5', 4;
BBSprite: 'Anticipate_6', 4;
BBSprite: 'Anticipate_7', 4;
BBSprite: 'Anticipate_8', 4;
BBSprite: 'Throw_1', 1;
CreateBall: SlashRing
  BeginIf: (Flip: Left)
    BallPos: -5000, -1000;
    BallParam: VelX, 34000;
    BallParam: VelY, -3000;
    BallParam: Accel, -30000;
    BallParam: MaxV, 34000;
    EndIf:
  BeginIf: (Flip: Right)
    BallPos: 5000, -1000;
    BallParam: VelX, -34000;
    BallParam: VelY, -3000;
    BallParam: Accel, 30000;
    BallParam: MaxV, 34000;
    EndIf:
  EndCreateBall:
BBSprite: 'Throw_1', 7;
BBSprite: 'Throw_2', 4;
BBSprite: 'Throw_3', 4;
BBSprite: 'Recover_1', 4;
BBSprite: 'Recover_2', 4;
BBSprite: 'Leave_1', 4;
BBSprite: 'Leave_2', 4;
BBSprite: 'Leave_3', 4;
GotoBehavior: 'Boss_Idle';

[Boss_Entry]
@Trigger:
return;

@Main:
BBSprite: 'Entry_1', 5;
BBSprite: 'Entry_2', 5;
BBSprite: 'Entry_3', 70;
BBSprite: 'Entry_4', 4;
BBSprite: 'Entry_5', 4;
BBSprite: 'Entry_6', 4;
BBSprite: 'Entry_7', 4;
GotoBehavior: 'Boss_Idle';

[Boss_TrippleEntry]
@Trigger:
return;

@Main:
#Stand
BBSprite: 'Entry_1', 5;
BBSprite: 'Entry_2', 5;
BBSprite: 'Entry_3', 50;
#Look
BBSprite: 'Entry_4', 5;
BBSprite: 'Entry_5', 105;
#Gesture
BBSprite: 'Entry_6', 5;
BBSprite: 'Entry_7', 5;
BBSprite: 'Entry_8', 5;
BBSprite: 'Entry_9', 50;
BBSprite: 'Entry_10', 5;
BBSprite: 'Entry_11', 40;
#Leave
BBSprite: 'Entry_12', 3;
BBSprite: 'Entry_13', 3;
BBSprite: 'Entry_14', 3;
BBSprite: 'Entry_15', 3;
GotoBehavior: 'Boss_Idle';

[Boss_TrippleEntry_1]
@Main:
#Stand
BBSprite: 'Entry_1', 5;
BBSprite: 'Entry_2', 5;
BBSprite: 'Entry_3', 100;
#Gesture
BBSprite: 'Entry_6', 5;
BBSprite: 'Entry_7', 5;
BBSprite: 'Entry_8', 5;
BBSprite: 'Entry_9', 50;
BBSprite: 'Entry_10', 5;
BBSprite: 'Entry_11', 40;
#Leave
BBSprite: 'Entry_12', 3;
BBSprite: 'Entry_13', 3;
BBSprite: 'Entry_14', 3;
BBSprite: 'Entry_15', 3;
GotoBehavior: 'Boss_Idle';

[Boss_Gesture]
@Trigger:
return;

@Main:
SetPos: {Self.PosX}, {Self.PosY};
SetFlip: {Self.Flip};
BBSprite: 'Gesture_1', 10000;
GotoBehavior: 'Boss_Idle';

[Boss_Wounded]
@Trigger:
return;

@Main:
SetPos: {Self.PosX}, {Self.PosY};
SetFlip: {Self.Flip};
BBSprite: 'Wounded_1', 3;
BBSprite: 'Wounded_2', 3;
BBSprite: 'Wounded_3', 3;
BBSprite: 'Wounded_4', 3;
BBSprite: 'Wounded_5', 3;
BBSprite: 'Wounded_6', 3;
BBSprite: 'Wounded_7', 3;
BBSprite: 'Wounded_8', 3;
return;

[Boss_Bow]
@Trigger:
return;

@Main:
BBSprite: 'Bow_1', 5;
BBSprite: 'Bow_2', 5;
BBSprite: 'Bow_3', 5;
BBSprite: 'Bow_4', 50;
BBSprite: 'Bow_5', 5;
BBSprite: 'Bow_6', 5;
BBSprite: 'Bow_7', 5;
BBSprite: 'Bow_8', 5;
BBSprite: 'Bow_9', 5;
return;


[Boss_Dead]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
BBSprite: 'Dead_3', 5;
BBSprite: 'Dead_4', 4;
BBSprite: 'Dead_5', 4;
BBSprite: 'Dead_3', 4;
BBSprite: 'Dead_4', 4;
BBSprite: 'Dead_5', 4;
BBSprite: 'Dead_3', 4;
BBSprite: 'Dead_4', 4;
BBSprite: 'Dead_5', 4;
BBSprite: 'Dead_3', 4;
BBSprite: 'Dead_4', 4;
BBSprite: 'Dead_5', 4;
BBSprite: 'Dead_6', 5;
BBSprite: 'Dead_7', 5;
GotoBehavior: 'Boss_Idle';
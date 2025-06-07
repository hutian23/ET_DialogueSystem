[Root]
@RootInit:
VFXInit;
RegistMove: (HellsFang_Idle)
  MoveType: None;
EndMove:
GotoBehavior: HellsFang_Idle;
return;

[HellsFang_Idle]
@Main:
SkillVFXScale: 16000, 11000;
SkillVFXPosition: 35000, -1000;
SkillVFXSprite: Idle_1, 3;
SkillVFXPosition: 32000, 2500;
SkillVFXSprite: Idle_1, 5;
SkillVFXPosition: 2000, 3000;
SkillVFXSprite: Idle_2, 3;
SkillVFXPosition: -6000, 3000;
SkillVFXSprite: Idle_2, 3;
SkillVFXPosition: -30000, -3500;
SkillVFXSprite: Idle_3, 10;
SkillVFXSprite: Idle_4, 3;
SkillVFXSprite: Idle_5, 3;
SkillVFXSprite: Idle_6, 3;
SkillVFXSprite: Idle_7, 3;
Dispose;
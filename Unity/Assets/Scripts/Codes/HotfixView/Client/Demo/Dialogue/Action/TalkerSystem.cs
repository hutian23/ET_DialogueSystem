namespace ET.Client
{
    [FriendOf(typeof (Talker))]
    public static class TalkerSystem
    {
        public static void Init(this Talker self, string character, string idle_clip)
        {
            self.talker = character;
            self.idle_clip = idle_clip;
            self.talk_clip = $"{idle_clip}_talk";
        }

        public class TalkerDestroySystem: DestroySystem<Talker>
        {
            protected override void Destroy(Talker self)
            {
                CharacterManager characterManager = self.GetParent<CharacterManager>();
                Unit character = characterManager.GetCharacter(self.talker);
                character?.AnimPlay(self.idle_clip);
                
                self.talker = null;
                self.idle_clip = null;
                self.talk_clip = null;
            }
        }

        public class TalkerUpdateSystem: UpdateSystem<Talker>
        {
            protected override void Update(Talker self)
            {
                CharacterManager characterManager = self.GetParent<CharacterManager>();
                DialogueComponent dialogueComponent = characterManager.GetParent<DialogueComponent>();
                Unit character = characterManager.GetCharacter(self.talker);

                if (dialogueComponent.ContainTag(DialogueTag.TypeCor))
                {
                    character.AnimPlay_Repeat(dialogueComponent.ContainTag(DialogueTag.Typing)? self.talk_clip : self.idle_clip);
                }
                else
                {
                    character.AnimPlay(self.idle_clip);
                    self.Dispose();
                }
            }
        }
    }
}
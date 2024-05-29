using EntityStates.GlobalSkills.LunarDetonator;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;

namespace VeliaerisMod.Survivors.Veliaeris
{
    public static class HenryStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(BasicScytheSlash));

            Modules.Content.AddEntityState(typeof(Corrupt));

            Modules.Content.AddEntityState(typeof(Split));

            Modules.Content.AddEntityState(typeof(VoidDetonator));
        }
    }
}

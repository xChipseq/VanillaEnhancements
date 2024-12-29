using HarmonyLib;

namespace VanillaEnhancements.Patches;

[HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Update))]
public static class DummyVotePatch
{
    public static bool Prefix(DummyBehaviour __instance)
    {
        var playerData = __instance.myPlayer.Data;
        if (playerData == null || playerData.IsDead) return true;

        if (!MeetingHud.Instance)
        {
            __instance.voted = false;
        }
        else if (!__instance.voted)
        {
            // Check if the local player has voted
            var localPlayerState = MeetingHud.Instance.playerStates[0];
            if (!localPlayerState.DidVote)
                return false;

            // Vote for the same player as the local player
            MeetingHud.Instance.CmdCastVote(playerData.PlayerId, localPlayerState.VotedFor);
            __instance.voted = true;
        }

        return false;
    }
}
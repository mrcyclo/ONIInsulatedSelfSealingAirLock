namespace ONIInsulatedSelfSealingAirLock
{
    // Dupe work animation when toggling the mod door (anim_use_remote_kanim).
    // Door.OnPrefabInit can overwrite overrideAnims with a static array that contains null
    // if Assets were not ready when the Door class loaded.
    internal static class InsulatedSelfSealingAirLockWorkerAnims
    {
        internal static void Apply(Door door)
        {
            KAnimFile remoteAnim = Assets.GetAnim("anim_use_remote_kanim");
            if (remoteAnim != null)
            {
                door.overrideAnims = new KAnimFile[] { remoteAnim };
            }
        }
    }
}

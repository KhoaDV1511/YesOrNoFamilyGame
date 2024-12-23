using com.unity3d.mediation;

public class UpdateProgressSignals : ASignal<int, int>{}
public class StartGameSignals : ASignal{}
public class UpDateHomeSignals : ASignal{}
public class LoadAdsSignal : ASignal<LevelPlayAdFormat, bool> { }
public class ShowAdsSignal : ASignal<LevelPlayAdFormat, bool> { }


public class UpdateProgressSignals : ASignal<int, int>{}
public class StartGameSignals : ASignal{}
public class UpDateHomeSignals : ASignal{}
public class LoadAdsSignal : ASignal<TypeAds, bool> { }
public class ShowAdsSignal : ASignal<bool> { }
public class UpdateCoinSignal : ASignal { }
package mono.com.google.android.gms.games.multiplayer.turnbased;


public class OnTurnBasedMatchUpdateReceivedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.games.multiplayer.turnbased.OnTurnBasedMatchUpdateReceivedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTurnBasedMatchReceived:(Lcom/google/android/gms/games/multiplayer/turnbased/TurnBasedMatch;)V:GetOnTurnBasedMatchReceived_Lcom_google_android_gms_games_multiplayer_turnbased_TurnBasedMatch_Handler:Android.Gms.Games.MultiPlayer.TurnBased.IOnTurnBasedMatchUpdateReceivedListenerInvoker, Xamarin.GooglePlayServices.Games\n" +
			"n_onTurnBasedMatchRemoved:(Ljava/lang/String;)V:GetOnTurnBasedMatchRemoved_Ljava_lang_String_Handler:Android.Gms.Games.MultiPlayer.TurnBased.IOnTurnBasedMatchUpdateReceivedListenerInvoker, Xamarin.GooglePlayServices.Games\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Games.MultiPlayer.TurnBased.IOnTurnBasedMatchUpdateReceivedListenerImplementor, Xamarin.GooglePlayServices.Games, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnTurnBasedMatchUpdateReceivedListenerImplementor.class, __md_methods);
	}


	public OnTurnBasedMatchUpdateReceivedListenerImplementor ()
	{
		super ();
		if (getClass () == OnTurnBasedMatchUpdateReceivedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Games.MultiPlayer.TurnBased.IOnTurnBasedMatchUpdateReceivedListenerImplementor, Xamarin.GooglePlayServices.Games, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTurnBasedMatchReceived (com.google.android.gms.games.multiplayer.turnbased.TurnBasedMatch p0)
	{
		n_onTurnBasedMatchReceived (p0);
	}

	private native void n_onTurnBasedMatchReceived (com.google.android.gms.games.multiplayer.turnbased.TurnBasedMatch p0);


	public void onTurnBasedMatchRemoved (java.lang.String p0)
	{
		n_onTurnBasedMatchRemoved (p0);
	}

	private native void n_onTurnBasedMatchRemoved (java.lang.String p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
